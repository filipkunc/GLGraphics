#pragma once

typedef struct
{
	GLfloat x, y;
	GLshort s, t;
} GLVertex;

//void CreateTexture(GLubyte *data, int components, GLuint *textureID, int width, int height, bool convertToAlpha);

namespace GLCanvas
{
	public ref class GLTexture
	{
	private:
		unsigned int textureID;
		float width;
		float height;
	public:
		property float Width { float get() { return width; } }
		property float Height { float get() { return height; } }
		
		GLTexture(unsigned int textureID, float width, float height);

		void Draw(PointF position);		
	};
}
