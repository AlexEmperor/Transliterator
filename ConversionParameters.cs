namespace Transliterator
{
    public static class ConversionParameters
    {
        public static bool IsConvertibleChar(char c)
        {
            return Layouts.EnglishLayout.Contains(c) || Layouts.RussianLayout.Contains(c) || char.IsDigit(c);
        }

        public static bool ShouldConvertWord(string word)
        {
            return DeniedWords.ExcludedWords.Contains(word) ? false : !IsLatinWord(word) || !StartsWithUpperLatin(word);
        }

        private static bool IsLatinWord(string word)
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

        private static bool StartsWithUpperLatin(string word)
        {
            if (word.Length == 0)
            {
                return false;
            }

            char first = word[0];
            return first >= 'A' && first <= 'Z';
        }
    }
}