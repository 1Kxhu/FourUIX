using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourProgressCircle : Control
    {
        private int progressValue = 0;
        private int borderWidth = 4;

        private int minval = 0;
        private int maxval = 100;

        private Color progressColor = Color.FromArgb(7, 126, 255);
        private Color staticColor = Color.FromArgb(18, 18, 18);

        [Category("FourUI")]
        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                if (progressValue < MinimumValue)
                {
                    progressValue = MinimumValue;
                }
                if (progressValue > MaximumValue)
                {
                    progressValue = MaximumValue;
                }

                Invalidate();
            }
        }

        [Category("FourUI")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                if (borderWidth < 1)
                {
                    borderWidth = 1;
                }

                Invalidate();
            }
        }

        [Category("FourUI")]
        public Color ProgressColor
        {
            get { return progressColor; }
            set
            {
                progressColor = value;
                Invalidate();
            }
        }

        [Category("FourUI")]
        public Color StaticColor
        {
            get { return staticColor; }
            set
            {
                staticColor = value;
                Invalidate();
            }
        }

        [Category("FourUI")]
        public int MinimumValue
        {
            get { return minval; }
            set
            {
                minval = value;
                if (minval > MaximumValue)
                {
                    minval = MaximumValue;
                }

                Invalidate();
            }
        }

        [Category("FourUI")]
        public int MaximumValue
        {
            get { return maxval; }
            set
            {
                maxval = value;
                if (maxval < MinimumValue)
                {
                    maxval = MinimumValue;
                }

                Invalidate();
            }
        }

        public FourProgressCircle()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(50, 50);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int halfx = BorderWidth / 2;
            int halfy = BorderWidth / 2;

            int circleWidth = Width - BorderWidth;
            int circleHeight = Height - BorderWidth;

            float percent = (float)(ProgressValue - MinimumValue) / (MaximumValue - MinimumValue) * 100;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(StaticColor, BorderWidth))
            {
                //10*3.6* = 36*
                path.AddArc(new Rectangle(halfx, halfy, circleWidth, circleHeight), (percent * 3.6f) - 90, 360 - (percent * 3.6f));
                e.Graphics.DrawPath(pen, path);
            }

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(ProgressColor, BorderWidth))
            {
                path.AddArc(new Rectangle(halfx, halfy, circleWidth, circleHeight), -90, percent * 3.6f);
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
