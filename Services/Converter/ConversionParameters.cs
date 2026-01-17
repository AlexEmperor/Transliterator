using Transliterator.Models;

namespace Transliterator.Services.Converter
{
    public static class ConversionParameters
    {
        public static bool IsConvertibleChar(char character)
        {
            return Layouts.EnglishLayout.Contains(character) || Layouts.RussianLayout.Contains(character)
                || char.IsDigit(character);
        }

        public static bool ShouldConvertWord(string word)
        {
            return DeniedWords.ExcludedWords.Contains(word) ? false :
                !IsLatinWord(word) || !StartsWithUpperLatin(word);
        }

        private static bool IsLatinWord(string word)
        {
            foreach (char character in word)
            {
                if (!IsConvertibleChar(character))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool StartsWithUpperLatin(string word)
        {
            if (word.Length == 0)
            {
                return false;
            }

            char firstCharacter = word[0];
            return firstCharacter >= 'A' && firstCharacter <= 'Z';
        }
    }
}