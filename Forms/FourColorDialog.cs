using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FourUIX.Forms
{
    public partial class FourColorDialog : Form
    {
        private Timer drawTimer = new Timer();

        public FourColorDialog()
        {
            InitializeComponent();
            fourPictureBox1.MouseMove += changeSelectedColor;
        }


        public Color defaultColor { get; set; }
        public Color selectedColor { get; set; }
        public Color borderColor { get; set; } = Color.FromArgb(45, 45, 45);

        public Color buttonColor { get; set; } = Color.FromArgb(10, 10, 10);
        public Color buttonHoverColor { get; set; } = Color.FromArgb(14, 14, 14);
        public Color buttonPressColor { get; set; } = Color.FromArgb(21, 21, 21);


        public int buttonCornerRadius { get; set; } = 2;
        public int cornerRadius { get; set; } = 5;
        public int borderThickness { get; set; } = 1;

        private void FourColorDialog_Load(object sender, EventArgs e)
        {
            selectedColor = defaultColor;

            if (selectedColor == Color.Empty)
            {
                selectedColor = Color.Black;
            }

            fourPanel1.PanelColor = selectedColor;
            fourButton1.CornerRadius = buttonCornerRadius;
            fourButton2.CornerRadius = buttonCornerRadius;

            fourPictureBox1.CornerRadius = buttonCornerRadius;
            fourPanel1.CornerRadius = buttonCornerRadius;

            fourBorder1.BorderRadius = cornerRadius;
            fourBorder2.BorderRadius = cornerRadius - 1;

            fourButton1.FillColor = buttonColor;
            fourButton2.FillColor = buttonColor;

            fourButton1.HoverColor = buttonHoverColor;
            fourButton2.HoverColor = buttonHoverColor;

            fourButton1.PressColor = buttonPressColor;
            fourButton2.PressColor = buttonPressColor;

            fourPanel1.PanelColor = buttonColor;

            fourTextBox1.Text = selectedColor.R.ToString();
            fourTextBox2.Text = selectedColor.G.ToString();
            fourTextBox3.Text = selectedColor.B.ToString();

            updateTextBoxes();
        }

        private void changeSelectedColor(object sender, MouseEventArgs e)
        {
            Point cursorPosition = Cursor.Position;
            if (e.Button == MouseButtons.Left)
            {
                using (Bitmap screenCapture = new Bitmap(1, 1))
                {
                    using (Graphics g = Graphics.FromImage(screenCapture))
                    {
                        g.CopyFromScreen(cursorPosition, new Point(0, 0), new Size(1, 1));

                        Color pixelColor = screenCapture.GetPixel(0, 0);
                        pixelColor = Color.FromArgb(255, pixelColor.R, pixelColor.G, pixelColor.B);
                        screenCapture.Dispose();
                        g.Dispose();
                        selectedColor = pixelColor;
                        fourTextBox1.Text = pixelColor.R.ToString();
                        fourTextBox2.Text = pixelColor.G.ToString();
                        fourTextBox3.Text = pixelColor.B.ToString();
                        fourPanel1.Opacity = 0;
                        fourPanel1.PanelColor = selectedColor;
                        fourPanel1.Opacity = 1;

                        Invalidate();
                        Refresh();
                        Update();
                        DrawForm(sender, e);
                    }
                }
            }
        }

        private void updateTextBoxes()
        {
            fourTextBox1.Invalidate();
            fourTextBox2.Invalidate();
            fourTextBox3.Invalidate();
        }

        private void fourButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void fourButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void fourPanel1_Paint(object sender, PaintEventArgs e)
        {
           
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            base.OnPaint(e);

            if (this != null)
            {
                if (cornerRadius == 0)
                {
                    fourBorder2.TargetForm = null;
                }
                else
                {
                    fourBorder2.TargetForm = this;
                }
            }
        }

        int R;
        int G;
        int B;

        private void fourTextBox1_TextChanged(object sender, EventArgs e)
        {
            int parsed = 0;

            if (fourTextBox1.Text == "")
            {
                return;
            }

            try
            {
                parsed = int.Parse(fourTextBox1.Text);
            }
            catch
            {
                parsed = 0;
                fourTextBox1.Text = "0";
                R = 0;
            }

            if (parsed < 0 || parsed > 255)
            {
                return;
            }
            else
            {
                R = (int)parsed;
            }
            fourPanel1.PanelColor = Color.FromArgb(255, R, G, B);
            selectedColor = Color.FromArgb(255, R, G, B);
        }

        private void fourTextBox2_TextChanged(object sender, EventArgs e)
        {
            int parsed = 0;

            if (fourTextBox2.Text == "")
            {
                return;
            }

            try
            {
                parsed = int.Parse(fourTextBox2.Text);
            }
            catch
            {
                parsed = 0;
                fourTextBox2.Text = "0";
                G = 0;
            }

            if (parsed < 0 || parsed > 255)
            {
                return;
            }
            else
            {
                G = (int)parsed;
            }
            fourPanel1.PanelColor = Color.FromArgb(255, R, G, B);
            selectedColor = Color.FromArgb(255, R, G, B);
        }

        private void fourTextBox3_TextChanged(object sender, EventArgs e)
        {
            int parsed = 0;

            if (fourTextBox3.Text == "")
            {
                return;
            }

            try
            {
                parsed = int.Parse(fourTextBox3.Text);
            }
            catch
            {
                parsed = 0;
                fourTextBox3.Text = "0";
                B = 0;
            }

            if (parsed < 0 || parsed > 255)
            {
                return;
            }
            else
            {
                B = (int)parsed;
            }
            fourPanel1.PanelColor = Color.FromArgb(255, R, G, B);
            selectedColor = Color.FromArgb(255, R, G, B);
        }

        private void DrawForm(object pSender, EventArgs pE)
        {
            using (Bitmap backImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(backImage))
                {
                    Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    using (Brush b = new LinearGradientBrush(gradientRectangle, BackColor, BackColor, 0.0f))
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;

                        RoundedRectangle.FillRoundedRectangle(graphics, b, gradientRectangle, cornerRadius);

                        foreach (Control ctrl in this.Controls)
                        {
                            using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height))
                            {
                                Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                                ctrl.DrawToBitmap(bmp, rect);
                                graphics.DrawImage(bmp, ctrl.Location);
                            }
                        }

                        PerPixelAlphaBlend.SetBitmap(backImage, Left, Top, Handle);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                Graphics graphics = e.Graphics;

                Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                Brush b = new LinearGradientBrush(gradientRectangle, BackColor, BackColor, 0.0f);

                graphics.SmoothingMode = SmoothingMode.HighQuality;

                RoundedRectangle.FillRoundedRectangle(graphics, b, gradientRectangle, cornerRadius);
            }

            base.OnPaint(e);
        }
        public static class RoundedRectangle
        {
            public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
            {
                int diameter = radius*2;
                Size size = new Size(diameter, diameter);
                Rectangle arc = new Rectangle(bounds.Location, size);
                GraphicsPath path = new GraphicsPath();

                if (radius == 0)
                {
                    path.AddRectangle(bounds);
                    return path;
                }

                // top left arc  
                path.AddArc(arc, 180, 90);

                // top right arc  
                arc.X = bounds.Right - diameter;
                path.AddArc(arc, 270, 90);

                // bottom right arc  
                arc.Y = bounds.Bottom - diameter;
                path.AddArc(arc, 0, 90);

                // bottom left arc 
                arc.X = bounds.Left;
                path.AddArc(arc, 90, 90);

                path.CloseFigure();
                return path;
            }

            public static void FillRoundedRectangle(Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
            {
                if (graphics == null)
                    throw new ArgumentNullException("graphics");
                if (brush == null)
                    throw new ArgumentNullException("brush");

                using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
                {
                    graphics.FillPath(brush, path);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            drawTimer.Stop();
            drawTimer.Dispose();
            base.OnFormClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                drawTimer.Interval = 100;
                drawTimer.Tick += DrawForm;
                drawTimer.Start();
            }
            base.OnLoad(e);
        }


        internal static class PerPixelAlphaBlend
        {
            public static void SetBitmap(Bitmap bitmap, int left, int top, IntPtr handle)
            {
                SetBitmap(bitmap, 255, left, top, handle);
            }

            public static void SetBitmap(Bitmap bitmap, byte opacity, int left, int top, IntPtr handle)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");


                IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
                IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = Win32.SelectObject(memDc, hBitmap);

                    Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
                    Win32.Point pointSource = new Win32.Point(0, 0);
                    Win32.Point topPos = new Win32.Point(left, top);
                    Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                    blend.BlendOp = Win32.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = opacity;
                    blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                    Win32.UpdateLayeredWindow(handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
                }
                finally
                {
                    Win32.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        Win32.SelectObject(memDc, oldBitmap);
                        Win32.DeleteObject(hBitmap);
                    }

                    Win32.DeleteDC(memDc);
                }
            }
        }

        internal class Win32
        {
            public enum Bool
            {
                False = 0,
                True
            };


            [StructLayout(LayoutKind.Sequential)]
            public struct Point
            {
                public Int32 x;
                public Int32 y;

                public Point(Int32 x, Int32 y)
                {
                    this.x = x;
                    this.y = y;
                }
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct Size
            {
                public Int32 cx;
                public Int32 cy;

                public Size(Int32 cx, Int32 cy)
                {
                    this.cx = cx;
                    this.cy = cy;
                }
            }


            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            struct ARGB
            {
                public byte Blue;
                public byte Green;
                public byte Red;
                public byte Alpha;
            }


            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct BLENDFUNCTION
            {
                public byte BlendOp;
                public byte BlendFlags;
                public byte SourceConstantAlpha;
                public byte AlphaFormat;
            }


            public const Int32 ULW_COLORKEY = 0x00000001;
            public const Int32 ULW_ALPHA = 0x00000002;
            public const Int32 ULW_OPAQUE = 0x00000004;

            public const byte AC_SRC_OVER = 0x00;
            public const byte AC_SRC_ALPHA = 0x01;


            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll", ExactSpelling = true)]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern Bool DeleteDC(IntPtr hdc);

            [DllImport("gdi32.dll", ExactSpelling = true)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern Bool DeleteObject(IntPtr hObject);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= 0x00080000;
                }
                return cp;
            }
        }
    }
}
