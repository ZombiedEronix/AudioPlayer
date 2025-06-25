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



        Application.Run(form);
    }

    private static void StartIPCServer()
    {
        ThreadPool.QueueUserWorkItem(state =>
        {
            while (true)
            {
                using (var server = new NamedPipeServerStream(_pipe, PipeDirection.In))
                using (var reader = new StreamReader(server))
                {
                    server.WaitForConnection();

                    var args = new System.Collections.Concurrent.ConcurrentQueue<string>();
                    while (!reader.EndOfStream)
                    {
                        args.Enqueue(reader.ReadLine());
                    }

                    foreach (string arg in args)
                    {
                        form.SelectFile(arg);
                        form.Play();
                    }

                }
            }
        });
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
