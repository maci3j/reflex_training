using System;
using System.Linq;

namespace reflex_training
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.newgame_button = new System.Windows.Forms.Button();
            this.exit_button = new System.Windows.Forms.Button();
            this.GameMode_box = new System.Windows.Forms.ComboBox();
            this.mode_label = new System.Windows.Forms.Label();
            this.GameType_desc = new System.Windows.Forms.Label();
            this.MovingTargets_checkbox = new System.Windows.Forms.CheckBox();
            this.ResizingTargets_checkbox = new System.Windows.Forms.CheckBox();
            this.RoundTime_UpDown = new System.Windows.Forms.NumericUpDown();
            this.TargetsNumber_UpDown = new System.Windows.Forms.NumericUpDown();
            this.RoundTime_text = new System.Windows.Forms.Label();
            this.TargetsNumber_text = new System.Windows.Forms.Label();
            this.TargetLifetime_UpDown = new System.Windows.Forms.NumericUpDown();
            this.TargetLifetime_text = new System.Windows.Forms.Label();
            this.TargetAddTime_text = new System.Windows.Forms.Label();
            this.TargetAddTime_UpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.RoundTime_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetsNumber_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetLifetime_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAddTime_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // newgame_button
            // 
            this.newgame_button.Location = new System.Drawing.Point(12, 245);
            this.newgame_button.Name = "newgame_button";
            this.newgame_button.Size = new System.Drawing.Size(215, 23);
            this.newgame_button.TabIndex = 0;
            this.newgame_button.Text = "Nowa gra";
            this.newgame_button.UseVisualStyleBackColor = true;
            this.newgame_button.Click += new System.EventHandler(this.newgame_button_Click);
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(242, 245);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(213, 23);
            this.exit_button.TabIndex = 2;
            this.exit_button.Text = "Wyjście";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // GameMode_box
            // 
            this.GameMode_box.DataSource = new reflex_training.GameType[] {
        reflex_training.GameType.Training,
        reflex_training.GameType.TimeTrial,
        reflex_training.GameType.ScoreTrial};
            this.GameMode_box.FormattingEnabled = true;
            this.GameMode_box.Location = new System.Drawing.Point(12, 33);
            this.GameMode_box.Name = "GameMode_box";
            this.GameMode_box.Size = new System.Drawing.Size(215, 21);
            this.GameMode_box.TabIndex = 3;
            this.GameMode_box.SelectedIndexChanged += new System.EventHandler(this.GameMode_box_SelectedIndexChanged);
            // 
            // mode_label
            // 
            this.mode_label.AutoSize = true;
            this.mode_label.Location = new System.Drawing.Point(12, 13);
            this.mode_label.Name = "mode_label";
            this.mode_label.Size = new System.Drawing.Size(52, 15);
            this.mode_label.TabIndex = 4;
            this.mode_label.Text = "Tryb gry:";
            // 
            // GameType_desc
            // 
            this.GameType_desc.AutoSize = true;
            this.GameType_desc.Location = new System.Drawing.Point(254, 33);
            this.GameType_desc.MaximumSize = new System.Drawing.Size(200, 0);
            this.GameType_desc.Name = "GameType_desc";
            this.GameType_desc.Size = new System.Drawing.Size(41, 15);
            this.GameType_desc.TabIndex = 5;
            this.GameType_desc.Text = "label1";
            // 
            // MovingTargets_checkbox
            // 
            this.MovingTargets_checkbox.AutoSize = true;
            this.MovingTargets_checkbox.Location = new System.Drawing.Point(15, 70);
            this.MovingTargets_checkbox.Name = "MovingTargets_checkbox";
            this.MovingTargets_checkbox.Size = new System.Drawing.Size(139, 19);
            this.MovingTargets_checkbox.TabIndex = 6;
            this.MovingTargets_checkbox.Text = "Poruszające się cele";
            this.MovingTargets_checkbox.UseVisualStyleBackColor = true;
            // 
            // ResizingTargets_checkbox
            // 
            this.ResizingTargets_checkbox.AutoSize = true;
            this.ResizingTargets_checkbox.Location = new System.Drawing.Point(15, 90);
            this.ResizingTargets_checkbox.Name = "ResizingTargets_checkbox";
            this.ResizingTargets_checkbox.Size = new System.Drawing.Size(166, 19);
            this.ResizingTargets_checkbox.TabIndex = 7;
            this.ResizingTargets_checkbox.Text = "Cele zmieniające rozmiar";
            this.ResizingTargets_checkbox.UseVisualStyleBackColor = true;
            // 
            // RoundTime_UpDown
            // 
            this.RoundTime_UpDown.Location = new System.Drawing.Point(214, 114);
            this.RoundTime_UpDown.Name = "RoundTime_UpDown";
            this.RoundTime_UpDown.Size = new System.Drawing.Size(65, 20);
            this.RoundTime_UpDown.TabIndex = 8;
            this.RoundTime_UpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // TargetsNumber_UpDown
            // 
            this.TargetsNumber_UpDown.Location = new System.Drawing.Point(214, 140);
            this.TargetsNumber_UpDown.Name = "TargetsNumber_UpDown";
            this.TargetsNumber_UpDown.Size = new System.Drawing.Size(65, 20);
            this.TargetsNumber_UpDown.TabIndex = 9;
            this.TargetsNumber_UpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // RoundTime_text
            // 
            this.RoundTime_text.AutoSize = true;
            this.RoundTime_text.Location = new System.Drawing.Point(15, 115);
            this.RoundTime_text.Name = "RoundTime_text";
            this.RoundTime_text.Size = new System.Drawing.Size(137, 15);
            this.RoundTime_text.TabIndex = 10;
            this.RoundTime_text.Text = "Czas trwania rundy(sec)";
            // 
            // TargetsNumber_text
            // 
            this.TargetsNumber_text.AutoSize = true;
            this.TargetsNumber_text.Location = new System.Drawing.Point(15, 141);
            this.TargetsNumber_text.Name = "TargetsNumber_text";
            this.TargetsNumber_text.Size = new System.Drawing.Size(167, 15);
            this.TargetsNumber_text.TabIndex = 11;
            this.TargetsNumber_text.Text = "Liczba celów na starcie rundy";
            // 
            // TargetLifetime_UpDown
            // 
            this.TargetLifetime_UpDown.Location = new System.Drawing.Point(214, 167);
            this.TargetLifetime_UpDown.Name = "TargetLifetime_UpDown";
            this.TargetLifetime_UpDown.Size = new System.Drawing.Size(65, 20);
            this.TargetLifetime_UpDown.TabIndex = 12;
            this.TargetLifetime_UpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // TargetLifetime_text
            // 
            this.TargetLifetime_text.AutoSize = true;
            this.TargetLifetime_text.Location = new System.Drawing.Point(15, 168);
            this.TargetLifetime_text.Name = "TargetLifetime_text";
            this.TargetLifetime_text.Size = new System.Drawing.Size(117, 15);
            this.TargetLifetime_text.TabIndex = 13;
            this.TargetLifetime_text.Text = "Czas życia celu(sec)";
            // 
            // TargetAddTime_text
            // 
            this.TargetAddTime_text.AutoSize = true;
            this.TargetAddTime_text.Location = new System.Drawing.Point(15, 194);
            this.TargetAddTime_text.Name = "TargetAddTime_text";
            this.TargetAddTime_text.Size = new System.Drawing.Size(200, 15);
            this.TargetAddTime_text.TabIndex = 14;
            this.TargetAddTime_text.Text = "Czas tworzenia kolejnego celu(sec)";
            // 
            // TargetAddTime_UpDown
            // 
            this.TargetAddTime_UpDown.Location = new System.Drawing.Point(214, 194);
            this.TargetAddTime_UpDown.Name = "TargetAddTime_UpDown";
            this.TargetAddTime_UpDown.Size = new System.Drawing.Size(65, 20);
            this.TargetAddTime_UpDown.TabIndex = 15;
            this.TargetAddTime_UpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 280);
            this.Controls.Add(this.TargetAddTime_UpDown);
            this.Controls.Add(this.TargetAddTime_text);
            this.Controls.Add(this.TargetLifetime_text);
            this.Controls.Add(this.TargetLifetime_UpDown);
            this.Controls.Add(this.TargetsNumber_text);
            this.Controls.Add(this.RoundTime_text);
            this.Controls.Add(this.TargetsNumber_UpDown);
            this.Controls.Add(this.RoundTime_UpDown);
            this.Controls.Add(this.ResizingTargets_checkbox);
            this.Controls.Add(this.MovingTargets_checkbox);
            this.Controls.Add(this.GameType_desc);
            this.Controls.Add(this.mode_label);
            this.Controls.Add(this.GameMode_box);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.newgame_button);
            this.Name = "Menu";
            this.Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)(this.RoundTime_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetsNumber_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetLifetime_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAddTime_UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newgame_button;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.ComboBox GameMode_box;
        private System.Windows.Forms.Label mode_label;
        private System.Windows.Forms.Label GameType_desc;
        private System.Windows.Forms.CheckBox MovingTargets_checkbox;
        private System.Windows.Forms.CheckBox ResizingTargets_checkbox;
        private System.Windows.Forms.NumericUpDown RoundTime_UpDown;
        private System.Windows.Forms.NumericUpDown TargetsNumber_UpDown;
        private System.Windows.Forms.Label RoundTime_text;
        private System.Windows.Forms.Label TargetsNumber_text;
        private System.Windows.Forms.NumericUpDown TargetLifetime_UpDown;
        private System.Windows.Forms.Label TargetLifetime_text;
        private System.Windows.Forms.Label TargetAddTime_text;
        private System.Windows.Forms.NumericUpDown TargetAddTime_UpDown;
    }
}