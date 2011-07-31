#pragma once

using namespace System::Collections::Generic;

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
		property int OriginalWidth { int get() { return originalWidth; } }
		property int OriginalHeight { int get() { return originalHeight; } }
		
		GLTexture();
		GLTexture(Bitmap ^bitmap, int originalWidth, int originalHeight);
		~GLTexture();
		void Update(Bitmap ^bitmap, int originalWidth, int originalHeight);

		void Draw(RectangleF dstRect, RectangleF srcRect);
		void DrawGlyphs(List<RectangleF> ^glyphDst, List<System::Drawing::Rectangle> ^glyphSrc);
		void DrawTiled(System::Drawing::Rectangle rect);
	};
}
