namespace AudioPlayer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            prevButton = new Button();
            nextButton = new Button();
            playButton = new Button();
            selectButton = new Button();
            trackProgressBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)trackProgressBar).BeginInit();
            SuspendLayout();
            // 
            // prevButton
            // 
            prevButton.Location = new Point(12, 73);
            prevButton.Name = "prevButton";
            prevButton.Size = new Size(75, 23);
            prevButton.TabIndex = 0;
            prevButton.Text = "Prev";
            prevButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            nextButton.Location = new Point(174, 73);
            nextButton.Name = "nextButton";
            nextButton.Size = new Size(75, 23);
            nextButton.TabIndex = 1;
            nextButton.Text = "Next";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += OnClickNextButton;
            // 
            // playButton
            // 
            playButton.Font = new Font("Segoe UI", 12F);
            playButton.Location = new Point(93, 61);
            playButton.Name = "playButton";
            playButton.Size = new Size(75, 43);
            playButton.TabIndex = 2;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += OnButtonPlayClick;
            // 
            // selectButton
            // 
            selectButton.Location = new Point(255, 73);
            selectButton.Name = "selectButton";
            selectButton.Size = new Size(102, 23);
            selectButton.TabIndex = 3;
            selectButton.Text = "Select File...";
            selectButton.UseVisualStyleBackColor = true;
            selectButton.Click += OnSelectButtonClick;
            // 
            // trackProgressBar
            // 
            trackProgressBar.Location = new Point(12, 10);
            trackProgressBar.Maximum = 200;
            trackProgressBar.Name = "trackProgressBar";
            trackProgressBar.Size = new Size(345, 45);
            trackProgressBar.TabIndex = 4;
            trackProgressBar.TickStyle = TickStyle.None;
            trackProgressBar.Scroll += trackProgressBar_Scroll;
            //trackProgressBar.MouseUp += trackProgressBar_MouseUp;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(369, 116);
            Controls.Add(trackProgressBar);
            Controls.Add(selectButton);
            Controls.Add(playButton);
            Controls.Add(nextButton);
            Controls.Add(prevButton);
            ForeColor = SystemColors.ControlText;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "AudioPlayer v0.0.1f";
            Load += FormInitializing;
            DragDrop += OnDropFile;
            DragEnter += OnDropEnter;
            ((System.ComponentModel.ISupportInitialize)trackProgressBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button prevButton;
        private Button nextButton;
        private Button playButton;
        private Button selectButton;
        private TrackBar trackProgressBar;
    }
}
