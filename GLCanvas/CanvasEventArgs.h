#pragma once

namespace GLWrapper
{
	public ref class CanvasEventArgs : public EventArgs
	{
	private:
		GLCanvas ^canvas;
	public:
		CanvasEventArgs(GLCanvas ^canvas);

		property GLCanvas ^Canvas
		{ 
			GLCanvas ^get() { return canvas; }
			void set(GLCanvas ^value) { canvas = value; }
		}
	};
}
