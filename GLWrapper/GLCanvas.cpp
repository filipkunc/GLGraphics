#include "stdafx.h"
#include "GLTexture.h"
#include "GLMatrix2D.h"
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

		_globalScale = PointF(1.0f, 1.0f);

		_currentColor = Color::Transparent;
		glColor4ub(0, 0, 0, 0);

		Clear(backColor);
	}

	void GLCanvas::Clear(Color color)
	{
		_backColor = color;
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
		if (_lineWidth <= 0.0f)
			glLineWidth(1.0f);
		else
			glLineWidth(_lineWidth * _globalScale.X);
	}

	float GLCanvas::PointSize::get()
	{
		return _pointSize;
	}

	void GLCanvas::PointSize::set(float value)
	{
		_pointSize = value;
		if (_pointSize <= 0.0f)
			glPointSize(1.0f);
		else
			glPointSize(_pointSize * _globalScale.X);
	}

	Color GLCanvas::BackColor::get()
	{
		return _backColor;
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

	PointF GLCanvas::Dpi::get()
	{
		return _dpi;
	}

	void GLCanvas::Dpi::set(PointF value)
	{
		_dpi = value;
	}

	PointF GLCanvas::GlobalScale::get()
	{
		return _globalScale;
	}

	void GLCanvas::GlobalScale::set(PointF value)
	{
		_globalScale = value;
		Identity();
	}

	void GLCanvas::DrawLine(Point a, Point b)
	{
		glBegin(GL_LINES);
		glVertex2i(a.X, a.Y);
		glVertex2i(b.X, b.Y);
		glEnd();
	}
	
	void GLCanvas::DrawLine(PointF a, PointF b)
	{
		glBegin(GL_LINES);
		glVertex2f(a.X, a.Y);
		glVertex2f(b.X, b.Y);
		glEnd();
	}

	void GLCanvas::DrawLines(array<Point> ^points)
	{
		pin_ptr<Point> vertexPtr = &points[0];
		
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_INT, sizeof(Point), vertexPtr);
		glDrawArrays(GL_LINE_STRIP, 0, points->Length);
		glDisableClientState(GL_VERTEX_ARRAY);
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

	void GLCanvas::DrawRectangle(System::Drawing::Rectangle rect)
	{
		glBegin(GL_LINE_LOOP);
		glVertex2i(rect.X, rect.Y);
		glVertex2i(rect.X + rect.Width, rect.Y);
		glVertex2i(rect.X + rect.Width, rect.Y + rect.Height);
		glVertex2i(rect.X, rect.Y + rect.Height);
		glEnd();
	}
	
	void GLCanvas::DrawRectangle(RectangleF rect)
	{
		glBegin(GL_LINE_LOOP);
		glVertex2f(rect.X, rect.Y);
		glVertex2f(rect.X + rect.Width, rect.Y);
		glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
		glVertex2f(rect.X, rect.Y + rect.Height);
		glEnd();		
	}

	void GLCanvas::FillRectangle(System::Drawing::Rectangle rect, array<Color> ^colors)
	{
		if (colors == nullptr)
		{
			glBegin(GL_QUADS);
			glVertex2i(rect.X, rect.Y);
			glVertex2i(rect.X + rect.Width, rect.Y);
			glVertex2i(rect.X + rect.Width, rect.Y + rect.Height);
			glVertex2i(rect.X, rect.Y + rect.Height);
			glEnd();
		}
		else
		{
			glBegin(GL_QUADS);
			glColor4ub(colors[0].R, colors[0].G, colors[0].B, colors[0].A);
			glVertex2i(rect.X, rect.Y);
			glColor4ub(colors[1].R, colors[1].G, colors[1].B, colors[1].A);
			glVertex2i(rect.X + rect.Width, rect.Y);
			glColor4ub(colors[2].R, colors[2].G, colors[2].B, colors[2].A);
			glVertex2i(rect.X + rect.Width, rect.Y + rect.Height);
			glColor4ub(colors[3].R, colors[3].G, colors[3].B, colors[3].A);
			glVertex2i(rect.X, rect.Y + rect.Height);
			glEnd();
		}
	}

	void GLCanvas::FillRectangle(RectangleF rect, array<Color> ^colors)
	{
		if (colors == nullptr)
		{
			glBegin(GL_QUADS);
			glVertex2f(rect.X, rect.Y);
			glVertex2f(rect.X + rect.Width, rect.Y);
			glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
			glVertex2f(rect.X, rect.Y + rect.Height);
			glEnd();
		}
		else
		{
			glBegin(GL_QUADS);
			glColor4ub(colors[0].R, colors[0].G, colors[0].B, colors[0].A);
			glVertex2f(rect.X, rect.Y);
			glColor4ub(colors[1].R, colors[1].G, colors[1].B, colors[1].A);
			glVertex2f(rect.X + rect.Width, rect.Y);
			glColor4ub(colors[2].R, colors[2].G, colors[2].B, colors[2].A);
			glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
			glColor4ub(colors[3].R, colors[3].G, colors[3].B, colors[3].A);
			glVertex2f(rect.X, rect.Y + rect.Height);
			glEnd();
		}
	}

	void GLCanvas::DrawEllipse(RectangleF rect)
	{
		const float DEG2RAD = (float)Math::PI / 180.0f;

		float xRadius = rect.Width / 2.0f;
		float yRadius = rect.Height / 2.0f;

		glBegin(GL_LINE_LOOP);
 
		for (float i = 0; i < 360.0f; i++)
		{
			float degInRad = i * DEG2RAD;
			glVertex2f(cosf(degInRad) * xRadius + rect.X + xRadius, sinf(degInRad) * yRadius + rect.Y + yRadius);
		}
	
		glEnd();
	}

	void GLCanvas::DrawArc(RectangleF rect, float startAngle, float sweepAngle, bool closed)
	{
		const float DEG2RAD = (float)Math::PI / 180.0f;

		float xRadius = rect.Width / 2.0f;
		float yRadius = rect.Height / 2.0f;

		if (closed)
			glBegin(GL_LINE_LOOP);
		else
			glBegin(GL_LINE_STRIP);
 
		for (float i = startAngle; i < startAngle + sweepAngle; i++)
		{
			//convert degrees into radians
			float degInRad = i * DEG2RAD;
			glVertex2f(cosf(degInRad) * xRadius + rect.X + xRadius, sinf(degInRad) * yRadius + rect.Y + yRadius);
		}
	
		glEnd();
	}

	void GLCanvas::Identity()
	{
		glPopMatrix();
		glPushMatrix();
		glScalef(_globalScale.X, _globalScale.Y, 1.0f);		
	}	

	void GLCanvas::Transform(GLMatrix2D ^matrix)
	{
		Identity();
		matrix->MultMatrix();
	}
}