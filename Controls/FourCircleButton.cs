﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourUI
{
    public class FourCircleButton : Button
    {
        private int cornerRadius = 5; private int borderWidth = 2; private Color backColor2 = Color.FromArgb(32, 32, 32); private Color originalOriginalColor;
        private Color originalColor;
        private Color pressCol;

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The hover color.")]
        public Color HoverColor
        {
            get { return backColor2; }
            set
            {
                backColor2 = value;
                Refresh();
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The press color.")]
        public Color PressColor
        {
            get { return pressCol; }
            set
            {
                pressCol = value;
                Refresh();
            }
        }

        private Size imageSize = new Size(20, 20);
        private bool stretchImage = false;

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The size of the image displayed on the button.")]
        public Size ImageSize
        {
            get { return imageSize; }
            set
            {
                imageSize = value;
                Refresh();
            }
        }

        [Browsable(true)]
        [Category("FourUI")]
        [Description("Specifies whether the image should be stretched to fit the specified size.")]
        public bool StretchImage
        {
            get { return stretchImage; }
            set
            {
                stretchImage = value;
                Refresh();
            }
        }


        [Browsable(true)]
        [Category("FourUI")]
        [Description("The default state color.")]
        public Color FillColor
        {
            get { return originalColor; }
            set
            {
                originalColor = value;
                Refresh(); originalOriginalColor = value;
            }
        }


        private ImageAlignmentOption imageAlignment = ImageAlignmentOption.Left;


        public enum ImageAlignmentOption
        {
            Left,
            Center,
            Right
        }




        [Browsable(true)]
        [Category("FourUI")]
        [Description("The alignment of the image on the button.")]
        public ImageAlignmentOption ImageAlignment
        {
            get { return ImageAlignment1; }
            set
            {
                ImageAlignment1 = value;
                Refresh();
            }
        }


        private designchoice dchoice = designchoice.Filled;

        public enum designchoice
        {
            Filled,
            Outline
        }


        [Browsable(true)]
        [Category("FourUI")]
        [Description("The style in which the control displays.")]
        public designchoice DesignChoice
        {
            get { return dchoice; }
            set
            {
                dchoice = value;
                Invalidate();
            }
        }


        private Color dontchangethis
        {
            //sorry for doing this, its a workaround i did early indev and now im too scared that ill break something. 😓
            get { return backColor2; }
            set
            {
                backColor2 = value;
                Refresh();
            }
        }

        [Browsable(false)]
        [Category("FourUI")]
        [Description("The rounding value.")]
        public int CornerRadius
        {
            get { return cornerRadius; }
        }

        private Image buttonImage;

        [Browsable(true)]
        [Category("FourUI")]
        [Description("The image displayed on the button.")]
        public Image ButtonImage
        {
            get { return buttonImage; }
            set
            {
                buttonImage = value;
                Refresh();
            }
        }


        public ImageAlignmentOption ImageAlignment1 { get => imageAlignment; set => imageAlignment = value; }

        public FourCircleButton()
        {
            FillColor = Color.FromArgb(32, 32, 32);
            dontchangethis = Color.FromArgb(41, 41, 41);
            pressCol = Color.FromArgb(23, 23, 23);
            originalOriginalColor = FillColor;

            if (HoverColor.IsEmpty || !HoverColor.IsEmpty)
                HoverColor = backColor2;

            ForeColor = Color.White;
            Font = new Font("Microsoft Yahei UI", 10, FontStyle.Regular);
            Text = "Text";
            Size = new Size(130, 40);

            MouseEnter += CustomButton_MouseEnter;
            MouseLeave += CustomButton_MouseLeave;
            MouseClick += Ye;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private CancellationTokenSource cancellationTokenSource; private async void Ye(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                originalColor = pressCol; int steps = 15; int delay = 1;
                int deltaR = (int)Math.Round((originalOriginalColor.R - originalColor.R) / (double)steps);
                int deltaG = (int)Math.Round((originalOriginalColor.G - originalColor.G) / (double)steps);
                int deltaB = (int)Math.Round((originalOriginalColor.B - originalColor.B) / (double)steps);

                originalColor = pressCol;
                Invalidate();
                Application.DoEvents();

                Point mousePosition = PointToClient(MousePosition);
                while (MouseButtons != MouseButtons.None && ClientRectangle.Contains(mousePosition))
                {
                    mousePosition = PointToClient(MousePosition);
                    int newR = originalColor.R + deltaR;
                    int newG = originalColor.G + deltaG;
                    int newB = originalColor.B + deltaB;

                    originalColor = Color.FromArgb(newR, newG, newB); Invalidate(); Application.DoEvents();
                    await Task.Delay(TimeSpan.FromMilliseconds(delay), cancellationTokenSource.Token);
                    if (MouseButtons != MouseButtons.None && ClientRectangle.Contains(mousePosition))
                    {
                        if (steps > 1)
                        {
                            deltaR = (int)Math.Round((backColor2.R - originalColor.R) / (double)(steps - 1));
                            deltaG = (int)Math.Round((backColor2.G - originalColor.G) / (double)(steps - 1));
                            deltaB = (int)Math.Round((backColor2.B - originalColor.B) / (double)(steps - 1));
                        }
                    }
                }

                deltaR = (int)Math.Round((originalOriginalColor.R - originalColor.R) / (double)steps);
                deltaG = (int)Math.Round((originalOriginalColor.G - originalColor.G) / (double)steps);
                deltaB = (int)Math.Round((originalOriginalColor.B - originalColor.B) / (double)steps);

                for (int i = 1; i <= steps; i++)
                {
                    if (ClientRectangle.Contains(mousePosition))
                    {
                        int newR = originalColor.R + deltaR;
                        int newG = originalColor.G + deltaG;
                        int newB = originalColor.B + deltaB;

                        originalColor = Color.FromArgb(newR, newG, newB); Invalidate();
                        try
                        {
                            if (cancellationTokenSource != null && cancellationTokenSource.Token != null && !cancellationTokenSource.IsCancellationRequested)
                                await Task.Delay(delay, cancellationTokenSource.Token);
                        }
                        catch
                        {
                        }

                        if (i != steps)
                        {
                            deltaR = (int)Math.Round((backColor2.R - originalColor.R) / (double)(steps - i));
                            deltaG = (int)Math.Round((backColor2.G - originalColor.G) / (double)(steps - i));
                            deltaB = (int)Math.Round((backColor2.B - originalColor.B) / (double)(steps - i));
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                Invalidate();
            }
            catch
            {
            }
        }

        private void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;

            int steps = 15; int delay = 1;
            int deltaR = (originalOriginalColor.R - originalColor.R) / steps;
            int deltaG = (originalOriginalColor.G - originalColor.G) / steps;
            int deltaB = (originalOriginalColor.B - originalColor.B) / steps;

            for (int i = 1; i <= steps; i++)
            {
                int newR = originalColor.R + deltaR;
                int newG = originalColor.G + deltaG;
                int newB = originalColor.B + deltaB;

                originalColor = Color.FromArgb(newR, newG, newB); Invalidate(); Application.DoEvents();
                Thread.Sleep(delay);

                if (i != steps)
                {
                    deltaR = (originalOriginalColor.R - originalColor.R) / (steps - i);
                    deltaG = (originalOriginalColor.G - originalColor.G) / (steps - i);
                    deltaB = (originalOriginalColor.B - originalColor.B) / (steps - i);
                }
            }

            originalColor = originalOriginalColor; Invalidate();
        }

        private void CustomButton_MouseEnter(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;

            Color targetColor = backColor2; int steps = 7; int delay = 1;
            int deltaR = (targetColor.R - originalColor.R) / steps;
            int deltaG = (targetColor.G - originalColor.G) / steps;
            int deltaB = (targetColor.B - originalColor.B) / steps;

            Point mousePosition = PointToClient(MousePosition);

            for (int i = 1; i <= steps && ClientRectangle.Contains(mousePosition); i++)
            {
                mousePosition = PointToClient(MousePosition);
                int newR = originalColor.R + deltaR;
                int newG = originalColor.G + deltaG;
                int newB = originalColor.B + deltaB;

                originalColor = Color.FromArgb(newR, newG, newB); Invalidate(); Application.DoEvents();
                Thread.Sleep(delay);
            }

            Invalidate();
        }



        protected override void OnPaint(PaintEventArgs pevent)
        {
            int imageX;
            if (imageAlignment == ImageAlignmentOption.Left)
            {
                imageX = Width / 14;
            }
            else if (imageAlignment == ImageAlignmentOption.Center)
            {
                imageX = (Width - imageSize.Width) / 2;
            }
            else
            {
                imageX = Width - imageSize.Width - 5;
            }

            Size displaySize = imageSize;
            if (StretchImage)
            {
                displaySize = new Size(Width - 10, Height - 10);
            }

            cornerRadius = (Height / 2) - 2;
            if (cornerRadius < 1)
            {
                cornerRadius = 1;
            }
            using (SolidBrush brush = new SolidBrush(Parent.BackColor))
            {
                pevent.Graphics.FillRectangle(brush, ClientRectangle);
            }

            Rectangle rect = new Rectangle(ClientRectangle.Left, ClientRectangle.Top,
                               ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            Rectangle rect2 = new Rectangle(ClientRectangle.Left + imageX - 4, ClientRectangle.Top,
                           ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            GraphicsPath path = new GraphicsPath();
            int arcSize = 2 * cornerRadius;

            path.AddArc(rect.Left, rect.Top, arcSize, arcSize, 180, 90);
            path.AddArc(rect.Right - arcSize, rect.Top, arcSize, arcSize, 270, 90);
            path.AddArc(rect.Right - arcSize, rect.Bottom - arcSize, arcSize, arcSize, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - arcSize, arcSize, arcSize, 90, 90);

            path.CloseFigure();

            using (Pen borderPen = new Pen(this.BackColor, borderWidth + 74))
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pevent.Graphics.DrawPath(borderPen, path);
            }

            if (DesignChoice == designchoice.Filled)
            {
                using (SolidBrush brush = new SolidBrush(originalColor))
                {
                    pevent.Graphics.FillPath(brush, path);
                }
            }
            else
            {
                using (Pen pen = new Pen(originalColor, 1.25f))
                {
                    pevent.Graphics.DrawPath(pen, path);
                }
            }


            TextFormatFlags flag = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            if (ButtonImage != null)
            {
                pevent.Graphics.DrawImage(ButtonImage, new Rectangle(new Point(imageX, (Height - displaySize.Height) / 2), displaySize));
            }
            TextRenderer.DrawText(pevent.Graphics, Text, Font, rect2, ForeColor,
    flag);

        }
    }
}