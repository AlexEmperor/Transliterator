using NHotkey;
using NHotkey.WindowsForms;
using System.Text;

namespace Transliterator
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;

        public Form1()
        {
            InitializeComponent();
            SetupTray();
            SetupHotkey();

            // Сразу сворачиваем в трей
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void SetupTray()
        {
            trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Application,
                Visible = true,
                Text = "EngRus Converter"
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Выход", null, (s, e) => Application.Exit());
            trayIcon.ContextMenuStrip = contextMenu;
        }

        private void SetupHotkey()
        {
            try
            {
                // Горячая клавиша Ctrl + Shift + R
                HotkeyManager.Current.AddOrReplace("ConvertHotkey", Keys.R | Keys.Control | Keys.Shift, OnHotkeyPressed);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось зарегистрировать Hotkey: " + ex.Message);
            }
        }

        private void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            try
            {
                // 1. Скопировать выделенный текст
                SendKeys.SendWait("^c");
                Thread.Sleep(50); // пауза, чтобы текст успел попасть в буфер

                if (Clipboard.ContainsText())
                {
                    string original = Clipboard.GetText();
                    string converted = ConvertEngToRus(original);

                    // 2. Заменяем выделение на новый текст
                    Clipboard.SetText(converted);

                    // 3. Вставляем сконвертированный текст
                    SendKeys.SendWait("^v");

                    trayIcon.ShowBalloonTip(1000, "Eng→Rus Converter", "Текст заменён!", ToolTipIcon.Info);
                }
            }
            catch
            {
                trayIcon.ShowBalloonTip(1000, "Eng→Rus Converter", "Ошибка при конвертации текста", ToolTipIcon.Error);
            }

            e.Handled = true;
        }

        private string ConvertEngToRus(string text)
        {
            var eng = "qwertyuiop[]asdfghjkl;'zxcvbnm,./`QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>?";
            var rus = "йцукенгшщзхъфывапролджэячсмитьбю.ёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,";

            var translit = new Dictionary<char, char>();
            for (int i = 0; i < eng.Length; i++)
            {
                translit[eng[i]] = rus[i];
            }

            var result = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                result.Append(translit.ContainsKey(c) ? translit[c] : c);
            }

            return result.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
            HotkeyManager.Current.Remove("ConvertHotkey");
            base.OnFormClosing(e);
        }
    }
}