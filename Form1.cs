using NHotkey.WindowsForms;
using Transliterator.Services.Buttons;

namespace Transliterator
{
    public partial class Form1 : Form
    {
        private ButtonsSetupService _setup = new();

        public Form1()
        {
            InitializeComponent();

            NotifyIcon trayIcon = _setup.SetupTray();
            _setup.SetupHotkey();

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