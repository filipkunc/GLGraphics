#include "stdafx.h"
#include "GLCanvas.h"
#include "CanvasEventArgs.h"

namespace GLWrapper
{
	CanvasEventArgs::CanvasEventArgs(GLCanvas ^canvas)
	{
		this->canvas = canvas;
	}
}
