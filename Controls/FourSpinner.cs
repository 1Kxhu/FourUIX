using System;
using System.ComponentModel;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace FourUIX.Controls
{
    public class FourSpinner : Control
    {
        private float rotationAngle = 0;
        private int sweepAngle = 270;
        private int _thickness = 1;
        private int rotationSpeed = 1;
        private Timer rotationTimer = new Timer();

        public FourSpinner()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            ForeColor = Color.FromArgb(33, 133, 255);

            // tried to make the timer not use 13% of my cpu, so did this instead
            rotationTimer.Interval = 1;
            rotationTimer.Tick += Rotation;
            rotationTimer.Start();
        }

        private void Rotation(object sender, EventArgs ea)
        {
            // rotation
            rotationAngle += rotationSpeed;
            Invalidate();
        }

        [Category("FourUI")]
        public int SweepAngle
        {
            get => sweepAngle;

            set
            {
                if (value > 0 && value <= 360)
                    sweepAngle = value;
                else
                    throw new ArgumentException("SweepAngle must be between 1 and 360 degrees.");
            }
        }

        [Category("FourUI")]
        public int RotationSpeed
        {
            get => rotationSpeed;

            set
            {
                if (value > 0 && value <= 360)
                    rotationSpeed = value;
                else
                    throw new ArgumentException("SweepAngle must be between 1 and 360 degrees.");
            }
        }

        [Category("FourUI")]
        public int Thickness
        {
            get => _thickness;

            set
            {
                if (value > 0)
                    _thickness = value;
                else
                    throw new ArgumentException("Thickness must be greater than 0.");
            }
        }

        Pen BackPen;
        Pen ForePen;

        [Category("FourUI")]
        public Color spinnerBackground { get; set; } = Color.FromArgb(25, 25, 25);

        float visualrotationangle;

        protected override void OnPaint(PaintEventArgs e)
        {
            visualrotationangle = ((visualrotationangle * 4) + rotationAngle * 2) / 6;
            int centerX = Width / 2;
            int centerY = Height / 2;

            int spinnerSize = Math.Min(centerX, centerY);
            int spinnerThickness = (spinnerSize / 10) * _thickness;

            if (BackPen == null || ForePen == null)
            {
                BackPen = new Pen(spinnerBackground, spinnerThickness);
                ForePen = new Pen(ForeColor, spinnerThickness);
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawArc(BackPen, spinnerSize / 2, spinnerSize / 2, spinnerSize, spinnerSize, sweepAngle, visualrotationangle + 360 - sweepAngle);
            e.Graphics.DrawArc(ForePen, spinnerSize / 2, spinnerSize / 2, spinnerSize, spinnerSize, visualrotationangle, sweepAngle);
        }
    }
}