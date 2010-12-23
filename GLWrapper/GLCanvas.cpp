#include "stdafx.h"
#include "GLTexture.h"
#include "GLCanvas.h"

namespace GLWrapper
{
	void GLCanvas::Clear(Color color)
	{
		glClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

		glClear(GL_COLOR_BUFFER_BIT);
	}

	void GLCanvas::EnableTexturing()
	{
		glEnable(GL_TEXTURE_2D);
	}

	void GLCanvas::DisableTexturing()
	{
		glDisable(GL_TEXTURE_2D);
	}

	void GLCanvas::EnableBlend()
	{
		glEnable(GL_BLEND);
	}

	void GLCanvas::DisableBlend()
	{
		glDisable(GL_BLEND);
	}

	void GLCanvas::EnableLineAntialiasing()
	{
		glEnable(GL_LINE_SMOOTH);
	}

	void GLCanvas::DisableLineAntialiasing()
	{
		glDisable(GL_LINE_SMOOTH);
	}

	void GLCanvas::SetCurrentColor(Color color)
	{
		glColor4ub(color.R, color.G, color.B, color.A);
	}

	void GLCanvas::SetLineWidth(float width)
	{
		glLineWidth(width);
	}

	void GLCanvas::SetPointSize(float size)
	{
		glPointSize(size);
	}

	void GLCanvas::DrawPoint(PointF a)
	{
		glBegin(GL_POINTS);
		glVertex2f(a.X, a.Y);
		glEnd();
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
		
		/*glBegin(GL_LINE_STRIP);
		for (int i = 0; i < points->Length; i++)
		{
			glVertex2f(points[i].X, points[i].Y);
		}
		glEnd();*/
		
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
}