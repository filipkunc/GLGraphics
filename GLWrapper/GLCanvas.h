#pragma once

namespace GLWrapper 
{
	public ref class GLCanvas
	{
	public:
		void Clear(Color color);

		void EnableTexturing();
		void DisableTexturing();
		void EnableBlend();
		void DisableBlend();
		void EnableLineAntialiasing();
		void DisableLineAntialiasing();
		
		void SetCurrentColor(Color color);
		void SetLineWidth(float width);
		void SetPointSize(float size);
		void DrawLine(PointF a, PointF b);
		void DrawPoint(PointF a);
		void FillRectangle(RectangleF rect);
		void DrawRectangle(RectangleF rect);
		void DrawLines(array<PointF> ^points);
	};
}
