using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace WasmWebGL;

public static partial class WebGL
{
    public static void glBufferData(int target, int size, List<float> data, int usage)
    {
        glBufferData(target, size, CollectionsMarshal.AsSpan(data), usage);
    }

    public static void glBufferData(int target, int size, byte[] data, int usage)
    {
        glBufferData(target, size, data.AsSpan(), usage);
    }

    public static void glBufferData(int target, int size, Span<byte> data, int usage)
    {
        glBufferData(target, size, ToJson(data), usage, GL_UNSIGNED_BYTE);
    }

    public static void glBufferData(int target, int size, float[] data, int usage)
    {
        glBufferData(target, size, data.AsSpan(), usage);
    }

    public static void glBufferData(int target, int size, Span<float> data, int usage)
    {
        glBufferData(target, size, ToJson(data), usage, GL_FLOAT);
    }

    public static void glUniformMatrix4fv(int uniformLocation, Matrix4x4 data)
    {
        unsafe
        {
            void* ptr = &data;
            glUniformMatrix4fv(uniformLocation, new Span<float>(ptr, 16), false);
        }
    }

    public static void glUniformMatrix4fv(int uniformLocation, float[] data, bool transpose)
    {
        glUniformMatrix4fv(uniformLocation, data.AsSpan(), transpose);
    }

    public static void glUniformMatrix4fv(int uniformLocation, Span<float> data, bool transpose)
    {
        glUniformMatrix4fv(uniformLocation, ToJson(data), transpose);
    }

    private static string ToJson<T>(Span<T> data)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("[");

        for (int i = 0; i < data.Length; i++)
        {
            stringBuilder.Append(data[i].ToString());

            if (i < data.Length - 1)
            {
                stringBuilder.Append(", ");
            }
        }

        stringBuilder.Append("]");

        return stringBuilder.ToString();
    }
}
