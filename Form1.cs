using NHotkey.WindowsForms;

namespace Transliterator
{
    public partial class Form1 : Form
    {
        private ButtonsSetup setup = new();

        public Form1()
        {
            InitializeComponent();

            NotifyIcon trayIcon = setup.SetupTray();
            setup.SetupHotkey();

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            HotkeyManager.Current.Remove("ConvertHotkey");
            base.OnFormClosing(e);
        }
    }
}