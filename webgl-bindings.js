﻿/// Generated by webgl-bindings.tt at 2023-10-08 11:09:17 UTC
/// Bindings are generated based on specification from webgl.spec.xml

let context = null;
let webGlDataIndex = -1;
const webGlData = {};
const uniformLocationsToWebGlDataIndex = {};

export const InitCanvasWebGL = (width, height) => { const canvas = document.createElement('canvas'); canvas.width = width; canvas.height = height; document.body.appendChild(canvas); context = canvas.getContext('webgl2'); };
export const glViewport = (x, y, width, height) => { context.viewport(x, y, width, height); };
export const glClearColor = (r, g, b, a) => { context.clearColor(r, g, b, a); };
export const glClear = (flags) => { context.clear(flags); };
export const glEnable = (mode) => { context.enable(mode); };
export const glGenVertexArrays = () => { webGlDataIndex++; webGlData[webGlDataIndex] = context.createVertexArray(); return webGlDataIndex; };
export const glBindVertexArray = (array) => { context.bindVertexArray(webGlData[array]); };
export const glVertexAttribPointer = (index, size, type, normalized, stride, offset) => { context.vertexAttribPointer(index, size, type, normalized, stride, offset); };
export const glEnableVertexAttribArray = (index) => { context.enableVertexAttribArray(index); };
export const glGenBuffers = () => { webGlDataIndex++; webGlData[webGlDataIndex] = context.createBuffer(); return webGlDataIndex; };
export const glBindBuffer = (target, buffer) => { context.bindBuffer(target, webGlData[buffer]); };
export const glBufferData = (target, size, jsonData, usage, type) => { const data = JSON.parse(jsonData); let typed = null; if (type === context.FLOAT) { typed = new Float32Array(data); } else if (type === context.UNSIGNED_BYTE) { typed = new Uint8Array(data); } else if (type === context.UNSIGNED_INT) { typed = new Uint32Array(data); } else { console.log('UNKNOWN type') } context.bufferData(target, typed, usage); };
export const glCreateShader = (type) => { webGlDataIndex++; webGlData[webGlDataIndex] = context.createShader(type); return webGlDataIndex; };
export const glShaderSource = (shader, source) => { context.shaderSource(webGlData[shader], source); };
export const glCompileShader = (shader) => { context.compileShader(webGlData[shader]); };
export const glGetShaderParameter = (shader, pname) => { return context.getShaderParameter(webGlData[shader], pname); };
export const glGetShaderInfoLog = (shader) => { return context.getShaderInfoLog(webGlData[shader]); };
export const glCreateProgram = () => { webGlDataIndex++; webGlData[webGlDataIndex] = context.createProgram(); return webGlDataIndex; };
export const glAttachShader = (program, shader) => { context.attachShader(webGlData[program], webGlData[shader]); };
export const glLinkProgram = (program) => { context.linkProgram(webGlData[program]); };
export const glGetUniformLocation = (program, uniformName) => { const locationKey = `${program}:${uniformName}`; const existingIndex = uniformLocationsToWebGlDataIndex[locationKey]; if (existingIndex !== undefined) { return existingIndex; } webGlDataIndex++; webGlData[webGlDataIndex] = context.getUniformLocation(webGlData[program], uniformName); uniformLocationsToWebGlDataIndex[locationKey] = webGlDataIndex; return webGlDataIndex; };
export const glUniformMatrix4fv = (uniformLocation, jsonData, transpose) => { context.uniformMatrix4fv( webGlData[uniformLocation], transpose, new Float32Array(JSON.parse(jsonData))); };
export const glDrawArrays = (mode, first, count) => { context.drawArrays(mode, first, count); };
export const glDrawElements = (mode, count, type, offset) => { context.drawElements(mode, count, type, offset); };
export const glUseProgram = (program) => { context.useProgram(webGlData[program]); };