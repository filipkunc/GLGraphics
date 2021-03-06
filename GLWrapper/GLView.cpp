#include "stdafx.h"
#include "DesignModeDevenv.h"
#include "GLCanvas.h"
#include "EventArgs.h"
#include "GLView.h"

bool WGLExtensionSupported(const char *extension_name)
{
    // this is pointer to function which returns pointer to string with list of all wgl extensions
    PFNWGLGETEXTENSIONSSTRINGEXTPROC wglGetExtensionsStringEXT = nullptr;

    // determine pointer to wglGetExtensionsStringEXT function
    wglGetExtensionsStringEXT = (PFNWGLGETEXTENSIONSSTRINGEXTPROC) wglGetProcAddress("wglGetExtensionsStringEXT");

    if (strstr(wglGetExtensionsStringEXT(), extension_name) == nullptr)
    {
        // string was not found
        return false;
    }

    // extension is supported
    return true;
}

namespace GLWrapper
{
	GLView::GLView(void)
	{
		deviceContext = nullptr;
		glRenderingContext = nullptr;
		sharedContextView = nullptr;
		glCanvas = nullptr;
		glEnabled = true;
		neverInitGL = false;
        testGLErrors = false;
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
        try
        {
            InitGL();
        }
        catch (Exception ^e)
        {
            glEnabled = false;
            InitGLErrorHandler(this, gcnew ExceptionEventArgs(e));
        }
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
			UserControl::OnPaint(e);
			return;
		}

		if (!glEnabled)
			UserControl::OnPaint(e);
		else
			PaintGL(e);
	}

	void GLView::OnPaintBackground(PaintEventArgs ^e)
	{
		if (DesignModeDevenv::DesignMode || !glEnabled)
			UserControl::OnPaintBackground(e);
	}

	#pragma endregion

    GLError GLView::GetGLError()
    {
        GLenum errorCode = glGetError();
        GLError glError = (GLError)errorCode;
        return glError;
    }

	void GLView::PaintGL(PaintEventArgs ^paintArgs)
	{
		if (!deviceContext || !glRenderingContext)
			return;

        try
        {
            BeginGL();
        }
        catch (Exception ^e)
        {
            glEnabled = false;
            InitGLErrorHandler(this, gcnew ExceptionEventArgs(e));
        }
		
		DrawGL(paintArgs);

        if (testGLErrors)
        {
            GLenum errorCode = glGetError();
            GLError glError = (GLError)errorCode;
            GLErrorHandler(this, gcnew GLErrorEventArgs(glError));
        }

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
            throw gcnew Win32Exception(Marshal::GetLastWin32Error(), "Unable to retrieve pixel format");
		}

		if (SetPixelFormat(deviceContext, pixelFormatIndex, &pfd) == 0)
		{
			throw gcnew Win32Exception(Marshal::GetLastWin32Error(), "Unable to set pixel format");
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
            throw gcnew Win32Exception(Marshal::GetLastWin32Error(), "Unable to get rendering context");
		}
		if (wglMakeCurrent(deviceContext, glRenderingContext) == 0)
		{
            throw gcnew Win32Exception(Marshal::GetLastWin32Error(), "Unable to make rendering context current");
		}
        
        if (WGLExtensionSupported("WGL_EXT_swap_control"))
        {
            PFNWGLSWAPINTERVALEXTPROC wglSwapIntervalEXT = (PFNWGLSWAPINTERVALEXTPROC)wglGetProcAddress("wglSwapIntervalEXT");

            wglSwapIntervalEXT(0); // Disable VSYNC
        }        
	}

	void GLView::BeginGL()
	{
		if (wglMakeCurrent(deviceContext, glRenderingContext) == 0)
		{
            throw gcnew Win32Exception(Marshal::GetLastWin32Error(), "Unable to make rendering context current");
		}
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

		//glTranslatef(0.5f, 0.5f, 0.0f);
	}

	void GLView::DrawGL(PaintEventArgs ^paintArgs)
	{
		ReshapeFlippedOrtho2D();

		if (glCanvas == nullptr)
			glCanvas = gcnew GLCanvas(this->BackColor);
		else
			glCanvas->Clear(this->BackColor);
		
		glCanvas->CanvasSize = this->ClientSize;
		glCanvas->Dpi = PointF(paintArgs->Graphics->DpiX, paintArgs->Graphics->DpiY);

		glPushMatrix();
		
		CanvasEventArgs ^args = gcnew CanvasEventArgs(glCanvas, paintArgs);
        PaintCanvas(this, args);

		glPopMatrix();
	}
}
