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
    /// <summary>
    /// GameForm definiton class.
    /// </summary>
    public partial class GameForm : Form
    {
        /// <summary>
        /// GameForm constructor.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Program.Debug(LogLevel.Error, "Window size: {0}x{1}", Width, Height);
            Program.Debug(LogLevel.Error, "Board size: {0}x{1}", main_board.Width, main_board.Height);
        }

        /// <summary>
        /// Updates the board.
        /// </summary>
        public void UpdateBoard()
        {
            main_board.Refresh();
        }

        /// <summary>
        /// Handles drawing targets and GUI updates.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_board_Paint_1(object sender, PaintEventArgs e)
        {
            Program.game.TargetsMutex.WaitOne();
            foreach (Target t in Program.game.GetTargets())
            {
                Graphics g = main_board.CreateGraphics();
                SolidBrush sb = new SolidBrush(Color.Red);
                Pen p = new Pen(Color.Transparent);
                Program.Debug(LogLevel.Verbose, "Drawing at {0}, {1}, size:{2}", t.x, t.y, Convert.ToInt32(t.GetSize()));
                g.DrawEllipse(p, t.x, t.y, Convert.ToInt32(t.GetSize()), Convert.ToInt32(t.GetSize()));
                g.FillEllipse(sb, t.x, t.y, Convert.ToInt32(t.GetSize()), Convert.ToInt32(t.GetSize()));
            }
            Program.game.TargetsMutex.ReleaseMutex();
            hit_text.Text = String.Format("Trafienia: {0}", Program.game.GetHits());
            miss_text.Text = String.Format("Chybienia: {0}", Program.game.GetMisses());
            accuracy_text.Text = String.Format("Celność: {0}%", (Program.game.GetHits() != 0 || Program.game.GetMisses() != 0) ? 100*Program.game.GetHits()/(Program.game.GetHits()+Program.game.GetMisses()) : 0);
            ticktime_text.Text = String.Format("{0}ms", Program.game.ticktime.Milliseconds);
            fps_text.Text = String.Format("{0}fps", Program.game.Fps);
            time_text.Text = String.Format("Czas: {0}", Program.game.ElapsedTime.ToString(@"mm\:ss"));
        }

        /// <summary>
        /// MouseDown action handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_board_MouseDown(object sender, MouseEventArgs e)
        {
            Program.game.ClickHandler(e.X, e.Y);
        }

        /// <summary>
        /// Returns board size.
        /// </summary>
        /// <returns>Board size</returns>
        public Size GetBoardSize()
        {
            return main_board.Size;
        }

        /// <summary>
        /// Handles game windows resize action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Resize(object sender, EventArgs e)
        {
            main_board.Size = new Size(Width-250, Height-100);
            Program.Debug(LogLevel.Error, "Window resized: {0}x{1}", Width, Height);
            Program.Debug(LogLevel.Error, "Board resized: {0}x{1}", main_board.Width, main_board.Height);
        }

        /// <summary>
        /// Handles game window close action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        /// <summary>
        /// Handles pause button click action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pause_button_Click(object sender, EventArgs e)
        {
            if (Program.game.Running())
                Program.game.Pause();
            else
                Program.game.Start();
        }

        /// <summary>
        /// Handles menu button click action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_button_Click(object sender, EventArgs e)
        {
            ShowMenu();
        }

        /// <summary>
        /// Method implementing menu show-up.
        /// </summary>
        public void ShowMenu()
        {
            Menu menu = new Menu();
            Program.game.Pause();
            menu.ShowDialog();
        }
    }
}
