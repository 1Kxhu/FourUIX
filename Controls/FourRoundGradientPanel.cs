using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourRoundGradientPanel : Control
    {
        public FourRoundGradientPanel()
        {
            DoubleBuffered = true;
        }

        private float angle;

        [Category("FourUI")]
        public float Angle
        {
            get => angle;
            set
            {
                if (angle != value)
                {
                    angle = value;
                    OnAngleChanged(EventArgs.Empty);
                }
            }
        }

        private int radius;

        [Category("FourUI")]
        public int CornerRadius
        {
            get => radius;
            set
            {
                if (radius != value)
                {
                    radius = value;
                    Invalidate();
                }
            }
        }

        private Color gradientColor1 = Color.Blue;
        private Color gradientColor2 = Color.Red;

        [Category("FourUI")]
        public Color GradientColor1
        {
            get { return gradientColor1; }
            set
            {
                if (gradientColor1 != value)
                {
                    gradientColor1 = value;
                    Invalidate();
                }
            }
        }

        [Category("FourUI")]
        public Color GradientColor2
        {
            get { return gradientColor2; }
            set
            {
                if (gradientColor2 != value)
                {
                    gradientColor2 = value;
                    Invalidate();
                }
            }
        }

        public event EventHandler AngleChanged;
        protected virtual void OnAngleChanged(EventArgs e)
        {
            AngleChanged?.Invoke(this, e);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
                Graphics g = e.Graphics;

                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = ClientRectangle;
                rect.Inflate(-1, -1);

                GraphicsPath path = FourUIX.Helper.RoundedRectangle(rect, CornerRadius);

                LinearGradientBrush brush = new LinearGradientBrush(rect, GradientColor1, GradientColor2, Angle, true);

                g.FillPath(brush, path);
            }
            catch (Exception ex)
            {
                Debugger.Log(0, "4UI", ex.Message);
            }
        }
    }
}