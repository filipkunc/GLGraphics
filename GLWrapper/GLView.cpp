#include "stdafx.h"
#include "DesignModeDevenv.h"
#include "GLCanvas.h"
#include "CanvasEventArgs.h"
#include "GLView.h"

namespace GLWrapper
{
	GLView::GLView(void)
	{
		deviceContext = nullptr;
		glRenderingContext = nullptr;
		sharedContextView = nullptr;
		glCanvas = nullptr;
		viewOffset.X = 0.0f;
		viewOffset.Y = 0.0f;
		glEnabled = true;
		neverInitGL = false;
	}

	GLView::~GLView()
	{
		EndGL();
		if (glRenderingContext)
		{
			wglDeleteContext(glRenderingContext);
			glRenderingContext = nullptr;
		}
		if (components)
		{
			delete components;
		}
	}

	GLView ^GLView::SharedContextView::get()
	{
		return sharedContextView;
	}

	void GLView::SharedContextView::set(GLView ^value)
	{
		sharedContextView = value;
	}

	#pragma region OnEvents

	void GLView::OnLoad(EventArgs ^e)
	{
		UserControl::OnLoad(e);
		InitGL();
	}

	void GLView::OnSizeChanged(EventArgs ^e)
	{
		UserControl::OnSizeChanged(e);
		Invalidate();
	}

	void GLView::OnPaint(PaintEventArgs ^e)
	{
		if (DesignModeDevenv::DesignMode)
		{
			e->Graphics->Clear(BackColor);
			e->Graphics->DrawRectangle(Pens::Gray, 0, 0, this->Width - 1, this->Height - 1);
			return;
		}

		if (!glEnabled)
			UserControl::OnPaint(e);
		else
			PaintGL(e);
	}

	void GLView::OnPaintBackground(PaintEventArgs ^e)
	{
		if (!glEnabled)
			UserControl::OnPaintBackground(e);
	}

	#pragma endregion

	void GLView::PaintGL(PaintEventArgs ^paintArgs)
	{
		if (!deviceContext || !glRenderingContext)
			return;

		BeginGL();
		DrawGL(paintArgs);
		SwapBuffers(deviceContext);
		EndGL();
	}

	void GLView::InitGL()
	{
		if (DesignModeDevenv::DesignMode || neverInitGL)
			return;

		deviceContext = GetDC((HWND)this->Handle.ToPointer());
		// CAUTION: Not doing the following SwapBuffers() on the DC will
		// result in a failure to subsequently create the RC.
		SwapBuffers(deviceContext);

		//Get the pixel format		
		PIXELFORMATDESCRIPTOR pfd;
		ZeroMemory(&pfd, sizeof(pfd));
		pfd.nSize = sizeof(pfd);
		pfd.nVersion = 1;
		pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
		pfd.iPixelType = PFD_TYPE_RGBA;
		pfd.cColorBits = 32;
		pfd.cDepthBits = 0;
		pfd.iLayerType = PFD_MAIN_PLANE;

		int pixelFormatIndex = ChoosePixelFormat(deviceContext, &pfd);
		if (pixelFormatIndex == 0)
		{
			throw gcnew Exception("Unable to retrieve pixel format");			
		}

		if (SetPixelFormat(deviceContext, pixelFormatIndex, &pfd) == 0)
		{
			throw gcnew Exception("Unable to set pixel format");
		}

		wglMakeCurrent(0, 0);

		//Create rendering context
		glRenderingContext = wglCreateContext(deviceContext);
		if (sharedContextView != nullptr)
		{
			wglShareLists(sharedContextView->glRenderingContext, glRenderingContext);
		}
		if (!glRenderingContext)
		{
			throw gcnew Exception("Unable to get rendering context");			
		}
		if (wglMakeCurrent(deviceContext, glRenderingContext) == 0)
		{
			throw gcnew Exception("Unable to make rendering context current");			
		}
	}

	void GLView::BeginGL()
	{
		wglMakeCurrent(deviceContext, glRenderingContext);
	}

	void GLView::EndGL()
	{
		wglMakeCurrent(NULL, NULL);
	}

	void GLView::ReshapeFlippedOrtho2D()
	{
		System::Drawing::Rectangle rect = this->ClientRectangle;

		glViewport(0, 0, rect.Width, rect.Height);

		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();
		glOrtho(0, rect.Width, 0, rect.Height, -1.0f, 1.0f);

		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();

		glTranslatef((float)-rect.X, (float)rect.Y + (float)rect.Height, 0);
		glScalef(1, -1, 1);

		glTranslatef(viewOffset.X, viewOffset.Y, 0.0f);
	}

	void GLView::DrawGL(PaintEventArgs ^paintArgs)
	{
		ReshapeFlippedOrtho2D();

		if (glCanvas == nullptr)
			glCanvas = gcnew GLCanvas(this->BackColor);
		
		glCanvas->CanvasSize = this->ClientSize;
		glCanvas->Dpi = PointF(paintArgs->Graphics->DpiX, paintArgs->Graphics->DpiY);

		glPushMatrix();
		
		CanvasEventArgs ^args = gcnew CanvasEventArgs(glCanvas);
		PaintCanvas(this, args);

		glPopMatrix();
	}
}
