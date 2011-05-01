#include "stdafx.h"
#include "GLMatrix2D.h"

namespace GLWrapper
{
	GLMatrix2D::GLMatrix2D()
	{
		m = new float[16];

		this->Identity();
	}

	GLMatrix2D::GLMatrix2D(System::Drawing::Drawing2D::Matrix ^gdiMatrix)
	{
		m = new float[16];

		this->Identity();
		this->SetFromGdiMatrix(gdiMatrix);
	}

	GLMatrix2D::~GLMatrix2D()
	{
		this->!GLMatrix2D();
	}

	GLMatrix2D::!GLMatrix2D()
	{
		if (m)
		{
			delete m;
			m = nullptr;
		}
	}


	void GLMatrix2D::SetFromGdiMatrix(System::Drawing::Drawing2D::Matrix ^gdiMatrix)
	{
		array<float> ^gdiM = gdiMatrix->Elements;

		m[0] = gdiM[0];
		m[1] = gdiM[1];
		m[4] = gdiM[2];
		m[5] = gdiM[3];

		m[12] = gdiM[4];
		m[13] = gdiM[5];
	}

	System::Drawing::Drawing2D::Matrix ^GLMatrix2D::ToGdiMatrix()
	{
		return gcnew System::Drawing::Drawing2D::Matrix(m[0], m[1], m[4], m[5], m[12], m[13]);
	}

	void GLMatrix2D::Identity()
	{
		memset(m, 0, 16 * sizeof(float));
		m[0] = 1.0f;
		m[5] = 1.0f;
		m[10] = 1.0f;
		m[15] = 1.0f;
	}

	void GLMatrix2D::LoadMatrix()
	{
		glLoadMatrixf(m);
	}

	void GLMatrix2D::MultMatrix()
	{
		glMultMatrixf(m);
	}
}