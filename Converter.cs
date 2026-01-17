using System.Text;

namespace Transliterator
{
    public class Converter
    {
        public string Convert(string text, LayoutMode _currentMode)
        {
            string from = _currentMode == LayoutMode.EngToRus ? Layouts.EnglishLayout : Layouts.RussianLayout;
            string to = _currentMode == LayoutMode.EngToRus ? Layouts.RussianLayout : Layouts.EnglishLayout;

            var map = new Dictionary<char, char>();
            for (int i = 0; i < from.Length; i++)
            {
                map[from[i]] = to[i];
            }

            var result = new StringBuilder(text.Length);
            var wordBuffer = new StringBuilder();

            foreach (char c in text)
            {
                if (ConversionParameters.IsConvertibleChar(c))
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

            if (ConversionParameters.ShouldConvertWord(word))
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
    }
}