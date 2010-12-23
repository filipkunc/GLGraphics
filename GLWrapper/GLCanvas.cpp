#include "stdafx.h"
#include "GLTexture.h"
#include "GLCanvas.h"

namespace GLWrapper
{
	void GLCanvas::Clear(Color color)
	{
		glClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

		glEnable(GL_TEXTURE_2D);
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

	GLTexture ^GLCanvas::CreateTexture(Bitmap ^bitmap)
	{
		System::Drawing::Rectangle rect = System::Drawing::Rectangle(Point::Empty, bitmap->Size);
        BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);

		unsigned int textureID;
		::CreateTexture((GLubyte *)data->Scan0.ToPointer(), 4, &textureID, rect.Width, rect.Height, false);
		
		bitmap->UnlockBits(data);

		return gcnew GLTexture(textureID, (float)rect.Width, (float)rect.Height);
	}	
}