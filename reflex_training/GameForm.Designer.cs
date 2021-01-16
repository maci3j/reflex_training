namespace reflex_training
{
    partial class GameForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.main_board = new System.Windows.Forms.Panel();
            this.hit_text = new System.Windows.Forms.Label();
            this.miss_text = new System.Windows.Forms.Label();
            this.accuracy_text = new System.Windows.Forms.Label();
            this.pause_button = new System.Windows.Forms.Button();
            this.ticktime_text = new System.Windows.Forms.Label();
            this.menu_button = new System.Windows.Forms.Button();
            this.fps_text = new System.Windows.Forms.Label();
            this.time_text = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // main_board
            // 
            this.main_board.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.main_board.Location = new System.Drawing.Point(12, 12);
            this.main_board.Name = "main_board";
            this.main_board.Size = new System.Drawing.Size(550, 426);
            this.main_board.TabIndex = 0;
            this.main_board.Paint += new System.Windows.Forms.PaintEventHandler(this.main_board_Paint_1);
            this.main_board.MouseDown += new System.Windows.Forms.MouseEventHandler(this.main_board_MouseDown);
            // 
            // hit_text
            // 
            this.hit_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hit_text.AutoSize = true;
            this.hit_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hit_text.Location = new System.Drawing.Point(585, 42);
            this.hit_text.Name = "hit_text";
            this.hit_text.Size = new System.Drawing.Size(133, 29);
            this.hit_text.TabIndex = 1;
            this.hit_text.Text = "Trafienia: 0";
            this.hit_text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // miss_text
            // 
            this.miss_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.miss_text.AutoSize = true;
            this.miss_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.miss_text.Location = new System.Drawing.Point(585, 71);
            this.miss_text.Name = "miss_text";
            this.miss_text.Size = new System.Drawing.Size(145, 29);
            this.miss_text.TabIndex = 2;
            this.miss_text.Text = "Chybienia: 0";
            this.miss_text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // accuracy_text
            // 
            this.accuracy_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accuracy_text.AutoSize = true;
            this.accuracy_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.accuracy_text.Location = new System.Drawing.Point(585, 100);
            this.accuracy_text.Name = "accuracy_text";
            this.accuracy_text.Size = new System.Drawing.Size(126, 29);
            this.accuracy_text.TabIndex = 3;
            this.accuracy_text.Text = "Celność: 0";
            this.accuracy_text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pause_button
            // 
            this.pause_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pause_button.Location = new System.Drawing.Point(590, 382);
            this.pause_button.Name = "pause_button";
            this.pause_button.Size = new System.Drawing.Size(75, 23);
            this.pause_button.TabIndex = 4;
            this.pause_button.Text = "Pauza";
            this.pause_button.UseVisualStyleBackColor = true;
            this.pause_button.Click += new System.EventHandler(this.pause_button_Click);
            // 
            // ticktime_text
            // 
            this.ticktime_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ticktime_text.AutoSize = true;
            this.ticktime_text.Location = new System.Drawing.Point(728, 423);
            this.ticktime_text.Name = "ticktime_text";
            this.ticktime_text.Size = new System.Drawing.Size(75, 15);
            this.ticktime_text.TabIndex = 5;
            this.ticktime_text.Text = "ticktime_text";
            // 
            // menu_button
            // 
            this.menu_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.menu_button.Location = new System.Drawing.Point(590, 412);
            this.menu_button.Name = "menu_button";
            this.menu_button.Size = new System.Drawing.Size(75, 23);
            this.menu_button.TabIndex = 6;
            this.menu_button.Text = "Menu";
            this.menu_button.UseVisualStyleBackColor = true;
            this.menu_button.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // fps_text
            // 
            this.fps_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fps_text.AutoSize = true;
            this.fps_text.Location = new System.Drawing.Point(681, 423);
            this.fps_text.Name = "fps_text";
            this.fps_text.Size = new System.Drawing.Size(23, 15);
            this.fps_text.TabIndex = 7;
            this.fps_text.Text = "fps";
            // 
            // time_text
            // 
            this.time_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.time_text.AutoSize = true;
            this.time_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.time_text.Location = new System.Drawing.Point(585, 13);
            this.time_text.Name = "time_text";
            this.time_text.Size = new System.Drawing.Size(91, 29);
            this.time_text.TabIndex = 8;
            this.time_text.Text = "Czas: 0";
            this.time_text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.time_text);
            this.Controls.Add(this.fps_text);
            this.Controls.Add(this.menu_button);
            this.Controls.Add(this.ticktime_text);
            this.Controls.Add(this.pause_button);
            this.Controls.Add(this.accuracy_text);
            this.Controls.Add(this.miss_text);
            this.Controls.Add(this.hit_text);
            this.Controls.Add(this.main_board);
            this.Name = "GameForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Resize += new System.EventHandler(this.GameForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel main_board;
        private System.Windows.Forms.Label hit_text;
        private System.Windows.Forms.Label miss_text;
        private System.Windows.Forms.Label accuracy_text;
        private System.Windows.Forms.Button pause_button;
        private System.Windows.Forms.Label ticktime_text;
        private System.Windows.Forms.Button menu_button;
        private System.Windows.Forms.Label fps_text;
        private System.Windows.Forms.Label time_text;
    }
}

