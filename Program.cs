using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO.Pipes;
namespace AudioPlayer;

static class NativeMethods
{
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);




    public static bool ActivateFirstInstance()
    {
        var processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

        if (processes.Length > 1)
        {
            var hWnd = processes[0].MainWindowHandle;

            if (hWnd == IntPtr.Zero)
            {
                SetForegroundWindow(IntPtr.Zero);

            }
            return true;
        }
        return false;
    }

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

}

internal static class Program
{
    private static Mutex _mutex;
    private const string _appName = "AudioPlayerByZombiedEronix_IPC_MUTEX";
    private const string _pipe = "AudioPlayerByZombiedEronix_command_pipe";
    private static MainForm form;


    [STAThread]
    static void Main(string[] args)
    {
        _mutex = new(true, _appName, out bool IsFirstInstance);
        ApplicationConfiguration.Initialize();
        form = new MainForm(args);
        if (!IsFirstInstance)
        {
            NativeMethods.ActivateFirstInstance();
            SendArgumentsToFirstInstance(args);
            return;
        }

        StartIPCServer();


        Application.ThreadException += (s, e) =>
        File.WriteAllText("crash.log", e.Exception.ToString());
        Application.Run(form);
    }

    private static void StartIPCServer()
    {
        ThreadPool.QueueUserWorkItem(state =>
        {
            while (true)
            {
                try
                {
                    using (var server = new NamedPipeServerStream(_pipe, PipeDirection.In))
                    {
                        server.WaitForConnection();

                        var args = new List<string>();
                        using (var reader = new StreamReader(server))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                    args.Add(line);
                            }
                        }

                        ProcessReceivedFiles(args);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"IPC Error: {ex.Message}");
                }
            }
        });
    }

    private static void ProcessReceivedFiles(List<string> files)
    {
        if (form.InvokeRequired)
        {
            form.Invoke(new Action<List<string>>(ProcessReceivedFiles), files);
            return;
        }

        bool playlistWasEmpty = form.IsPlaylistEmpty();

        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                form.AddTrackInPlaylist(file);
            }
        }

        if (playlistWasEmpty && files.Count > 0)
        {
            form.SelectTrackInPlaylist(0);
            form.Play();
        }
    }

    public static void SendArgumentsToFirstInstance(string[] args)
    {
        try
        {
            using (var client = new NamedPipeClientStream(".", _pipe, PipeDirection.Out))
            {
                client.Connect(500);

                using (var writer = new StreamWriter(client))
                {
                    foreach (var arg in args)
                    {
                        writer.WriteLine(arg);
                    }
                }
            }
        }
        catch
        {

        }
    }

}
