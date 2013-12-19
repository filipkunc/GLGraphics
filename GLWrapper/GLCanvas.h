#pragma once

#include "Math\Camera.h"

namespace GLWrapper 
{
    // from http://msdn.microsoft.com/en-us/library/dd373546%28VS.85%29.aspx
    public enum class GLError
    {
        InvalidEnum = GL_INVALID_ENUM,
        InvalidValue = GL_INVALID_VALUE,
        InvalidOperation = GL_INVALID_OPERATION,
        NoError = GL_NO_ERROR,
        StackOverflow = GL_STACK_OVERFLOW,
        StackUnderflow = GL_STACK_UNDERFLOW,
        OutOfMemory = GL_OUT_OF_MEMORY,
    };

	ref class GLMatrix2D;

	public value struct GLVector3
	{
	public:
		float X, Y, Z;

		GLVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	};

	public value struct GLPoint
	{
	public:
		float X, Y, Z;
		Byte R, G, B, A;

		GLPoint(float x, float y, float z, System::Drawing::Color color)
		{
			X = x;
			Y = y;
			Z = z;
			R = color.R;
			G = color.G;
			B = color.B;
			A = color.A;
		}
	};

	public ref class GLCamera
	{
	private:
		Camera *_camera;
	public:
		GLCamera(GLVector3 center, float degreesX, float degreesY, float zoom)
		{
			_camera = new Camera();
			_camera->SetCenter(Vector3D(center.X, center.Y, center.Z));
			_camera->SetRadians(Vector2D(degreesX * DEG_TO_RAD, degreesY * DEG_TO_RAD));
			_camera->SetZoom(zoom);
		}

		~GLCamera()
		{
			if (_camera != nullptr)
			{
				delete _camera;
				_camera = nullptr;
			}
		}

		Camera *GetCamera()
		{
			return _camera;
		}

		void Move(float x, float y)
		{
			_camera->LeftRight(x);
			_camera->UpDown(y);
		}

		property float DegreesX
		{
			float get() { return _camera->GetRadians().x * RAD_TO_DEG; }
			void set(float value) { _camera->SetRadians(Vector2D(value * DEG_TO_RAD, _camera->GetRadians().y)); }
		}

		property float DegreesY
		{
			float get() { return _camera->GetRadians().y * RAD_TO_DEG; }
			void set(float value) { _camera->SetRadians(Vector2D(_camera->GetRadians().x, value * DEG_TO_RAD)); }
		}

		property float Zoom
		{
			float get() { return _camera->GetZoom(); }
			void set(float value) { _camera->SetZoom(value); }
		}
	};

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

		void DrawLines(array<Point> ^points, bool strip);
		void DrawLines(array<PointF> ^points, bool strip);

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

		void FillBitmap(Bitmap ^bitmap, Point offset);
		void DrawPixels(Bitmap ^bitmap, PointF offset);

		void SetLineStipplePattern(int factor, unsigned short pattern);
		void ResetLineStipplePattern();

		void SetPolygonStipplePattern(array<Byte> ^pattern);
		void ResetPolygonStipplePattern();

		void BeginPerspective(System::Drawing::Rectangle rect, double fovy, double zNear, double zFar);
        void BeginOrtho(System::Drawing::Rectangle rect, 
            double left, double right, double top, double bottom, double zNear, double zFar);
		void SetCamera(GLCamera ^camera);
		void DrawLines(array<GLPoint> ^vertices, bool strip);
		void DrawTriangles(array<GLPoint> ^vertices, bool strip);
		void DrawQuads(array<GLPoint> ^vertices, bool strip);
		void EndPerspectiveOrOrtho();		

        List<PointF> ^ProjectPoints(System::Collections::Generic::IEnumerable<GLVector3> ^points);
        List<GLVector3> ^UnProjectPoints(System::Collections::Generic::IEnumerable<PointF> ^points);
	};
}
