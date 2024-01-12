using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FourUIX.Controls
{
    public class FourSpinner : Control
    {
        private float rotationAngle = 0;
        private DateTime lastRenderTime = DateTime.Now;
        private int sweepAngle = 270;
        private int _thickness = 1;
        private int rotationSpeed = 1;

        public enum SpinnerTypes
        {
            DefaultSpinner,
            PacMan
        }

        [Category("FourUI")]
        public SpinnerTypes SpinnerType { get; set; } = SpinnerTypes.DefaultSpinner;


        public FourSpinner()
        {
            ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Application.Idle += Application_Idle;

            ForeColor = Color.FromArgb(33, 133, 255);

            if (Parent != null)
            {
                Parent.Resize += inval;
                Parent.Move += inval;
            }
        }

        private void inval(object sender, EventArgs e)
        {
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

        int pulsingSize;
        bool animatingphase = false;

        [Category("FourUI")]
        public Color spinnerBackground { get; set; } = Color.FromArgb(25, 25, 25);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int centerX = Width / 2;
            int centerY = Height / 2;


            int spinnerSize = Math.Min(centerX, centerY);
            int spinnerThickness = (spinnerSize / 10) * _thickness;

            float startAngle = rotationAngle;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var pen = new Pen(spinnerBackground, spinnerThickness))
            {
                e.Graphics.DrawArc(pen, spinnerSize / 2, spinnerSize / 2, spinnerSize, spinnerSize, sweepAngle, startAngle + 360 - sweepAngle);
            }

            using (var pen = new Pen(ForeColor, spinnerThickness))
            {
                e.Graphics.DrawArc(pen, spinnerSize / 2, spinnerSize / 2, spinnerSize, spinnerSize, startAngle, sweepAngle);
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            double elapsedMilliseconds = (currentTime - lastRenderTime).TotalMilliseconds;
            rotationAngle = (rotationAngle + (float)(RotationSpeed * 0.1 * elapsedMilliseconds)) % 360;
            lastRenderTime = currentTime;

            int center = Math.Min(Width, Height) / 2;
            pulsingSize += animatingphase ? 1 : -1;

            pulsingSize = Math.Min(center + 5, Math.Max(1, pulsingSize));
            animatingphase = (pulsingSize == 1) ? true : (pulsingSize == center + 5) ? false : animatingphase;

            Invalidate();
        }


    }
}