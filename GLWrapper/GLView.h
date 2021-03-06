#pragma once

namespace GLWrapper
{
	public ref class GLView : public System::Windows::Forms::UserControl
	{
	private:
		System::ComponentModel::Container^ components;

		HDC deviceContext;
		HGLRC glRenderingContext;
		GLView ^sharedContextView;
		GLCanvas ^glCanvas;
		bool glEnabled;
		bool neverInitGL;
        bool testGLErrors;
	protected:
		~GLView();

		virtual void OnLoad(EventArgs ^e) override;
		virtual void OnSizeChanged(EventArgs ^e) override;
		virtual void OnPaintBackground(PaintEventArgs ^e) override;
		virtual void OnPaint(PaintEventArgs ^e) override;

		void InitGL();
		void BeginGL();
		void EndGL();
		void ReshapeFlippedOrtho2D();
		void DrawGL(PaintEventArgs ^paintArgs);		
		void PaintGL(PaintEventArgs ^paintArgs);
	public:
		GLView(void);

		event EventHandler<CanvasEventArgs ^> ^PaintCanvas;
        event EventHandler<ExceptionEventArgs ^> ^InitGLErrorHandler;
        event EventHandler<GLErrorEventArgs ^> ^GLErrorHandler;
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property GLView ^SharedContextView
		{ 
			GLView ^get();
			void set(GLView ^value); 
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property bool GLEnabled
		{
			bool get() { return glEnabled; }
			void set(bool value) { glEnabled = value; DoubleBuffered = !value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property bool NeverInitGL
		{
			bool get() { return neverInitGL; }
			void set(bool value) { neverInitGL = value; }
		}

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property bool TestGLErrors
		{
            bool get() { return testGLErrors; }
			void set(bool value) { testGLErrors = value; }
		}

        GLError GetGLError();

	private:
	#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
		}
	#pragma endregion
	};
}

