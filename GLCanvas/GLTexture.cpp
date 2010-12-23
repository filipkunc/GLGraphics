#include "stdafx.h"
#include "GLTexture.h"

namespace GLCanvas
{
	GLTexture::GLTexture(unsigned int textureID, float width, float height)
	{
		this->textureID = textureID;
		this->width = width;
		this->height = height;
	}

	void GLTexture::Draw(PointF position)
	{
		const float x = position.X;
		const float y = position.Y;

		const GLVertex vertices[] = 
		{
			{ x,			y,				0, 0, }, // 0
			{ x + width,	y,				1, 0, }, // 1
			{ x,			y + height,		0, 1, }, // 2
			{ x + width,	y + height,		1, 1, }, // 3
		};	

		glEnable(GL_TEXTURE_2D);
		glBindTexture(GL_TEXTURE_2D, textureID);
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, sizeof(GLVertex), &vertices->x);	
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		glTexCoordPointer(2, GL_SHORT, sizeof(GLVertex), &vertices->s);
		glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	}	
}
