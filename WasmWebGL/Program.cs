using System;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using static WasmWebGL.WebGL;

await JSHost.ImportAsync("webgl-bindings.js", "./webgl-bindings.js");

var canvasWidth = 720;
var canvasHeight = 720;

InitCanvasWebGL(canvasWidth, canvasHeight);

glViewport(0, 0, canvasWidth, canvasHeight);
glClearColor(0, 0, 0, 1);
glEnable(GL_DEPTH_TEST);

// VAO could be also used but since its only single 3d model its not needed
// var vao = glGenVertexArrays();
// glBindVertexArray(vao);

float[] vertices = {
    // https://en.wikipedia.org/wiki/Tetrahedron
     1.0f,  0.0f, -1 / MathF.Sqrt(2), 1f, 0f, 0f,
    -1.0f,  0.0f, -1 / MathF.Sqrt(2), 0f, 1f, 0f,
     0.0f,  1.0f,  1 / MathF.Sqrt(2), 0f, 0f, 1f,
     0.0f, -1.0f,  1 / MathF.Sqrt(2), 1f, 1f, 1f
};

var vbo = glGenBuffers();

glBindBuffer(GL_ARRAY_BUFFER, vbo);
glBufferData(
    GL_ARRAY_BUFFER,
    vertices.Length * sizeof(float),
    vertices,
    GL_STATIC_DRAW);

glVertexAttribPointer(0, 3, GL_FLOAT, 0, 6 * sizeof(float), 0);
glEnableVertexAttribArray(0);
glVertexAttribPointer(1, 3, GL_FLOAT, 0, 6 * sizeof(float), 3 * sizeof(float));
glEnableVertexAttribArray(1);

var ebo = glGenBuffers();
byte[] indices =
{
    0, 1, 2,
    0, 1, 3,
    0, 2, 3,
    1, 2, 3
};
glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
glBufferData(
    GL_ELEMENT_ARRAY_BUFFER,
    indices.Length * sizeof(byte),
    indices,
    GL_STATIC_DRAW);

var vertexShader = glCreateShader(GL_VERTEX_SHADER);
glShaderSource(vertexShader, """
    #version 300 es

    in vec3 vertex_position;
    in vec3 vertex_color_in;
    out vec3 vertex_color;

    uniform mat4 perspective;
    uniform mat4 model;

    void main() {
        gl_Position = perspective * model * vec4(vertex_position, 1.0);
        vertex_color = vertex_color_in;
    }
    """);
glCompileShader(vertexShader);

if (!glGetShaderParameter(vertexShader, GL_COMPILE_STATUS))
{
    Console.WriteLine("Failed to compile vertex shader");
    Console.WriteLine(glGetShaderInfoLog(vertexShader));
}

var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
glShaderSource(fragmentShader, """
    #version 300 es
    precision highp float;

    in vec3 vertex_color;
    out vec4 fragColor;

    void main()
    {
        fragColor = vec4(vertex_color, 1.0);
    }
    """);
glCompileShader(fragmentShader);

if (!glGetShaderParameter(fragmentShader, GL_COMPILE_STATUS))
{
    Console.WriteLine("Failed to compile fragment shader");
    Console.WriteLine(glGetShaderInfoLog(fragmentShader));
}

var shaderProgram = glCreateProgram();
glAttachShader(shaderProgram, vertexShader);
glAttachShader(shaderProgram, fragmentShader);
glLinkProgram(shaderProgram);
glUseProgram(shaderProgram);

var perspective = Matrix4x4.CreatePerspectiveFieldOfView(
    fieldOfView: AngleToRadians(45f),
    aspectRatio: (float)canvasWidth / canvasHeight,
    nearPlaneDistance: 0.1f,
    farPlaneDistance: 500f);

glUniformMatrix4fv(
    glGetUniformLocation(shaderProgram, "perspective"),
    perspective);

var loopStartTimestamp = DateTime.UtcNow;

while (true)
{
    var secondsElapsed = (float)DateTime.UtcNow.Subtract(loopStartTimestamp).TotalSeconds;

    var modelMatrix = Matrix4x4.CreateRotationY(secondsElapsed)
        * Matrix4x4.CreateRotationX(secondsElapsed / 2.0f)
        * Matrix4x4.CreateTranslation(0, 0, -5f);

    glUniformMatrix4fv(
        glGetUniformLocation(shaderProgram, "model"),
        modelMatrix);

    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glDrawElements(GL_TRIANGLES, indices.Length, GL_UNSIGNED_BYTE, 0);

    // Delay is needed to let JS do its job because otherwise page will freeze.
    // If there is a better way to let the main thread to do necessary "js things" then DM me.
    await Task.Delay(1);
}

static float AngleToRadians(float degrees) => degrees * (MathF.PI / 180f);
