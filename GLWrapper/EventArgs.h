#pragma once

namespace GLWrapper
{
	public ref class CanvasEventArgs : public PaintEventArgs
	{
	private:
		GLCanvas ^_canvas;		
	public:
		CanvasEventArgs(GLCanvas ^canvas, PaintEventArgs ^gdiArgs);

		property GLCanvas ^Canvas
		{ 
			GLCanvas ^get() { return _canvas; }
			void set(GLCanvas ^value) { _canvas = value; }
		}
	};

    public ref class ExceptionEventArgs : public EventArgs
    {
    private:
        Exception ^_exception;
    public:
        ExceptionEventArgs(Exception ^exception);

        property Exception ^Error
        {
            Exception ^get() { return _exception; }
            void set(Exception ^value) { _exception = value; }
        }
    };

    public ref class GLErrorEventArgs : public EventArgs
    {
    private:
        GLError _error;
    public:
        GLErrorEventArgs(GLError error);

        property GLError Error
        {
            GLError get() { return _error; }
            void set(GLError value) { _error = value; }
        }
    };
}
