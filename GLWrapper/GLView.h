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
		PointF viewOffset;
		GLCanvas ^glCanvas;
		bool glEnabled;
		bool neverInitGL;
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
		void DrawGL();		
	public:
		GLView(void);

		void PaintGL();

		event EventHandler<CanvasEventArgs ^> ^PaintCanvas;
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property GLView ^SharedContextView
		{ 
			GLView ^get();
			void set(GLView ^value); 
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility::Hidden)]
		property PointF ViewOffset
		{
			PointF get() { return viewOffset; }
			void set(PointF value) { viewOffset = value; }
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

