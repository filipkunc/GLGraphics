#pragma once

typedef struct
{
	GLfloat x, y;
	GLshort s, t;
} GLVertex;

namespace GLWrapper
{
	public ref class GLTexture
	{
	private:
		unsigned int textureID;
		int width;
		int height;
	public:
		property int Width { int get() { return width; } }
		property int Height { int get() { return height; } }
		
		GLTexture();
		GLTexture(Bitmap ^bitmap);
		void Update(Bitmap ^bitmap);
		void Draw(PointF position);		
	};
}
