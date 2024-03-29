﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace FourUIX.Components
{
    public partial class FourDrag : Component
    {
        private Form targetControl; private bool isDragging = false; private Point mouseOffset; private float smoothness = 4f;
        private Timer smoothMoveTimer;
        public FourDrag()
        {
            InitializeTimer();
        }

        public FourDrag(IContainer container)
        {
            container.Add(this);
            InitializeTimer();
        }

        private int gethz()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject mo in searcher.Get())
                {
                    return Convert.ToInt32(mo["CurrentRefreshRate"]);
                }
            }
            catch
            {
                return 60;
            }
            return 60;

        }

        private void InitializeTimer()
        {
            smoothMoveTimer = new Timer();
            smoothMoveTimer.Interval = 1000 / gethz(); ;
            smoothMoveTimer.Tick += SmoothMoveTimer_Tick;
        }

        public Form TargetControl
        {
            get { return targetControl; }
            set
            {
                if (targetControl != null)
                {
                    targetControl.MouseDown -= TargetControl_MouseDown;
                    targetControl.MouseMove -= TargetControl_MouseMove;
                    targetControl.MouseUp -= TargetControl_MouseUp;
                }

                targetControl = value;

                if (targetControl != null)
                {
                    targetControl.MouseDown += TargetControl_MouseDown;
                    targetControl.MouseMove += TargetControl_MouseMove;
                    targetControl.MouseUp += TargetControl_MouseUp;
                }
            }
        }

        public float Smoothness
        {
            get { return smoothness; }
            set
            {
                smoothness = value;
                if (smoothness == 0)
                {
                    smoothness = 1;
                }
            }
        }

        private void TargetControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            mouseOffset = new Point(e.X, e.Y);
        }

        private void TargetControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePos = targetControl.PointToScreen(e.Location);
                mousePos.Offset(-mouseOffset.X, -mouseOffset.Y);

                float targetX = mousePos.X;
                float targetY = mousePos.Y;

                targetControl.Location = new Point((int)targetX, (int)targetY);
            }
        }


        private void TargetControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            smoothMoveTimer.Stop();
        }

        private void SmoothMoveTimer_Tick(object sender, EventArgs e)
        {
            PointF increment = (PointF)smoothMoveTimer.Tag;

            float newX = targetControl.Location.X + increment.X;
            float newY = targetControl.Location.Y + increment.Y;

            targetControl.Location = new Point((int)newX, (int)newY);

            if (Math.Abs(increment.X) < 1 && Math.Abs(increment.Y) < 1)
            {
                smoothMoveTimer.Stop();
            }
        }
    }
}