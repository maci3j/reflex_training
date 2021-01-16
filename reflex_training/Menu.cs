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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            SetGuiForTraining();
        }

        private void newgame_button_Click(object sender, EventArgs e)
        {
            Program.SelectedType = (GameType)GameMode_box.SelectedItem;
            TimeSpan TimeToEnd = new TimeSpan(0, 0, Convert.ToInt32(RoundTime_UpDown.Value));
            bool MovingTargets = MovingTargets_checkbox.Checked;
            bool ResizableTargets = ResizingTargets_checkbox.Checked;
            int TargetLifetime = Convert.ToInt32(TargetLifetime_UpDown.Value * 30);
            int StartingTargets = Convert.ToInt32(TargetsNumber_UpDown.Value);
            Program.StartGame(TimeToEnd, MovingTargets, ResizableTargets, TargetLifetime, StartingTargets, new TimeSpan(0,0,2));
            Hide();
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

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

        void SetGuiForTraining()
        {
            GameType_desc.Text = "Training";
        }

        void SetGuiForScoreTrial()
        {
            GameType_desc.Text = "ScoreTrial";
        }

        void SetGuiForTimeTrial()
        {
            GameType_desc.Text = "TimeTrial";
        }
    }
}
