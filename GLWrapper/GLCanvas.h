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
		void DrawLines(array<PointF> ^points);

		void DrawPoint(PointF a);		
		
		void DrawRectangle(RectangleF rect);
		void FillRectangle(RectangleF rect);
	};
}
