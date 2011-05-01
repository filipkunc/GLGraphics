#pragma once

namespace GLWrapper
{
	public ref class GLMatrix2D
	{
	private:
		float *m;
	public:
		GLMatrix2D();
		GLMatrix2D(System::Drawing::Drawing2D::Matrix ^gdiMatrix);
		~GLMatrix2D();
		!GLMatrix2D();

		void SetFromGdiMatrix(System::Drawing::Drawing2D::Matrix ^gdiMatrix);
		System::Drawing::Drawing2D::Matrix ^ToGdiMatrix();

		void Identity();
		void LoadMatrix();
		void MultMatrix();
	};
}
