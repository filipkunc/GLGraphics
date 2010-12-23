#include "stdafx.h"
#include "GLCanvas.h"
#include "CanvasEventArgs.h"

namespace GLCanvas
{
	CanvasEventArgs::CanvasEventArgs(GLCanvas ^canvas)
	{
		this->canvas = canvas;
	}
}
