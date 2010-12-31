#include "stdafx.h"
#include "GLTexture.h"
#include "GLCanvas.h"

namespace GLWrapper
{
	GLCanvas::GLCanvas(Color backColor)
	{
		_texture2DEnabled = false;
		glDisable(GL_TEXTURE_2D);

		_blendEnabled = true;
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		
		_antialiasingEnabled = false;

		_lineWidth = 1.0f;
		_pointSize = 1.0f;

		_currentColor = Color::Transparent;
		glColor4ub(0, 0, 0, 0);

		Clear(backColor);
	}

	void GLCanvas::Clear(Color color)
	{
		glClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		glClear(GL_COLOR_BUFFER_BIT);
	}

	bool GLCanvas::Texture2DEnabled::get()
	{
		return _texture2DEnabled;
	}

	void GLCanvas::Texture2DEnabled::set(bool value)
	{
		if (_texture2DEnabled != value)
		{
			_texture2DEnabled = value;
			if (_texture2DEnabled)
				glEnable(GL_TEXTURE_2D);
			else
				glDisable(GL_TEXTURE_2D);
		}
	}

	bool GLCanvas::BlendEnabled::get()
	{
		return _blendEnabled;
	}

	void GLCanvas::BlendEnabled::set(bool value)
	{
		if (_blendEnabled != value)
		{
			_blendEnabled = value;
			if (_blendEnabled)
				glEnable(GL_BLEND);
			else
				glDisable(GL_BLEND);
		}
	}

	bool GLCanvas::AntialiasingEnabled::get()
	{
		return _antialiasingEnabled;
	}

	void GLCanvas::AntialiasingEnabled::set(bool value)
	{
		if (_antialiasingEnabled != value)
		{
			_antialiasingEnabled = value;
			if (_antialiasingEnabled)
			{
				glEnable(GL_LINE_SMOOTH);
				glHint(GL_LINE_SMOOTH, GL_NICEST);
			}
			else
			{
				glDisable(GL_LINE_SMOOTH);				
			}
		}
	}

	float GLCanvas::LineWidth::get()
	{
		return _lineWidth;
	}

	void GLCanvas::LineWidth::set(float value)
	{
		_lineWidth = value;
		glLineWidth(_lineWidth);
	}

	float GLCanvas::PointSize::get()
	{
		return _pointSize;
	}

	void GLCanvas::PointSize::set(float value)
	{
		_pointSize = value;
		glPointSize(_pointSize);
	}

	Color GLCanvas::CurrentColor::get()
	{
		return _currentColor;
	}

	void GLCanvas::CurrentColor::set(Color value)
	{
		_currentColor = value;
		glColor4ub(_currentColor.R, _currentColor.G, _currentColor.B, _currentColor.A);
	}

	Size GLCanvas::CanvasSize::get()
	{
		return _size;
	}

	void GLCanvas::CanvasSize::set(Size value)
	{
		_size = value;
	}
	
	void GLCanvas::DrawLine(PointF a, PointF b)
	{
		glBegin(GL_LINES);
		glVertex2f(a.X, a.Y);
		glVertex2f(b.X, b.Y);
		glEnd();
	}

	void GLCanvas::DrawLines(array<PointF> ^points)
	{
		pin_ptr<PointF> vertexPtr = &points[0];
		
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, sizeof(PointF), vertexPtr);
		glDrawArrays(GL_LINE_STRIP, 0, points->Length);
		glDisableClientState(GL_VERTEX_ARRAY);
	}

	void GLCanvas::DrawPoint(PointF a)
	{
		glBegin(GL_POINTS);
		glVertex2f(a.X, a.Y);
		glEnd();
	}

	void GLCanvas::DrawPoints(array<PointF> ^points)
	{
		pin_ptr<PointF> vertexPtr = &points[0];
		
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, sizeof(PointF), vertexPtr);
		glDrawArrays(GL_POINTS, 0, points->Length);
		glDisableClientState(GL_VERTEX_ARRAY);
	}

	void GLCanvas::FillRectangle(RectangleF rect)
	{
		glRectf(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
	}

	void GLCanvas::DrawRectangle(RectangleF rect)
	{
		glBegin(GL_LINE_LOOP);
		glVertex2f(rect.X, rect.Y);
		glVertex2f(rect.X + rect.Width, rect.Y);
		glVertex2f(rect.X + rect.Width, rect.Y + rect.Height + 1.0f);
		glVertex2f(rect.X, rect.Y + rect.Height);
		glEnd();		
	}	

	void GLCanvas::DrawPixels(Bitmap ^bitmap)
	{
		bitmap->RotateFlip(System::Drawing::RotateFlipType::RotateNoneFlipY);

		System::Drawing::Rectangle rect = System::Drawing::Rectangle(Point::Empty, bitmap->Size);
        BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);

		glDrawPixels(rect.Width, rect.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, data->Scan0.ToPointer());

		bitmap->UnlockBits(data);		
	}
}