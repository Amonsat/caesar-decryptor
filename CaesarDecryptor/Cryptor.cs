using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarDecryptor
{
    class Cryptor
    {
        static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static Dictionary<char, float> letterFrequency = new Dictionary<char, float>
        {
            { 'E', 12.7f },
            { 'T', 9.06f },
            { 'A', 8.17f },
            { 'O', 7.51f },
            { 'I', 6.97f },
            { 'N', 6.75f },
            { 'S', 6.33f },
            { 'H', 6.09f },
            { 'R', 5.99f },
            { 'D', 4.25f },
            { 'L', 4.03f },
            { 'C', 2.78f },
            { 'U', 2.76f },
            { 'M', 2.41f },
            { 'W', 2.36f },
            { 'F', 2.23f },
            { 'G', 2.02f },
            { 'Y', 1.97f },
            { 'P', 1.93f },
            { 'B', 1.49f },
            { 'V', 0.98f },
            { 'K', 0.77f },
            { 'X', 0.15f },
            { 'J', 0.15f },
            { 'Q', 0.1f },
            { 'Z', 0.05f }
        };

        public static string Decrypt(string text)
        {
            var decryptedText = String.Empty;

            Dictionary<string, float> variants = new Dictionary<string, float>();


            for (var key = 0; key < 26; key++)
            {
                var tempText = Crypt(text, key, true);
                variants[tempText] = FrequencyAnalysis(tempText);
            }

            var list = variants.Values.ToList();
            list.Sort();

            foreach (var variant in variants)
            {
                if (variant.Value == list[list.Count - 1])
                {
                    decryptedText = variant.Key;
                    break;
                }
            }

            return decryptedText;
        }

        public static string Crypt(string text, int key, bool decrypt = false)
        {
            var returnText = String.Empty;
            var sourcedText = text.ToCharArray();

            foreach (var letter in sourcedText)
            {
                for (var i = 0; i < alpha.Length; i++)
                {
                    if (Char.IsPunctuation(letter) || Char.IsWhiteSpace(letter))
                    {
                        returnText += letter;
                        break;
                    }

                    if (Char.ToUpper(letter) == alpha[i])
                    {
                        var index = 0;
                        if (decrypt) index = (i - key >= 0) ? i - key : i - key + 26;
                        else index = (i + key < 26) ? i + key : i + key - 26;

                        returnText += Char.IsUpper(letter) ? alpha[index].ToString() : alpha[index].ToString().ToLower();
                        break;
                    }
                }
            }

            return returnText;
        }

        private static float FrequencyAnalysis(string text)
        {
            Dictionary<char, int> summary = new Dictionary<char, int>();
            Dictionary<char, float> frequency = new Dictionary<char, float>();

            foreach (var letter in text.ToUpper())
            {
                if (Char.IsPunctuation(letter) || Char.IsWhiteSpace(letter)) continue;
                var sum = 0;
                summary.TryGetValue(letter, out sum);
                summary[letter] = sum + 1;
            }

            foreach (var letter in summary)
            {
                frequency.Add(letter.Key, (float)letter.Value / text.Length);
            }

            var x = 0f;
            foreach (var letter in frequency)
            {
                x += letter.Value * letterFrequency[letter.Key];
            }

            return x;
        }
    }
}
