
namespace AudioPlayer
{
    using System.Windows.Forms;
    using NAudio.Wave;

    public enum PlaybackStopReason
    {
        User,
        TrackEnded,
        Error,
    }

    public enum RepeatMode
    {
        None,
        RepeatOneTrack,
        RepeatList,
    }

    public partial class MainForm : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFileReader;
        private bool fileSelected;
        private Timer progressTimer;
        private string selectedFile;

        private Action<PlaybackStopReason> PlayBackStopped;

        private int SelectedTrack;


        private RepeatMode repeatMode = RepeatMode.None;



        public MainForm(string[] files)
        {
            InitializeComponent();
            progressTimer = ProgressCountWhile();
            Initialize();
            this.AllowDrop = true;
            if (files.Length == 1)
            {
                trackList.Items.Clear();
                trackList.Items.Add(files[0]);
                SelectFile(files[0]);
                Play();
            }
            else if (files.Length > 0)
            {

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        trackList.Items.Add(file);
                    }
                }

            }
        }

        private void OnDropEnter(object e, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                args.Effect = DragDropEffects.Copy;
            }
            else
            {
                args.Effect = DragDropEffects.None;
            }
        }

        private void OnDropFileInList(object e, DragEventArgs args)
        {

            string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    trackList.Items.Add(file);
                }
            }

        }

        private void OnPlaybackStopped(PlaybackStopReason reason)
        {

            switch (reason)
            {
                case PlaybackStopReason.TrackEnded:
                    switch (repeatMode)
                    {
                        case RepeatMode.None:

                            PlayNextFile();

                            break;

                        case RepeatMode.RepeatOneTrack: Play(); break;

                    }
                    break;

                case PlaybackStopReason.Error: MessageBox.Show("Воспроизведение остановлено из за ошибки !", "Error!", MessageBoxButtons.OK); break;
            }
        }

        private void FormInitializing(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        public void Initialize()
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += (e, a) =>
                {
                    if (audioFileReader.Position > audioFileReader.Length - 2048)
                    {

                        PlayBackStopped?.Invoke(PlaybackStopReason.TrackEnded);
                    }
                };
                PlayBackStopped += OnPlaybackStopped;
            }

        }

        public void OnButtonPlayClick(object sender, EventArgs e)
        {
            if (!fileSelected)
            {
                OnSelectButtonClick(this, new EventArgs());
            }
            Play();
        }

        public void Play()
        {
            if (this.IsDisposed) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(Play));
                return;
            }

            try
            {
                switch (outputDevice?.PlaybackState)
                {
                    case PlaybackState.Playing:
                        outputDevice.Pause();
                        playButton.Text = "Paused";
                        break;
                    default:
                        if (fileSelected && audioFileReader != null)
                        {
                            outputDevice?.Play();
                            playButton.Text = "Playing";
                        }
                        break;
                }
            }
            catch (ObjectDisposedException) { }
        }

        private void OnSelectButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectFile(dialog.FileName);
                trackList.Items.Add(dialog.FileName);
                Play();
            }
        }

        public void SelectFile(string directory)
        {
            outputDevice.Stop();
            audioFileReader?.Dispose();
            selectedFile = directory;
            fileSelected = true;
            try
            {
                audioFileReader = new(selectedFile);
            }
            catch
            {
                MessageBox.Show("Формат файла не поддерживается", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            outputDevice.Init(audioFileReader);
        }

        private void OnClickNextButton(object sender, EventArgs e)
        {
            PlayNextFile();
        }

        private void PlayNextFile()
        {

            SelectedTrack++;
            if (SelectedTrack < trackList.Items.Count)
            {
                trackList.SelectedIndex = SelectedTrack;
                SelectFile(trackList.Items[SelectedTrack].ToString());
                Play();
                return;
            }
        }
        private void PlayPrevFile()
        {
            if(SelectedTrack-1 !< 0) SelectedTrack--;
            if (SelectedTrack < trackList.Items.Count)
            {
                trackList.SelectedIndex = SelectedTrack;
                SelectFile(trackList.Items[SelectedTrack].ToString());
                Play();
                return;
            }
        }

        private void trackProgressBar_Scroll(object sender, EventArgs e)
        {
            progressTimer.Stop();
            audioFileReader.Position = audioFileReader.Length / trackProgressBar.Maximum * trackProgressBar.Value;
            progressTimer.Start();
        }

        private void OnSelectedTrackInList(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex > trackList.Items.Count - 1 || trackList.SelectedIndex < 0)
            {
                trackList.SelectedIndex = SelectedTrack;
                return;
            }
            PlayBackStopped?.Invoke(PlaybackStopReason.User);

            SelectedTrack = trackList.SelectedIndex;
            SelectFile(trackList.SelectedItem.ToString());
            Play();
        }
        private Timer ProgressCountWhile()
        {
            Timer timer = new Timer();
            timer.Start();
            timer.Interval = 200;
            timer.Tick += (s, e) =>
            {
                if (fileSelected && outputDevice != null && audioFileReader != null)
                {
                    TimeSpan current = audioFileReader.CurrentTime;
                    TimeSpan total = audioFileReader.TotalTime;
#if DEBUG
                    Text = $"DEBUG : Tracks : {trackList.Items.Count} Selected:{SelectedTrack}";
#endif
                    float i = (float)(current / total);
                    trackProgressBar.Value = (int)(i * trackProgressBar.Maximum);
                }
            };
            return timer;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (trackList.Items.Count > 0)
            {
                if (trackList.Items[trackList.SelectedIndex] != null)
                {
                    trackList.Items.Remove(trackList.SelectedIndex);
                    trackList.BeginUpdate();
                }
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            PlayPrevFile();
        }
    }
}