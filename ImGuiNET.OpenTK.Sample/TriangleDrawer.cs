using OpenTK.Graphics.OpenGL;

namespace ImGuiNET.OpenTK.Sample;

public class TriangleDrawer
{
    public void OnLoad()
    {
        // Create a vertex buffer object (VBO) to store the triangle's vertices.
        GL.GenBuffers(1, out _vbo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 9, new float[]
        {
            0.0f, 0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f
        }, BufferUsageHint.StaticDraw);

        // Create a vertex array object (VAO) to store the vertex attribute information.
        GL.GenVertexArrays(1, out _vao);
        GL.BindVertexArray(_vao);

        // Specify the location of the position attribute.
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

        // Create a vertex shader.
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, "attribute vec3 position; void main() { gl_Position = vec4(position, 1.0); }");
        GL.CompileShader(vertexShader);

        // Create a fragment shader.
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, "void main() { gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0); }");
        GL.CompileShader(fragmentShader);

        // Link the shaders together into a shader program.
        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);

        GL.LinkProgram(_shaderProgram);

        // Delete the compiled shaders after lining them.
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

    }

    public void OnResize(int width, int height)
    {
        // Set the viewport to the size of the window.
        GL.Viewport(0, 0, width, height);
    }

    public void OnRenderFrame()
    {
        // Set the clear color for the FBO.
        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        // Clear the screen.
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Use the shader program.
        GL.UseProgram(_shaderProgram);

        // Use the VAO.
        GL.BindVertexArray(_vao);

        // Draw the triangle.
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public void OnClosed()
    {
        // Delete the vertex array object.
        GL.DeleteVertexArray(_vao);

        // Delete the vertex buffer object.
        GL.DeleteBuffer(_vbo);

        // Delete the shader program.
        GL.DeleteProgram(_shaderProgram);
    }

    private int _vbo;
    private int _vao;
    private int _shaderProgram;
}