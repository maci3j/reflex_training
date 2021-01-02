using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace reflex_training
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            Program.Debug(LogLevel.Error, "Window size: {0}x{1}", Width, Height);
            Program.Debug(LogLevel.Error, "Board size: {0}x{1}", main_board.Width, main_board.Height);
        }

        public void UpdateBoard()
        {
            main_board.Refresh();
        }

        private void main_board_Paint_1(object sender, PaintEventArgs e)
        {
            foreach (Target t in Program.game.GetTargets())
            {
                Graphics g = main_board.CreateGraphics();
                SolidBrush sb = new SolidBrush(Color.Red);
                Pen p = new Pen(Color.Transparent);
                g.DrawEllipse(p, t.x, t.y, t.GetSize(), t.GetSize());
                g.FillEllipse(sb, t.x, t.y, t.GetSize(), t.GetSize());
            }
            hit_text.Text = String.Format("Trafienia: {0}", Program.game.GetHits());
            miss_text.Text = String.Format("Chybienia: {0}", Program.game.GetMisses());
            accuracy_text.Text = String.Format("Celność: {0}%", (Program.game.GetHits() != 0 || Program.game.GetMisses() != 0) ? 100*Program.game.GetHits()/(Program.game.GetHits()+Program.game.GetMisses()) : 0);
        }

        private void main_board_MouseDown(object sender, MouseEventArgs e)
        {
            if (Program.game.State()) // click should be registered only when game is running
            {
                Program.Debug(LogLevel.Info, "Click: {0}, {1}", e.X, e.Y);
                foreach (Target t in Program.game.GetTargets())
                {
                    Program.Debug(LogLevel.Info, "Hit: {0}", t.IsHit(e.X, e.Y));
                    if (t.IsHit(e.X, e.Y))
                    {
                        Program.game.GetTargets().Remove(t);
                        Program.game.AddHit();
                        Program.game.AddTarget(true);
                        return;
                    }
                }
                Program.game.AddMiss();
            }
        }

        public Size GetBoardSize()
        {
            return main_board.Size;
        }

        private void GameForm_Resize(object sender, EventArgs e)
        {
            main_board.Size = new Size(Width-250, Height-100);
            Program.Debug(LogLevel.Error, "Window resized: {0}x{1}", Width, Height);
            Program.Debug(LogLevel.Error, "Board resized: {0}x{1}", main_board.Width, main_board.Height);
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void pause_button_Click(object sender, EventArgs e)
        {
            if (Program.game.State())
            {
                Program.game.Pause();
                Program.Debug(LogLevel.Error, "Game paused");
            }
            else
            {
                Program.game.Start();
                Program.Debug(LogLevel.Error, "Game started");
            }
        }
    }
}
