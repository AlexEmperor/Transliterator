using System.Text;
using Transliterator.Models;

namespace Transliterator.Services.Converter
{
    public class ConverterService
    {
        public string Convert(string text, LayoutMode currentMode)
        {
            string beginningLayout = currentMode == LayoutMode.EngToRus ? Layouts.EnglishLayout : Layouts.RussianLayout;
            string endingLayout = currentMode == LayoutMode.EngToRus ? Layouts.RussianLayout : Layouts.EnglishLayout;

            var mappingDictionary = new Dictionary<char, char>();
            for (int i = 0; i < beginningLayout.Length; i++)
            {
                mappingDictionary[beginningLayout[i]] = endingLayout[i];
            }

            var conversionResult = new StringBuilder(text.Length);
            var wordBuffer = new StringBuilder();

            foreach (char c in text)
            {
                if (ConversionParameters.IsConvertibleChar(c))
                {
                    wordBuffer.Append(c);
                }
                else
                {
                    FlushWord(conversionResult, wordBuffer, mappingDictionary);
                    conversionResult.Append(c);
                }
            }

            FlushWord(conversionResult, wordBuffer, mappingDictionary);
            return conversionResult.ToString();
        }

        private void FlushWord(StringBuilder conversionResult, StringBuilder wordBuffer,
            Dictionary<char, char> mappingDictionary)
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
                    conversionResult.Append(mappingDictionary.TryGetValue(c, out var r) ? r : c);
                }
            }
            else
            {
                conversionResult.Append(word);
            }

            wordBuffer.Clear();
        }
    }
}