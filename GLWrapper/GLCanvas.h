#pragma once

namespace GLWrapper 
{
	ref class GLMatrix2D;

	public ref class GLCanvas
	{
	private:
		bool _texture2DEnabled;
		bool _blendEnabled;
		bool _antialiasingEnabled;
		float _lineWidth;
		float _pointSize;
		Color _currentColor;
		Color _backColor;
		Size _size;
		PointF _dpi;
		PointF _globalScale;		
	public:
		GLCanvas(Color backColor);

		property bool Texture2DEnabled { bool get(); void set(bool value); }
		property bool BlendEnabled { bool get(); void set(bool value); }
		property bool AntialiasingEnabled { bool get(); void set(bool value); }
		property float LineWidth { float get(); void set(float value); }
		property float PointSize { float get(); void set(float value); }
		property Color CurrentColor { Color get(); void set(Color value); }
		property Color BackColor { Color get(); }
		property Size CanvasSize { Size get(); void set(Size value); }
		property PointF Dpi { PointF get(); void set(PointF value); }
		property PointF GlobalScale { PointF get(); void set(PointF value); }
		
		void Clear(Color color);

		void DrawLine(Point a, Point b);
		void DrawLine(PointF a, PointF b);

		void DrawLines(array<Point> ^points);
		void DrawLines(array<PointF> ^points);

		void DrawPoint(PointF a);
		void DrawPoints(array<PointF> ^points);
		
		void DrawRectangle(System::Drawing::Rectangle rect);
		void DrawRectangle(RectangleF rect);
		
		void FillRectangle(System::Drawing::Rectangle rect, array<Color> ^colors);
		void FillRectangle(RectangleF rect, array<Color> ^colors);
		
		void DrawEllipse(RectangleF rect);
		void DrawArc(RectangleF rect, float startAngle, float sweepAngle, bool closed);

		void Identity();
		void Transform(GLMatrix2D ^matrix);

		void SetClip(System::Drawing::Rectangle rect);
		void ResetClip();
	};
}
