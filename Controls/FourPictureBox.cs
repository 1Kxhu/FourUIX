using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourPictureBox : Control
    {
        private Image _image;
        private int _cornerRadius = 5;
        private float _rotationAngle = 0;
        private Matrix _translationMatrix = new Matrix();
        private Color borderColor = Color.Transparent;
        private int borderWidth = 1;

        public FourPictureBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
        }

        [Category("FourUI")]
        [Description("The image itself that will be displayed.")]
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                _cachedImage = null;
                Invalidate();
            }
        }

        [Category("FourUI")]
        [Description("The color of the border.")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("FourUI")]
        [Description("The rounding radius.")]
        public int BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
                Invalidate();
            }
        }

        [Category("FourUI")]
        [Description("The rounding radius.")]
        public int CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = value;
                Invalidate();
            }
        }

        [Category("FourUI")]
        [Description("Rotation of image from in degrees.")]
        public float RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
            set
            {
                _rotationAngle = value;
                _cachedImage = null;
                Invalidate();
            }
        }

        [Browsable(false)]
        public Matrix TranslationMatrix
        {
            get
            {
                return _translationMatrix;
            }
            set
            {
                _translationMatrix = value;
                Invalidate();
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _cachedImage = null;
        }

        private Image _cachedImage;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle crect = ClientRectangle;
            crect.Inflate(5, 5);
            crect.Offset(-2, -2);

            e.Graphics.FillRectangle(new SolidBrush(BackColor), crect);

            if (_image != null)
            {
                if (_cachedImage == null)
                {
                    _cachedImage = TransformImage(_image, Width, Height, _rotationAngle, _translationMatrix);
                }

                int diameter = _cornerRadius * 2;
                Rectangle rawrect = e.ClipRectangle;
                rawrect.Inflate(-1, -1);

                using (var path = RoundedRectangle(rawrect, CornerRadius))
                {
                    using (var pen = new Pen(borderColor, BorderWidth))
                    using (var brush = new TextureBrush(_cachedImage))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.InterpolationMode = InterpolationMode.Low;

                        brush.WrapMode = WrapMode.Clamp;
                        e.Graphics.FillPath(brush, path);
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }

        private Image TransformImage(Image inputImage, int width, int height, float rotation, Matrix matrixTranslate)
        {
            var newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.TranslateTransform(width / 2, height / 2);
                g.RotateTransform(rotation);
                g.ScaleTransform((float)width / inputImage.Width, (float)height / inputImage.Height);
                g.TranslateTransform(-inputImage.Width / 2, -inputImage.Height / 2);
                g.MultiplyTransform(matrixTranslate);
                g.DrawImage(inputImage, Point.Empty); // point.empty is the same as new point(0,0)
                g.Dispose();
            }

            return newImage;
        }
    }
}