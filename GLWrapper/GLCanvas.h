#pragma once

namespace GLWrapper 
{
	public ref class GLCanvas
	{
	private:
		bool _texture2DEnabled;
		bool _blendEnabled;
		bool _antialiasingEnabled;
		float _lineWidth;
		float _pointSize;
		Color _currentColor;
		Size _size;
		PointF _dpi;
	public:
		GLCanvas(Color backColor);

		property bool Texture2DEnabled { bool get(); void set(bool value); }
		property bool BlendEnabled { bool get(); void set(bool value); }
		property bool AntialiasingEnabled { bool get(); void set(bool value); }
		property float LineWidth { float get(); void set(float value); }
		property float PointSize { float get(); void set(float value); }
		property Color CurrentColor { Color get(); void set(Color value); }
		property Size CanvasSize { Size get(); void set(Size value); }
		property PointF Dpi { PointF get(); void set(PointF value); }
		
		void Clear(Color color);

		void DrawLine(Point a, Point b);
		void DrawLine(PointF a, PointF b);

		void DrawLines(array<Point> ^points);
		void DrawLines(array<PointF> ^points);

		void DrawPoint(PointF a);
		void DrawPoints(array<PointF> ^points);
		
		void DrawRectangle(RectangleF rect);
		void FillRectangle(RectangleF rect);

		void Identity();
		void Translate(float dx, float dy);
		void Rotate(float degrees);
		void Scale(float sx, float sy);
	};
}
