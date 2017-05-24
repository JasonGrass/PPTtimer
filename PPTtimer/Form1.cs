using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using CCWin;
using CCWin.SkinControl;

namespace PPTtimer
{
    public partial class Form1 : CCSkinMain
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TimeSpan timerTimeSpan;
        private TimeSpan pauseTimeSpan;
        private DateTime startTime;
        private TimerStat currentTimerStatus ;
        private Timer timer = new Timer(1000);

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
             * 变量初始化
             */
            timerTimeSpan = new TimeSpan();
            pauseTimeSpan = new TimeSpan();
            currentTimerStatus = TimerStat.Stop;
            timer.Elapsed += TimerOnElapsed;

            /*
             * 窗体及控件初始化
             */
            lbTime.Text = "00:00";
            btnStart.Focus();
            this.ShowBorder = false;
            this.ShowDrawIcon = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
            this.TopMost = true;
            this.CanResize = false;       
            
            /*
             * 右键菜单
             */

            ToolStripMenuItem aboutItem = new ToolStripMenuItem()
            {
                Text = "关于",
            };
            aboutItem.Click += (o, args) =>
            {
                FormAbout about = new FormAbout();
                about.StartPosition = FormStartPosition.Manual;
                about.Location = new Point(this.Location.X + 5, this.Location.Y + this.Height + 5);
                about.ShowDialog();
            };
            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(aboutItem);

            lbTime.MouseClick += (o, args) =>
            {
                if (args.Button == MouseButtons.Right)
                {
                    ContextMenuStrip.Show(this, args.Location);
                }
            };

            /*
             * 窗体控制
             */

            lbTime.MouseDown += (o, args) => this.OnMouseDown(args);
            this.Activated += (o, args) =>
            {
                lbTime.ForeColor = Color.Crimson;
            };
            this.Deactivate += (o, args) =>
            {
                lbTime.ForeColor = Color.Black;
            };

        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timerTimeSpan = DateTime.Now - startTime + pauseTimeSpan;
            UpdateShowTime();
        }

        private void UpdateShowTime()
        {
            string showTimeString = (timerTimeSpan.Hours*60 + timerTimeSpan.Minutes).ToString("00") + ":" + timerTimeSpan.Seconds.ToString("00");
            this.Invoke(new Action(() =>
            {
                lbTime.Text = showTimeString;
            }));
        }

        // 开始 与 暂停 
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (currentTimerStatus == TimerStat.Pause || currentTimerStatus == TimerStat.Stop)
            {
                if (currentTimerStatus == TimerStat.Stop)
                {
                    lbTime.Text = "00:00";
                }
                startTime = DateTime.Now;
                timer.Start();
                currentTimerStatus = TimerStat.Running;

                btnStart.BackgroundImage = Properties.Resources.Pause_128px_1186203_easyicon_net;
            }
            else if (currentTimerStatus == TimerStat.Running)
            {
                pauseTimeSpan = new TimeSpan();
                pauseTimeSpan = pauseTimeSpan.Add(timerTimeSpan);
                timerTimeSpan = new TimeSpan();
                timer.Stop();
                currentTimerStatus = TimerStat.Pause;

                btnStart.BackgroundImage = Properties.Resources.Start_128px_1186321_easyicon_net;

            }

        }

        // 停止 与 重新开始
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (Equals(sender, btnClear))
            {
                lbTime.Text = "00:00";
            }
            pauseTimeSpan = new TimeSpan();
            timerTimeSpan = new TimeSpan();
            timer.Stop();
            currentTimerStatus = TimerStat.Stop;
            btnStart.BackgroundImage = Properties.Resources.Start_128px_1186321_easyicon_net;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormMessageBox messageBox = new FormMessageBox();
            messageBox.StartPosition = FormStartPosition.Manual;
            messageBox.Location = new Point(this.Location.X + 5, this.Location.Y + this.Height + 5);

            if (messageBox.ShowDialog() == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    internal enum TimerStat
    {
        Running,Pause,Stop
    }

}
