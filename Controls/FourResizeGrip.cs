using FourUIX.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourResizeGrip : Control
    {
        private Point resizeStart;

        public FourResizeGrip()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.SizeNWSE;
            if (!canChange())
            {
                throw new System.Exception("Resize grips can only be used on FormBorderStyle none forms.");
            }
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        bool canChange()
        {
            if (Parent is Form fr)
            {
                if (fr.FormBorderStyle != FormBorderStyle.None)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        [Category("FourUI")]
        public Point OffsetFromForm { get; set; } = new Point(0, 0);

        protected override void OnPaint(PaintEventArgs e)
        {
            if (canChange())
            {
                base.OnMouseUp(null);
                Visible = true;
                Capture = false;
                Location = new Point(Parent.Width - Width + OffsetFromForm.X, Parent.Height - Height + OffsetFromForm.Y);
            }
            if (canChange())
            {
                Rectangle rect = new Rectangle(Width - 18, Height - 20, 20, 20);
                rect.Inflate(-6, -6);
                rect.Offset(1, 3);

                using (ImageAttributes attributes = new ImageAttributes())
                {
                    ColorMatrix colorMatrix = new ColorMatrix
                    {
                        Matrix00 = ForeColor.R / 255.0f,
                        Matrix11 = ForeColor.G / 255.0f,
                        Matrix22 = ForeColor.B / 255.0f,
                        Matrix33 = ForeColor.A / 255.0f
                    };

                    attributes.SetColorMatrix(colorMatrix);

                    e.Graphics.DrawImage(Resources.grip, rect, 0, 0, Resources.grip.Width, Resources.grip.Height, GraphicsUnit.Pixel, attributes);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                if (BackColor == Color.Transparent)
                {
                    if (BackgroundImage != null)
                    {
                        e.Graphics.DrawImage(BackgroundImage, ClientRectangle);
                    }
                    else
                    {
                        if (Parent != null)
                        {
                            using (var bmp = new Bitmap(Parent.Width * 2, Parent.Height * 2))
                            {
                                Parent.Controls.Cast<Control>().Where(c => Parent.Controls.GetChildIndex(c) > Parent.Controls.GetChildIndex(this))
                                    .ToList()
                                    .ForEach(c => c.DrawToBitmap(bmp, new Rectangle(c.Bounds.X - 1, c.Bounds.Y - 1, c.Width + 1, c.Height + 1)));

                                e.Graphics.DrawImage(bmp, -Left, -Top);
                            }
                        }
                        else
                        {
                            base.OnPaintBackground(e);
                        }
                    }
                }
                else
                {
                    base.OnPaintBackground(e);
                }
            }
            catch
            {
                base.OnPaintBackground(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (canChange())
            {
                Visible = false;
                resizeStart = e.Location;
                Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (canChange())
            {
                base.OnMouseMove(e);
                if (Capture)
                {
                    int changeX = e.X - resizeStart.X;
                    int changeY = e.Y - resizeStart.Y;

                    Parent.Width += changeX;
                    Parent.Height += changeY;
                    resizeStart = e.Location;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (canChange())
            {
                base.OnMouseUp(e);
                Visible = true;
                Capture = false;
                Location = new Point(Parent.Width - Width + OffsetFromForm.X, Parent.Height - Height + OffsetFromForm.Y);
            }
        }
    }
}
