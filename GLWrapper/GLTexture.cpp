#include "stdafx.h"
#include "GLTexture.h"

#include <vector>

std::vector<GLVertex> _glyphVertices;

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
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	/*glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);*/
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
}

void AddRectangles(RectangleF dstRect, RectangleF srcRect)
{
	GLVertex topLeft	 =	{ dstRect.X,					dstRect.Y,						srcRect.X,					srcRect.Y,					}; // 0
	GLVertex topRight	 =	{ dstRect.X + dstRect.Width,	dstRect.Y,						srcRect.X + srcRect.Width,	srcRect.Y,					}; // 1
	GLVertex bottomLeft	 =	{ dstRect.X,					dstRect.Y + dstRect.Height,		srcRect.X,					srcRect.Y + srcRect.Height,	}; // 2
	GLVertex bottomRight =  { dstRect.X + dstRect.Width,	dstRect.Y + dstRect.Height,		srcRect.X + srcRect.Width,	srcRect.Y + srcRect.Height,	}; // 3

	_glyphVertices.push_back(topLeft);
	_glyphVertices.push_back(topRight);
	_glyphVertices.push_back(bottomRight);
	_glyphVertices.push_back(bottomLeft);
}

void DrawGlyphVertices(unsigned int textureID)
{
	glBindTexture(GL_TEXTURE_2D, textureID);
	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(2, GL_FLOAT, sizeof(GLVertex), &_glyphVertices[0].x);	
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);
	glTexCoordPointer(2, GL_FLOAT, sizeof(GLVertex), &_glyphVertices[0].s);
	glDrawArrays(GL_QUADS, 0, _glyphVertices.size());
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);
	glDisableClientState(GL_VERTEX_ARRAY);
}

namespace GLWrapper
{
	GLTexture::GLTexture()
	{
		unsigned int textureID = 0;

		glGenTextures(1, &textureID);
		this->textureID = textureID;
	}

	GLTexture::GLTexture(Bitmap ^bitmap, int originalWidth, int originalHeight)
	{
		unsigned int textureID = 0;

		glGenTextures(1, &textureID);
		this->textureID = textureID;
		
		Update(bitmap, originalWidth, originalHeight);		
	}

	GLTexture::~GLTexture()
	{
		unsigned int textureID = this->textureID;
		if (textureID > 0)
			glDeleteTextures(1, &textureID);
		this->textureID = 0;
	}
	
	void GLTexture::Update(Bitmap ^bitmap, int originalWidth, int originalHeight)
	{
		System::Drawing::Rectangle rect = System::Drawing::Rectangle(0, 0, bitmap->Width, bitmap->Height);
		BitmapData ^data = bitmap->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);

		::UpdateTexture((GLubyte *)data->Scan0.ToPointer(), 4, textureID, rect.Width, rect.Height, false);

		bitmap->UnlockBits(data);

		width = rect.Width;
		height = rect.Height;

		this->originalWidth = originalWidth;
		this->originalHeight = originalHeight;
	}

	void GLTexture::Draw(RectangleF dstRect, RectangleF srcRect)
	{
		float oos = 1.0f / (float)width;
		float oot = 1.0f / (float)height;

		_glyphVertices.clear();

		srcRect = RectangleF(srcRect.X * oos, srcRect.Y * oot, srcRect.Width * oos, srcRect.Height * oot);

		::AddRectangles(dstRect, srcRect);
		::DrawGlyphVertices(textureID);
	}

	void GLTexture::DrawGlyphs(List<RectangleF> ^glyphDst, List<System::Drawing::Rectangle> ^glyphSrc)
	{
		float oos = 1.0f / (float)width;
		float oot = 1.0f / (float)height;

		_glyphVertices.clear();

		if (glyphDst->Count <= 0)
			return;

		for (int i = 0; i < glyphDst->Count; i++)
		{
			RectangleF dstRect = glyphDst[i];
			System::Drawing::Rectangle srcCoords = glyphSrc[i];
			RectangleF srcRect = RectangleF(srcCoords.X * oos, srcCoords.Y * oot, srcCoords.Width * oos, srcCoords.Height * oot);
			
			::AddRectangles(dstRect, srcRect);
		}

		::DrawGlyphVertices(textureID);
	}

	void GLTexture::DrawTiled(System::Drawing::Rectangle rect)
	{
		_glyphVertices.clear();

		float t = (float)originalWidth / (float)width;
		float s = (float)originalHeight / (float)height;

		RectangleF srcRect = RectangleF(0.0f, 0.0f, t, s);
		RectangleF dstRect = RectangleF(0.0f, 0.0f, (float)originalWidth, (float)originalHeight);

		for (int y = rect.Top; y < rect.Bottom; y += originalHeight)
        {
            for (int x = rect.Left; x < rect.Right; x += originalWidth)
            {
				dstRect.X = (float)x;
				dstRect.Y = (float)y;
						
				::AddRectangles(dstRect, srcRect);				
            }
		}

		if (_glyphVertices.size() > 0)
			::DrawGlyphVertices(textureID);			
	}
}
