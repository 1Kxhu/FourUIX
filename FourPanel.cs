﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FourUI
{
    public partial class FourPanel : Control
    {
        private int cornerRadius = 5;
        private Color panelColor = Color.FromArgb(32, 32, 32);
        private int borderWidth = 2;


        [Browsable(true)]
        [Category("Appearance")]
        [Description("The radius of the rounded corners.")]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                cornerRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The color of the panel.")]
        public Color PanelColor
        {
            get { return panelColor; }
            set
            {

                panelColor = value;
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; return cp;
            }
        }

        public FourPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int arcSize = cornerRadius * 2;

            Rectangle outerRect = new Rectangle(0 - 2, 0 - 3, Width + 4, Height - 1);
            Rectangle innerRect = new Rectangle(borderWidth - 1, borderWidth - 1, Width - borderWidth * 2 + 2, Height - borderWidth * 2 + 2);

            using (GraphicsPath outerPath = new GraphicsPath())
            using (GraphicsPath innerPath = new GraphicsPath())
            using (Pen borderPen = new Pen(panelColor, borderWidth))
            {
                outerPath.AddArc(outerRect.X, outerRect.Y, arcSize, arcSize, 180, 90);
                outerPath.AddArc(outerRect.Width - arcSize, outerRect.Y, arcSize, arcSize, 270, 90);
                outerPath.AddArc(outerRect.Width - arcSize, outerRect.Height - arcSize, arcSize, arcSize, 0, 90);
                outerPath.AddArc(outerRect.X, outerRect.Height - arcSize, arcSize, arcSize, 90, 90);
                outerPath.CloseFigure();

                innerPath.AddArc(innerRect.X, innerRect.Y, arcSize, arcSize, 180, 90);
                innerPath.AddArc(innerRect.Width - arcSize, innerRect.Y, arcSize, arcSize, 270, 90);
                innerPath.AddArc(innerRect.Width - arcSize, innerRect.Height - arcSize, arcSize, arcSize, 0, 90);
                innerPath.AddArc(innerRect.X, innerRect.Height - arcSize, arcSize, arcSize, 90, 90);
                innerPath.CloseFigure();

                using (Brush brush = new SolidBrush(panelColor))
                {
                    g.FillPath(brush, innerPath);
                    g.FillPath(Brushes.Transparent, outerPath);
                }

                g.DrawPath(borderPen, innerPath);
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}
