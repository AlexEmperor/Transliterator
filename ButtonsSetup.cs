using NHotkey.WindowsForms;

namespace Transliterator
{
    public class ButtonsSetup
    {
        private Click click;
        private HotKeysPress hotKeysPress;
        private NotifyIcon? trayIcon;

        public LayoutMode CurrentMode { get; set; } = LayoutMode.EngToRus;

        public ButtonsSetup()
        {
            click = new Click(this); // передаём сам объект
            hotKeysPress = new HotKeysPress(this);
        }

        public NotifyIcon SetupTray()
        {
            trayIcon = new NotifyIcon
            {
                Icon = new Icon("RU_EN_icon.ico"),
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