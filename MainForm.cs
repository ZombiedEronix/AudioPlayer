
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

        private float Volume = .5f;
        private int SelectedTrack;

        private RepeatMode repeatMode = RepeatMode.None;

        public bool IsPlaylistEmpty() => trackList.Items.Count == 0;

        public MainForm(string[] files)
        {
            InitializeComponent();
            progressTimer = ProgressCountWhile();
            Initialize();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AllowDrop = true;

            if (files.Length > 0)
            {
                if (files.Length == 1)
                {
                    trackList.Items.Clear();
                }

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        AddTrackInPlaylist(file);
                    }
                }


                if (trackList.Items.Count > 0)
                {
                    SelectTrackInPlaylist(0);
                    Play();
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
                    AddTrackInPlaylist(file);
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

        public void Initialize()
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += (e, a) =>
                {
                    if (audioFileReader.CurrentTime >= audioFileReader.TotalTime - TimeSpan.FromMilliseconds(100))
                    {
                        PlayBackStopped?.Invoke(PlaybackStopReason.TrackEnded);
                    }
                };
                PlayBackStopped += OnPlaybackStopped;
            }

        }


        public void SelectTrackInPlaylist(int id)
        {
            if (id < 0)
            {
                new NotImplementedException();
                return;
            }
            SelectFile(trackList.Items[id].ToString());
        }

        public void ClearPlayList()
        {
            trackList.Items.Clear();
        }

        public int AddTrackInPlaylist(string[] tracks)
        {
            foreach (string track in tracks)
            {
                trackList.Items.Add(track);
            }
            return trackList.Items.Count - 1;
        }
        public int AddTrackInPlaylist(string track)
        {
            trackList.Items.Add(track);
            return trackList.Items.Count-1;
        }



        public void OnButtonPlayClick(object sender, EventArgs e)
        {
            if (!fileSelected)
            {
                if (trackList.Items.Count == 0)
                {
                    OnAddTracksButtonClick(this, new EventArgs());
                }
                else
                {
                    SelectTrackInPlaylist(0);
                }
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

        private void OnAddTracksButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Поддерживаемые аудиофайлы (*.mp3;*.wav;*.ogg;*.flac;*.mid;*.midi)|*.mp3;*.wav;*.ogg;*.flac;*.mid;*.midi|Все файлы (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileNames.Length > 0)
                {
                    foreach (string file in dialog.FileNames)
                    {
                        AddTrackInPlaylist(file);
                    }
                }

            }
        }

        public void SelectFile(string directory)
        {
            PlayBackStopped.Invoke(PlaybackStopReason.User);

            outputDevice.Stop();
            audioFileReader?.Dispose();

            selectedFile = directory;
            fileSelected = true;

            try
            {
                audioFileReader = new(selectedFile);
                audioFileReader.Volume = Volume;
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
            if (SelectedTrack < trackList.Items.Count - 1)
            {
                SelectedTrack++;
                trackList.SelectedIndex = SelectedTrack;
                SelectTrackInPlaylist(SelectedTrack);
                Play();
                return;
            }
        }
        private void PlayPrevFile()
        {
            if (SelectedTrack - 1 > -1) SelectedTrack--;
            else return;

            if (SelectedTrack < trackList.Items.Count - 1)
            {
                trackList.SelectedIndex = SelectedTrack;
                SelectTrackInPlaylist(SelectedTrack);
                Play();
                return;
            }
        }

        private void OnTrackProgressBarScroll(object sender, EventArgs e)
        {
            progressTimer.Stop();
            audioFileReader.Position = audioFileReader.Length / trackProgressBar.Maximum * trackProgressBar.Value;
            progressTimer.Start();
        }

        private void TrackList_OnDoubleClick(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex > trackList.Items.Count - 1 || trackList.SelectedIndex < 0)
            {
                trackList.SelectedIndex = SelectedTrack;
                return;
            }
            PlayBackStopped?.Invoke(PlaybackStopReason.User);

            SelectedTrack = trackList.SelectedIndex;
            SelectTrackInPlaylist(trackList.SelectedIndex);
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
            if (trackList.SelectedIndex == -1)
                return;

            int removeIndex = trackList.SelectedIndex;

            if (removeIndex == SelectedTrack)
            {
                outputDevice?.Stop();
                fileSelected = false;
            }

            trackList.Items.RemoveAt(removeIndex);

            if (SelectedTrack >= removeIndex)
                SelectedTrack--;

            if (trackList.Items.Count == 0)
            {
                SelectedTrack = -1;
                fileSelected = false;
            }
        }

        private void OnClickPrevButton(object sender, EventArgs e)
        {
            PlayPrevFile();
        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            Volume = (float)VolumeBar.Value / VolumeBar.Maximum;
            audioFileReader.Volume = Volume;
        }
    }
}