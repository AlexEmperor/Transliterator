using NHotkey.WindowsForms;
using Transliterator.Models;

namespace Transliterator.Services.Buttons
{
    public class ButtonsSetupService
    {
        private readonly ClickService click;
        private readonly HotKeyPressService hotKeysPress;
        private NotifyIcon? trayIcon;

        public LayoutMode CurrentMode { get; set; } = LayoutMode.EngToRus;

        public ButtonsSetupService()
        {
            click = new ClickService(this);
            hotKeysPress = new HotKeyPressService(this);
        }

        public NotifyIcon SetupTray()
        {
            trayIcon = new NotifyIcon
            {
                Icon = new Icon("Resources/RU_EN_icon.ico"),
                Visible = true,
                Text = "EngRus Converter"
            };

            var engRusItem = click.ButtonEngRusCLick();
            var rusEngItem = click.ButtonRusEngCLick();

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(engRusItem);
            contextMenu.Items.Add(rusEngItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Выход", null, (_, _) => Application.Exit());

            trayIcon.ContextMenuStrip = contextMenu;

            return trayIcon;
        }

        public void SetupHotkey()
        {
            try
            {
                HotkeyManager.Current.AddOrReplace(
                    "ConvertHotkey",
                    Keys.R | Keys.Control | Keys.Shift,
                    hotKeysPress.OnHotkeyPressed
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось зарегистрировать Hotkey: " + ex.Message);
            }
        }
    }
}