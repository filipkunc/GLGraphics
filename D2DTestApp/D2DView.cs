using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace D2DTestApp
{
    public partial class D2DView : UserControl
    {
        public class RenderTargetEventArgs : EventArgs
        {
            public RenderTarget RenderTarget { get; private set; }
            public Graphics Graphics { get; private set; }

            public RenderTargetEventArgs(RenderTarget renderTarget, Graphics graphics)
            {
                RenderTarget = renderTarget;
                Graphics = graphics;
            }
        }

        SharpDX.Direct2D1.Factory _factoryD2D;
        SharpDX.DirectWrite.Factory _factoryDWrite;
        WindowRenderTarget _renderTarget;

        public D2DView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public RenderTargetType _renderTargetType = RenderTargetType.Default;

        void InitD2D()
        {
            _factoryD2D = new SharpDX.Direct2D1.Factory();
            _factoryDWrite = new SharpDX.DirectWrite.Factory();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties();
            properties.Hwnd = this.Handle;
            properties.PixelSize = new SharpDX.Size2(this.Width, this.Height);
            properties.PresentOptions = PresentOptions.Immediately;

            _renderTarget = new WindowRenderTarget(_factoryD2D,
                new RenderTargetProperties(_renderTargetType, 
                    new PixelFormat(Format.Unknown, AlphaMode.Premultiplied), 96.0f, 96.0f, 
                    RenderTargetUsage.GdiCompatible, FeatureLevel.Level_DEFAULT), properties);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitD2D();
        }

        bool _d2dEnabled = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool D2DEnabled
        {
            get { return _d2dEnabled; }
            set
            {
                _d2dEnabled = value;
                DoubleBuffered = !_d2dEnabled;
            }
        }

        public event EventHandler<RenderTargetEventArgs> PaintCanvas;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!_d2dEnabled)
                base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_d2dEnabled)
                base.OnPaint(e);
            else
            {
                _renderTarget.BeginDraw();
                _renderTarget.Clear(BackColor.ToColor4());

                if (PaintCanvas != null)
                    PaintCanvas(this, new RenderTargetEventArgs(_renderTarget, e.Graphics));

                _renderTarget.EndDraw();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_renderTarget != null)
                _renderTarget.Resize(new SharpDX.Size2(this.Width, this.Height));
        }
    }
}
