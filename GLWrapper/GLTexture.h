#pragma once

typedef struct
{
	GLint x, y;
	GLfloat s, t;
} GLIntVertex;

typedef struct
{
	GLfloat x, y;
	GLfloat s, t;
} GLVertex;

namespace GLWrapper
{
	public ref class GLTexture
	{
	private:
		unsigned int textureID;
		int width;
		int height;
		int originalWidth;
		int originalHeight;
	public:
		property int Width { int get() { return width; } }
		property int Height { int get() { return height; } }
		
		GLTexture();
		GLTexture(Bitmap ^bitmap, int originalWidth, int originalHeight);
		~GLTexture();
		void Update(Bitmap ^bitmap, int originalWidth, int originalHeight);

		void Draw(System::Drawing::Rectangle rect);
		void Draw(RectangleF rect);

		void Draw(Point position);
		void Draw(PointF position);
	};
}
