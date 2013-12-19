/*
 *  MathForwardDeclaration.h
 *  OpenGLEditor
 *
 *  Created by Filip Kunc on 6/20/09.
 *  For license see LICENSE.TXT
 *
 */
#pragma once

#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <float.h>

class Vector2D;
class Vector3D;
class Vector4D;
class Quaternion;
class Matrix4x4;

const double DOUBLE_PI = 3.14159265358979323846264338327950288419716939937510582;
const float FLOAT_PI = 3.14159265358979323846264338327950288419716939937510582f;
const float FLOAT_EPS = 10e-06f;

const float RAD_TO_DEG = 180.0f / FLOAT_PI;	// degrees = radians * RAD_TO_DEG;
const float DEG_TO_RAD = FLOAT_PI / 180.0f;	// radians = degrees * DEG_TO_RAD;

template <typename T>
T Min(T a, T b)
{
	return a < b ? a : b;
}

template <typename T>
T Max(T a, T b)
{
	return a > b ? a : b;
}

template <typename T>
T Abs(T x)
{
	return x < 0 ? -x : x;
}
