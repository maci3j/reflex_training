using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reflex_training
{
    /// <summary>
    /// Menu GUI definition class.
    /// </summary>
    public partial class Menu : Form
    {
        /// <summary>
        /// Menu constructor.
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            SetGuiForTraining();
        }

        /// <summary>
        /// New game button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newgame_button_Click(object sender, EventArgs e)
        {
            Program.SelectedType = (GameType)GameMode_box.SelectedItem;
            TimeSpan TimeToEnd = new TimeSpan(0, 0, Convert.ToInt32(RoundTime_UpDown.Value));
            bool MovingTargets = MovingTargets_checkbox.Checked;
            bool ResizableTargets = ResizingTargets_checkbox.Checked;
            int TargetLifetime = Convert.ToInt32(TargetLifetime_UpDown.Value * 30);
            int StartingTargets = Convert.ToInt32(TargetsNumber_UpDown.Value);
            int TargetAddTime_seconds = Convert.ToInt32(TargetAddTime_UpDown.Value);
            Program.StartGame(TimeToEnd, MovingTargets, ResizableTargets, TargetLifetime, StartingTargets, new TimeSpan(0, 0, TargetAddTime_seconds));
            Hide();
        }

        /// <summary>
        /// Exit button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Game type dropdown change handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameMode_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameType type = (GameType)GameMode_box.SelectedItem;
            Program.Debug(LogLevel.Info, "GameType selected: {0}", type);

            if(type == GameType.ScoreTrial)
            {
                SetGuiForScoreTrial();
            }
            else if(type == GameType.TimeTrial)
            {
                SetGuiForTimeTrial();
            }
            else
            {
                SetGuiForTraining();
            }
        }

        /// <summary>
        /// Sets component states to suit Training configurable values.
        /// </summary>
        void SetGuiForTraining()
        {
            GameType_desc.Text = "Tryb treningu - bez żadnych ograniczeń";
            ResizingTargets_checkbox.Enabled = false;
            ResizingTargets_checkbox.Checked = false;
            TargetLifetime_UpDown.Enabled = false;
            TargetAddTime_UpDown.Enabled = false;
            RoundTime_UpDown.Enabled = false;
        }

        /// <summary>
        /// Sets component states to suit ScoreTrial configurable values.
        /// </summary>
        void SetGuiForScoreTrial()
        {
            GameType_desc.Text = "Zastrzel wszystkie pojawiające się cele, jeżeli nie zdążysz - przegrywasz";
            ResizingTargets_checkbox.Enabled = true;
            ResizingTargets_checkbox.Checked = true;
            TargetLifetime_UpDown.Enabled = true;
            TargetAddTime_UpDown.Enabled = true;
            RoundTime_UpDown.Enabled = false;
        }

        /// <summary>
        /// Sets component states to suit TimeTrial configurable values.
        /// </summary>
        void SetGuiForTimeTrial()
        {
            GameType_desc.Text = "Zdobądź jak największą liczbę punktów w wyznaczonym czasie";
            ResizingTargets_checkbox.Enabled = false;
            ResizingTargets_checkbox.Checked = false;
            TargetLifetime_UpDown.Enabled = false;
            TargetAddTime_UpDown.Enabled = false;
            RoundTime_UpDown.Enabled = true;
        }
    }
}
