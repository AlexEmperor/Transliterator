using NHotkey;

namespace Transliterator
{
    public class HotKeysPress
    {
        private ButtonsSetup _setup;
        private Converter converter = new();

        public HotKeysPress(ButtonsSetup setup)
        {
            _setup = setup;
        }

        public void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    return;
                }

                string original = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(original))
                {
                    return;
                }

                string converted = converter.Convert(original, _setup.CurrentMode);

                Clipboard.SetText(converted);
            }
            catch
            {

            }

            e.Handled = true;
        }
    }
}