# Welcome to GLGraphics

---

## License

GLGraphics is under MIT license. See LICENSE.TXT for details.

## About this project

GLGraphics is partial reimplementation of System.Drawing.Graphics in C#, C++/CLI and very simple OpenGL 1.x with main aim to speed up drawing and work as drop-in replacement as much as possible.

## How it is done?

System.Drawing.Graphics is sealed class so it cannot be inherited. There also doesn't exist any interface for drawing. First thing I have done is to add interface with every public method Graphics has, this lies in file GraphicsImplementation/IGraphics.cs. Then I created very simple wrapper for OpenGL in GLWrapper C++/CLI project which implements very basic GLCanvas class. Finally in C# was created GLGraphics class which holds reference to GLCanvas and implements IGraphics interface.

## How to use it in my project?

 1. Place GLView control on any of your UserControl or Form.
 2. Replace Graphics with IGraphics in your drawing code.
 3. Add event handler to Paint event and disable OpenGL in GLView (property GLEnabled).
 4. Add event handler to PaintCanvas event and enable OpenGL.

Event handlers should look like this:

    void Draw(IGraphics g)
    {
        g.FillRectangle(Brushes.Blue, new Rectangle(10, 10, 100, 50));
    }
    
    void glView_Paint(object sender, PaintEventArgs e)
    {
        GDIGraphics g = new GDIGraphics(e.Graphics);
        Draw(g);
    }
    
    void glView_PaintCanvas(object sender, CanvasEventArgs e)
    {
        GLGraphics g = new GLGraphics(e.Canvas);
        Draw(g);
    }

