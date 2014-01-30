#include "stdafx.h"
#include "GLTexture.h"
#include "GLMatrix2D.h"
#include "GLCanvas.h"
#include "Math\Camera.h"

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

        _polygonMode = GLPolygonMode::Fill;
        glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

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

    GLPolygonMode GLCanvas::PolygonMode::get()
    {
        return _polygonMode;
    }

    void GLCanvas::PolygonMode::set(GLPolygonMode mode)
    {
        _polygonMode = mode;
        glPolygonMode(GL_FRONT_AND_BACK, (GLenum)_polygonMode);
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

	void GLCanvas::DrawLines(array<Point> ^points, bool strip)
	{
		pin_ptr<Point> vertexPtr = &points[0];
		
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_INT, sizeof(Point), vertexPtr);
		glDrawArrays(strip ? GL_LINE_STRIP : GL_LINES, 0, points->Length);
		glDisableClientState(GL_VERTEX_ARRAY);
	}	

	void GLCanvas::DrawLines(array<PointF> ^points, bool strip)
	{
		pin_ptr<PointF> vertexPtr = &points[0];
		
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, sizeof(PointF), vertexPtr);
		glDrawArrays(strip ? GL_LINE_STRIP : GL_LINES, 0, points->Length);
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

	void GLCanvas::SetClip(System::Drawing::Rectangle rect)
	{
		glEnable(GL_SCISSOR_TEST);
		glScissor(rect.X, _size.Height - rect.Bottom, rect.Width, rect.Height);
	}

	void GLCanvas::ResetClip()
	{
		glDisable(GL_SCISSOR_TEST);		
	}

	void GLCanvas::FillBitmap(Bitmap ^bitmap, Point offset)
	{
		glPushMatrix();
		glLoadIdentity();

		System::Drawing::Rectangle rect = System::Drawing::Rectangle(0, 0, bitmap->Width, bitmap->Height);
		BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::WriteOnly, PixelFormat::Format32bppArgb);

		glRasterPos2f(0.0f, 0.0f);
		glReadPixels(offset.X, offset.Y, rect.Width, rect.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, data->Scan0.ToPointer());

		bitmap->UnlockBits(data);

		glPopMatrix();
	}

	void GLCanvas::DrawPixels(Bitmap ^bitmap, PointF offset)
	{
		glPushMatrix();
		glLoadIdentity();

		System::Drawing::Rectangle rect = System::Drawing::Rectangle(0, 0, bitmap->Width, bitmap->Height);
		BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);

		glRasterPos2f(offset.X, offset.Y);
		glDrawPixels(bitmap->Width, bitmap->Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, data->Scan0.ToPointer());

		bitmap->UnlockBits(data);		

		glPopMatrix();		
	}

	void GLCanvas::SetLineStipplePattern(int factor, unsigned short pattern)
	{
		glEnable(GL_LINE_STIPPLE);
		glLineStipple(factor, pattern);
	}

	void GLCanvas::ResetLineStipplePattern()
	{
		glDisable(GL_LINE_STIPPLE);
	}

	void GLCanvas::SetPolygonStipplePattern(array<Byte> ^pattern)
	{
		pin_ptr<Byte> mask = &pattern[0];

		glEnable(GL_POLYGON_STIPPLE);
		glPolygonStipple(mask);
	}

	void GLCanvas::ResetPolygonStipplePattern()
	{
		glDisable(GL_POLYGON_STIPPLE);
	}

	void GLCanvas::BeginPerspective(System::Drawing::Rectangle rect, double fovy, double zNear, double zFar)
	{
		glPopMatrix();

		glViewport(rect.X, _size.Height - rect.Bottom, rect.Width, rect.Height);		

		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();

		double aspect = (double)rect.Width / (double)rect.Height;
		gluPerspective(fovy, aspect, zNear, zFar);		

		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();

		glClear(GL_DEPTH_BUFFER_BIT);
		glEnable(GL_DEPTH_TEST);
        glDepthFunc(GL_LEQUAL);
	}

    void GLCanvas::BeginOrtho(System::Drawing::Rectangle rect, 
        double left, double right, double top, double bottom, double zNear, double zFar)
	{
		glPopMatrix();

		glViewport(rect.X, _size.Height - rect.Bottom, rect.Width, rect.Height);		

		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();

        glOrtho(left, right, top, bottom, zNear, zFar);

		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();

		glClear(GL_DEPTH_BUFFER_BIT);
		glEnable(GL_DEPTH_TEST);
        glDepthFunc(GL_LEQUAL);
	} 

	void GLCanvas::SetCamera(GLCamera ^camera)
	{
		glLoadMatrixf(camera->GetCamera()->GetViewMatrix());
	}

	void GLCanvas::DrawLines(array<GLPoint> ^vertices, bool strip)
	{
		pin_ptr<float> vertexPtr = &vertices[0].X;
		pin_ptr<Byte> colorPtr = &vertices[0].R;

		glEnableClientState(GL_VERTEX_ARRAY);
		glEnableClientState(GL_COLOR_ARRAY);
		glVertexPointer(3, GL_FLOAT, sizeof(GLPoint), vertexPtr);
		glColorPointer(4, GL_UNSIGNED_BYTE, sizeof(GLPoint), colorPtr);
		glDrawArrays(strip ? GL_LINE_STRIP : GL_LINES, 0, vertices->Length);
		glDisableClientState(GL_COLOR_ARRAY);
		glDisableClientState(GL_VERTEX_ARRAY);
	}

	void GLCanvas::DrawTriangles(array<GLPoint> ^vertices, bool strip)
	{
		pin_ptr<float> vertexPtr = &vertices[0].X;
		pin_ptr<Byte> colorPtr = &vertices[0].R;

		glEnableClientState(GL_VERTEX_ARRAY);
		glEnableClientState(GL_COLOR_ARRAY);
		glVertexPointer(3, GL_FLOAT, sizeof(GLPoint), vertexPtr);
		glColorPointer(4, GL_UNSIGNED_BYTE, sizeof(GLPoint), colorPtr);
		glDrawArrays(strip ? GL_TRIANGLE_STRIP : GL_TRIANGLES, 0, vertices->Length);
		glDisableClientState(GL_COLOR_ARRAY);
		glDisableClientState(GL_VERTEX_ARRAY);
	}
	
	void GLCanvas::DrawQuads(array<GLPoint> ^vertices, bool strip)
	{
		pin_ptr<float> vertexPtr = &vertices[0].X;
		pin_ptr<Byte> colorPtr = &vertices[0].R;

		glEnableClientState(GL_VERTEX_ARRAY);
		glEnableClientState(GL_COLOR_ARRAY);
		glVertexPointer(3, GL_FLOAT, sizeof(GLPoint), vertexPtr);
		glColorPointer(4, GL_UNSIGNED_BYTE, sizeof(GLPoint), colorPtr);
		glDrawArrays(strip ? GL_QUAD_STRIP : GL_QUADS, 0, vertices->Length);
		glDisableClientState(GL_COLOR_ARRAY);
		glDisableClientState(GL_VERTEX_ARRAY);
	}

	void GLCanvas::EndPerspectiveOrOrtho()
	{
		glViewport(0, 0, _size.Width, _size.Height);

		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();
		glOrtho(0, _size.Width, 0, _size.Height, -1.0f, 1.0f);

		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();

		glTranslatef(0, (float)_size.Height, 0);
		glScalef(1, -1, 1);

		glTranslatef(0.5f, 0.5f, 0.0f);

		glPushMatrix();		

		glDisable(GL_DEPTH_TEST);
	}

    List<PointF> ^GLCanvas::ProjectPoints(System::Collections::Generic::IEnumerable<GLVector3> ^points)
    {
        GLint viewport[4];
        GLdouble modelView[16]; 
        GLdouble projection[16]; 
        glGetDoublev(GL_MODELVIEW_MATRIX, modelView);
        glGetDoublev(GL_PROJECTION_MATRIX, projection);
        glGetIntegerv(GL_VIEWPORT, viewport);

        List<PointF> ^projectedPoints = gcnew List<PointF>();

        for each(GLVector3 point in points)
        {
            double x, y, z;
            gluProject(point.X, point.Y, point.Z, modelView, projection, viewport, &x, &y, &z);
            projectedPoints->Add(PointF((float)x, (float)(_size.Height - y)));
        }

        return projectedPoints;
    }

    List<GLVector3> ^GLCanvas::UnProjectPoints(System::Collections::Generic::IEnumerable<PointF> ^points)
    {
        GLint viewport[4];
        GLdouble modelView[16]; 
        GLdouble projection[16]; 
        glGetDoublev(GL_MODELVIEW_MATRIX, modelView);
        glGetDoublev(GL_PROJECTION_MATRIX, projection);
        glGetIntegerv(GL_VIEWPORT, viewport);

        List<GLVector3> ^unProjectedPoints = gcnew List<GLVector3>();

        for each(PointF point in points)
        {
            double x, y, z;
            gluUnProject((double)point.X, (double)(_size.Height - point.Y), 0, modelView, projection, viewport, &x, &y, &z);
            unProjectedPoints->Add(GLVector3((float)x, (float)y, (float)z));
        }

        return unProjectedPoints;
    }
}