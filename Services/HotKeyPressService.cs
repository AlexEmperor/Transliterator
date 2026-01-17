using NHotkey;
using Transliterator.Services.Buttons;
using Transliterator.Services.Converter;

namespace Transliterator.Services
{
    public class HotKeyPressService
    {
        private readonly ButtonsSetupService _setup;
        private readonly ConverterService _converter = new();

        public HotKeyPressService(ButtonsSetupService setup)
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

                string clipboardText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(clipboardText))
                {
                    return;
                }

                string converted = _converter.Convert(clipboardText, _setup.CurrentMode);

                Clipboard.SetText(converted);
            }
            catch
            {

            }

            e.Handled = true;
        }
    }
}