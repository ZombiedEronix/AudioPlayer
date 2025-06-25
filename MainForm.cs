
namespace AudioPlayer
{
    using System.Windows.Forms;
    using NAudio.Wave;

    public partial class MainForm : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFileReader;
        private bool fileSelected;
        private Timer progressTimer;
        private string selectedFile;

        public MainForm(string[] files)
        {
            InitializeComponent();
            progressTimer = ProgressCountWhile();
            Initialize();
            this.AllowDrop = true;
            if (files.Length > 0 && File.Exists(files[0]))
            {
                SelectFile(files[0]);
                Play();
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

        private void OnDropFile(object e, DragEventArgs args)
        {


            string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);


            string filepath = files[0];


            SelectFile(filepath);
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


                    float i = (float)(current / total);
                    trackProgressBar.Value = (int)(i * trackProgressBar.Maximum);
                }
            };
            return timer;
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            trackProgressBar.Value = 0;
            playButton.Text = "Play";
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
                outputDevice.PlaybackStopped += OnPlaybackStopped;
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
            switch (outputDevice.PlaybackState)
            {
                case PlaybackState.Playing:
                    outputDevice.Pause();
                    playButton.Text = "Play";
                    break;
                default:
                    if (fileSelected)
                    {
                        outputDevice.Play();
                        playButton.Text = "Pause";
                    }
                    break;
            }
        }

        private void OnSelectButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectFile(dialog.FileName);
            }
        }



        public void SelectFile(string directory)
        {
            selectedFile = directory;
            fileSelected = true;
            outputDevice.Stop();
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader.Close();
            }
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

        }

        private void trackProgressBar_Scroll(object sender, EventArgs e)
        {
            progressTimer.Stop();
            audioFileReader.Position = audioFileReader.Length / trackProgressBar.Maximum * trackProgressBar.Value;
            progressTimer.Start();
        }
        
    }
}

//public class MainForm : Form
//{
//    private WaveOutEvent outputDevice;
//    private AudioFileReader audioFile;

//    public MainForm()
//    {
//        InitializeComponent();
//    }

//    private void InitializeComponent()
//    {
//        var flowPanel = new FlowLayoutPanel();
//        flowPanel.FlowDirection = FlowDirection.LeftToRight;
//        flowPanel.Margin = new Padding(10);

//        var buttonPlay = new Button();
//        buttonPlay.Text = "Play";
//        buttonPlay.Click += OnButtonPlayClick;
//        flowPanel.Controls.Add(buttonPlay);

//        var buttonStop = new Button();
//        buttonStop.Text = "Stop";
//        buttonStop.Click += OnButtonStopClick;
//        flowPanel.Controls.Add(buttonStop);

//        this.Controls.Add(flowPanel);

//        this.FormClosing += OnButtonStopClick;
//    }
//}
