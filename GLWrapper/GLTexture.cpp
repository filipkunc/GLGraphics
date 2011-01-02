#include "stdafx.h"
#include "GLTexture.h"

void UpdateTexture(GLubyte *data, int components, GLuint textureID, int width, int height, bool convertToAlpha)
{
	glBindTexture(GL_TEXTURE_2D, textureID);

	if (convertToAlpha)
	{
		GLubyte *alphaData = (GLubyte *)malloc(width * height);
		for (int i = 0; i < width * height; i++)
			alphaData[i] = data[i * components];

		glTexImage2D(GL_TEXTURE_2D, 0, GL_ALPHA, width, height, 0, GL_ALPHA, GL_UNSIGNED_BYTE, alphaData);
		free(alphaData);
	}
	else 
	{
		if (components == 3)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
		else if (components == 4)
		{
			GLubyte *rgbaData = (GLubyte *)malloc(width * height * components);
			for (int i = 0; i < width * height; i++)
			{
				rgbaData[i * components + 0] = data[i * components + 2];
				rgbaData[i * components + 1] = data[i * components + 1];
				rgbaData[i * components + 2] = data[i * components + 0];
				rgbaData[i * components + 3] = data[i * components + 3];
			}
			
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, rgbaData);
			free(rgbaData);
		}
		else
			throw "Unsupported texture format";
	}

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
}

namespace GLWrapper
{
	GLTexture::GLTexture()
	{
		unsigned int textureID = 0;

		glEnable(GL_TEXTURE_2D);
		glGenTextures(1, &textureID);
		this->textureID = textureID;
	}

	GLTexture::GLTexture(Bitmap ^bitmap, int originalWidth, int originalHeight)
	{
		unsigned int textureID = 0;

		glEnable(GL_TEXTURE_2D);
		glGenTextures(1, &textureID);
		this->textureID = textureID;
		
		Update(bitmap, originalWidth, originalHeight);		
	}
	
	void GLTexture::Update(Bitmap ^bitmap, int originalWidth, int originalHeight)
	{
		System::Drawing::Rectangle rect = System::Drawing::Rectangle(Point::Empty, bitmap->Size);
        BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);

		::UpdateTexture((GLubyte *)data->Scan0.ToPointer(), 4, textureID, rect.Width, rect.Height, false);

		bitmap->UnlockBits(data);

		width = rect.Width;
		height = rect.Height;

		this->originalWidth = originalWidth;
		this->originalHeight = originalHeight;
	}

	void GLTexture::Draw(System::Drawing::Rectangle rect)
	{
		float textureX = (float)rect.Width / (float)width;
		float textureY = (float)rect.Height / (float)height;

		const GLIntVertex vertices[] = 
		{
			{ rect.X,				rect.Y,						0,			0, }, // 0
			{ rect.X + rect.Width,	rect.Y,						textureX,	0, }, // 1
			{ rect.X,				rect.Y + rect.Height,		0,			textureY, }, // 2
			{ rect.X + rect.Width,	rect.Y + rect.Height,		textureX,	textureY, }, // 3
		};

		glEnable(GL_TEXTURE_2D);
		glBindTexture(GL_TEXTURE_2D, textureID);
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_INT, sizeof(GLIntVertex), &vertices->x);	
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		glTexCoordPointer(2, GL_FLOAT, sizeof(GLIntVertex), &vertices->s);
		glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

		glDisableClientState(GL_TEXTURE_COORD_ARRAY);
		glDisableClientState(GL_VERTEX_ARRAY);	
	}

	void GLTexture::Draw(RectangleF rect)
	{
		float textureX = (float)rect.Width / (float)width;
		float textureY = (float)rect.Height / (float)height;

		const GLVertex vertices[] = 
		{
			{ rect.X,				rect.Y,						0,			0, }, // 0
			{ rect.X + rect.Width,	rect.Y,						textureX,	0, }, // 1
			{ rect.X,				rect.Y + rect.Height,		0,			textureY, }, // 2
			{ rect.X + rect.Width,	rect.Y + rect.Height,		textureX,	textureY, }, // 3
		};

		glEnable(GL_TEXTURE_2D);
		glBindTexture(GL_TEXTURE_2D, textureID);
		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, sizeof(GLVertex), &vertices->x);	
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		glTexCoordPointer(2, GL_FLOAT, sizeof(GLVertex), &vertices->s);
		glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

		glDisableClientState(GL_TEXTURE_COORD_ARRAY);
		glDisableClientState(GL_VERTEX_ARRAY);
	}

	void GLTexture::Draw(Point position)
	{
		Draw(System::Drawing::Rectangle(position, Size(originalWidth, originalHeight)));
	}

	void GLTexture::Draw(PointF position)
	{
		Draw(RectangleF(position, SizeF(originalWidth, originalHeight)));
	}
}
