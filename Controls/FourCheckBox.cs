using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourCheckBox : Control
    {
        private Color thumbColorUnchecked = Color.Crimson;
        private Color thumbColorChecked = Color.FromArgb(33, 133, 255);
        private Color borderColor = Color.FromArgb(21, 21, 21);

        private bool ischecked = false;

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The unchecked switch thumb color.")]
        public Color UncheckedColor
        {
            get { return thumbColorUnchecked; }
            set
            {
                thumbColorUnchecked = value;
                if (Checked)
                {
                    thumbColor = thumbColorChecked;
                }
                else
                {
                    thumbColor = thumbColorUnchecked;
                }
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The checked switch thumb color.")]
        public Color CheckedColor
        {
            get { return thumbColorChecked; }
            set
            {
                thumbColorChecked = value;
                if (Checked)
                {
                    thumbColor = thumbColorChecked;
                }
                else
                {
                    thumbColor = thumbColorUnchecked;
                }
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
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

        public FourCheckBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            ischecked = Checked;

            MouseClick += (sender, e) =>
            {
                ischecked = !ischecked;
                animate();
            };
        }

        private async void animate()
        {
            for (int i = 0; i < 10; i++)
            {
                Invalidate();
                await Task.Delay(1);
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
        [Description("A boolean stating if the checkbox is checked.")]
        public bool Checked
        {
            get { return ischecked; }
            set
            {
                ischecked = value;
                Invalidate();
            }
        }

        public Color thumbColor;

        private int cornerRadius = 5;

        [Browsable(true)]
        [Category("FourUI")]
        [Description("Rounding of the control.")]
        public int CornerRadius
        {
            get
            {
                return cornerRadius;
            }
            set
            {
                cornerRadius = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int rectX = 0;
            int rectY = 0;
            int rectWidth = Height-1;
            int rectHeight = rectWidth;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (Checked)
            {
                thumbColor = Color.FromArgb(((thumbColor.R * 2) + thumbColorChecked.R) / 3, ((thumbColor.G * 2) + thumbColorChecked.G) / 3, ((thumbColor.B * 2) + thumbColorChecked.B) / 3);
            }
            else
            {
                thumbColor = Color.FromArgb(((thumbColor.R * 2) + thumbColorUnchecked.R) / 3, ((thumbColor.G * 2) + thumbColorUnchecked.G) / 3, ((thumbColor.B * 2) + thumbColorUnchecked.B) / 3);
            }

            if (DesignMode)
            {
                if (Checked)
                {
                    thumbColor = thumbColorChecked;
                }
                else
                {
                    thumbColor = thumbColorUnchecked;
                }
            }

            using (GraphicsPath path = Helper.RoundedRectangleXY(rectX, rectY, rectWidth, rectHeight, cornerRadius))
            {
                using (Pen borderPen = new Pen(borderColor))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }

            using (Brush thumbBrush = new SolidBrush(thumbColor))
            {
                GraphicsPath path = Helper.RoundedRectangleXY(rectX+2, rectY+2, rectWidth-4, rectHeight-4, cornerRadius);
                e.Graphics.FillPath(thumbBrush, path);
            }

            int xText = Height + 2;
            int yText = (Height / 2) - (Font.Height / 2);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), xText, yText);
        }
    }
}