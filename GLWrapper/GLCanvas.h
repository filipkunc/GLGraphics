#pragma once

using namespace System;

namespace GLWrapper 
{
	ref class GLTexture;

	public ref class GLCanvas
	{
	public:
		void Clear(Color color);

		void EnableTexturing();
		void DisableTexturing();
		void EnableBlend();
		void DisableBlend();
		
		void SetCurrentColor(Color color);
		void SetLineWidth(float width);
		void SetPointSize(float size);
		void DrawLine(PointF a, PointF b);
		void DrawPoint(PointF a);
		void FillRectangle(RectangleF rect);
		void DrawRectangle(RectangleF rect);

		GLTexture ^CreateTexture(Bitmap ^bitmap);		
	};
}
