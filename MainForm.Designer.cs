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
            trackList = new ListBox();
            RemoveButton = new Button();
            VolumeBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)trackProgressBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeBar).BeginInit();
            SuspendLayout();
            // 
            // prevButton
            // 
            prevButton.Location = new Point(8, 423);
            prevButton.Name = "prevButton";
            prevButton.Size = new Size(75, 23);
            prevButton.TabIndex = 0;
            prevButton.Text = "Prev";
            prevButton.UseVisualStyleBackColor = true;
            prevButton.Click += OnClickPrevButton;
            // 
            // nextButton
            // 
            nextButton.Location = new Point(170, 423);
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
            playButton.Location = new Point(89, 411);
            playButton.Name = "playButton";
            playButton.Size = new Size(75, 35);
            playButton.TabIndex = 2;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += OnButtonPlayClick;
            // 
            // selectButton
            // 
            selectButton.Location = new Point(282, 394);
            selectButton.Name = "selectButton";
            selectButton.Size = new Size(75, 23);
            selectButton.TabIndex = 3;
            selectButton.Text = "Add";
            selectButton.UseVisualStyleBackColor = true;
            selectButton.Click += OnAddTracksButtonClick;
            // 
            // trackProgressBar
            // 
            trackProgressBar.Location = new Point(12, 323);
            trackProgressBar.Maximum = 200;
            trackProgressBar.Name = "trackProgressBar";
            trackProgressBar.Size = new Size(345, 45);
            trackProgressBar.TabIndex = 4;
            trackProgressBar.TickStyle = TickStyle.None;
            trackProgressBar.Scroll += OnTrackProgressBarScroll;
            // 
            // trackList
            // 
            trackList.AllowDrop = true;
            trackList.BorderStyle = BorderStyle.FixedSingle;
            trackList.FormattingEnabled = true;
            trackList.Location = new Point(12, 15);
            trackList.Name = "trackList";
            trackList.Size = new Size(345, 302);
            trackList.TabIndex = 5;
            trackList.DragDrop += OnDropFileInList;
            trackList.DragEnter += OnDropEnter;
            trackList.DoubleClick += TrackList_OnDoubleClick;
            // 
            // RemoveButton
            // 
            RemoveButton.Location = new Point(282, 423);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(75, 23);
            RemoveButton.TabIndex = 6;
            RemoveButton.Text = "Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += RemoveButton_Click;
            // 
            // VolumeBar
            // 
            VolumeBar.BackColor = SystemColors.ActiveCaptionText;
            VolumeBar.Location = new Point(12, 360);
            VolumeBar.Maximum = 100;
            VolumeBar.Name = "VolumeBar";
            VolumeBar.Size = new Size(104, 45);
            VolumeBar.TabIndex = 7;
            VolumeBar.TickStyle = TickStyle.None;
            VolumeBar.Value = 50;
            VolumeBar.Scroll += VolumeBar_Scroll;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(369, 458);
            Controls.Add(VolumeBar);
            Controls.Add(RemoveButton);
            Controls.Add(trackList);
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
            DragEnter += OnDropEnter;
            ((System.ComponentModel.ISupportInitialize)trackProgressBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button prevButton;
        private Button nextButton;
        private Button playButton;
        private Button selectButton;
        private TrackBar trackProgressBar;
        private ListBox trackList;
        private Button RemoveButton;
        private TrackBar VolumeBar;
    }
}
