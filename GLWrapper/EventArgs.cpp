#include "stdafx.h"
#include "GLCanvas.h"
#include "EventArgs.h"

namespace GLWrapper
{
	CanvasEventArgs::CanvasEventArgs(GLCanvas ^canvas, PaintEventArgs ^gdiArgs)
		: PaintEventArgs(gdiArgs->Graphics, gdiArgs->ClipRectangle)
	{
		_canvas = canvas;
	}

    ExceptionEventArgs::ExceptionEventArgs(Exception ^exception)
        : EventArgs()
    {
        _exception = exception;
    }

    GLErrorEventArgs::GLErrorEventArgs(GLError error)
    {
        _error = error;
    }
}
