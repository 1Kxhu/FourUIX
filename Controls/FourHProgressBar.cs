using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourHProgressBar : Control
    {
        private int value = 0;
        private int minimum = 0;
        private int maximum = 100;

        private Color borderColor = Color.FromArgb(45, 45, 45);
        private int borderWidth = 2;

        Color progressColor = Color.FromArgb(33, 133, 255);
        Color _bgColor = Color.FromArgb(10, 10, 10);

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The progress color.")]
        public Color ProgressColor
        {
            get { return progressColor; }
            set { progressColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The color of the border.")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The color of the border.")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The background color.")]
        public Color BackgroundColor
        {
            get { return _bgColor; }
            set { _bgColor = value; Invalidate(); }
        }

        public event EventHandler ValueChanged;

        public FourHProgressBar()
        {
            this.Size = new System.Drawing.Size(200, 30);
            this.DoubleBuffered = true;
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value < minimum)
                    this.value = minimum;
                else if (value > maximum)
                    this.value = maximum;
                else
                {
                    this.value = value;
                    OnValueChanged(EventArgs.Empty);
                    this.Invalidate();
                }
            }
        }

        public int Minimum
        {
            get { return minimum; }
            set
            {
                minimum = value;
                if (value > maximum)
                    maximum = value;
                if (this.value < minimum)
                    this.value = minimum;
                this.Invalidate();
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                if (value < minimum)
                    minimum = value;
                if (this.value > maximum)
                    this.value = maximum;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int thirtyPercent = (int)((double)(Maximum / 100) * 30);
            int onePercent = (int)((double)(Maximum / 100) * 1);
            int twoPercent = (int)((double)(Maximum / 100) * 2);
            int threePercent = (int)((double)(Maximum / 100) * 2);


            if (Parent != null)
            {
                MaximumSize = new Size(Parent.Width, 62);
                MinimumSize = new Size(Height, 2);
            }

            Rectangle crect = ClientRectangle;
            crect.Inflate(5, 5);
            crect.Offset(-2, -2);

            if (Value > thirtyPercent * 3)
            {

            }

            if (BackColor != Color.Transparent)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), crect);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int trackHeight = Height - 2;
            float thumbPosition = (float)((value - minimum) / (double)(maximum - minimum)) * (Width - 20);


            int newV = (Value * 30);
            newV = Math.Min(newV, 34);
            newV = Math.Max(newV, 40);
            thumbPosition = thumbPosition + newV;
            if (Value == onePercent)
            {
                thumbPosition = thumbPosition + (Height / 8);
            }
            if (Value == twoPercent)
            {
                thumbPosition = thumbPosition + (Height / 9);
            }
            if (Value == threePercent)
            {
                thumbPosition = thumbPosition + (Height / 10);
            }

            //never let me cook up progress bars again 

            if (Value > 67)
            {
                thumbPosition -= onePercent;
            }

            if (Value > 77)
            {
                thumbPosition -= twoPercent;
            }

            if (Value > 87)
            {
                thumbPosition -= threePercent;
            }

            if (Value > 97)
            {
                thumbPosition -= (threePercent + 1);
            }

            Rectangle trackRectBehindThumb = new Rectangle(1, Height / 2 - trackHeight / 2, Width - 3, trackHeight);
            Rectangle trackRectAfterThumb = new Rectangle(1, Height / 2 - trackHeight / 2, (int)thumbPosition, trackHeight);

            if (Value > 0)
            {
                trackRectBehindThumb.Inflate(-1, 0);
                trackRectBehindThumb.Offset(1, 0);
            }

            using (SolidBrush bgBrush = new SolidBrush(_bgColor))
            {
                e.Graphics.FillPath(bgBrush, RoundedRectangle(trackRectBehindThumb, ((Height / 2) - 2)));
            }

            if (Value > 0)
            {
                using (SolidBrush progressBrush = new SolidBrush(progressColor))
                {
                    e.Graphics.FillPath(progressBrush, RoundedRectangle(trackRectAfterThumb, ((Height / 2) - 2)));
                }
            }

            using (Pen borderPen = new Pen(borderColor, borderWidth))
            {
                e.Graphics.DrawPath(borderPen, RoundedRectangle(trackRectBehindThumb, ((Height / 2) - 2)));
            }
        }


        private GraphicsPath RoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - 2 * radius, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - 2 * radius, rect.Bottom - 2 * radius, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - 2 * radius, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
