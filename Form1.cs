using NHotkey;
using NHotkey.WindowsForms;
using System.Text;

namespace Transliterator
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private LayoutMode _currentMode = LayoutMode.EngToRus;
        private static readonly string Eng =
    "qwertyuiop[]asdfghjkl;'zxcvbnm,./`QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>?";

        private static readonly string Rus =
            "йцукенгшщзхъфывапролджэячсмитьбю.ёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,";

        private static readonly HashSet<string> ExcludedWords =
        [
            "Excel",
            "Word",
            "PowerPoint",
            "Windows",
            "GitHub",
            "VisualStudio"
        ];

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

            var engRusItem = new ToolStripMenuItem("Английский → Русский")
            {
                Checked = true
            };

            var rusEngItem = new ToolStripMenuItem("Русский → Английский");

            engRusItem.Click += (_, _) =>
            {
                _currentMode = LayoutMode.EngToRus;
                engRusItem.Checked = true;
                rusEngItem.Checked = false;
            };

            rusEngItem.Click += (_, _) =>
            {
                _currentMode = LayoutMode.RusToEng;
                engRusItem.Checked = false;
                rusEngItem.Checked = true;
            };

            contextMenu.Items.Add(engRusItem);
            contextMenu.Items.Add(rusEngItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Выход", null, (_, _) => Application.Exit());

            trayIcon.ContextMenuStrip = contextMenu;
        }


        private void SetupHotkey()
        {
            try
            {
                // Горячая клавиша Ctrl + Shift + R
                HotkeyManager.Current.AddOrReplace(
                    "ConvertHotkey",
                    Keys.R | Keys.Alt,
                    OnHotkeyPressed
                );
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
                if (!Clipboard.ContainsText())
                {
                    trayIcon.ShowBalloonTip(
                        800,
                        "Transliterator",
                        "Буфер пуст. Скопируйте текст (Ctrl+C)",
                        ToolTipIcon.Warning
                    );
                    return;
                }

                string original = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(original))
                {
                    return;
                }

                string converted = Convert(original);

                Clipboard.SetText(converted);

                trayIcon.ShowBalloonTip(
                    1000,
                    "Transliterator",
                    "Текст сконвертирован. Вставьте Ctrl+V",
                    ToolTipIcon.Info
                );
            }
            catch
            {
                trayIcon.ShowBalloonTip(
                    800,
                    "Transliterator",
                    "Ошибка конвертации",
                    ToolTipIcon.Error
                );
            }

            e.Handled = true;
        }

        private string WaitClipboardText(int timeoutMs)
        {
            int elapsed = 0;

            while (elapsed < timeoutMs)
            {
                if (Clipboard.ContainsText())
                {
                    return Clipboard.GetText();
                }

                Thread.Sleep(20);
                elapsed += 20;
            }

            return string.Empty;
        }


        private string Convert(string text)
        {
            string from = _currentMode == LayoutMode.EngToRus ? Eng : Rus;
            string to = _currentMode == LayoutMode.EngToRus ? Rus : Eng;

            var map = new Dictionary<char, char>();
            for (int i = 0; i < from.Length; i++)
            {
                map[from[i]] = to[i];
            }

            var result = new StringBuilder(text.Length);
            var wordBuffer = new StringBuilder();

            foreach (char c in text)
            {
                if (IsConvertibleChar(c))
                {
                    wordBuffer.Append(c);
                }
                else
                {
                    FlushWord(result, wordBuffer, map);
                    result.Append(c);
                }
            }

            FlushWord(result, wordBuffer, map);
            return result.ToString();
        }

        private void FlushWord(StringBuilder result, StringBuilder wordBuffer, Dictionary<char, char> translit)
        {
            if (wordBuffer.Length == 0)
            {
                return;
            }

            string word = wordBuffer.ToString();

            if (ShouldConvertWord(word))
            {
                foreach (char c in word)
                {
                    result.Append(translit.TryGetValue(c, out var r) ? r : c);
                }
            }
            else
            {
                result.Append(word);
            }

            wordBuffer.Clear();
        }

        private bool IsConvertibleChar(char c)
        {
            return Eng.Contains(c) || Rus.Contains(c) || char.IsDigit(c);
        }

        private bool ShouldConvertWord(string word)
        {
            // Явные исключения
            if (ExcludedWords.Contains(word))
            {
                return false;
            }

            // Excel, Word, GitHub
            return !IsLatinWord(word) || !StartsWithUpperLatin(word);
        }

        private bool IsLatinWord(string word)
        {
            foreach (char c in word)
            {
                if (!IsConvertibleChar(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool StartsWithUpperLatin(string word)
        {
            if (word.Length == 0)
            {
                return false;
            }

            char first = word[0];
            return first >= 'A' && first <= 'Z';
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
            HotkeyManager.Current.Remove("ConvertHotkey");
            base.OnFormClosing(e);
        }
    }
}

public enum LayoutMode
{
    EngToRus,
    RusToEng
}
