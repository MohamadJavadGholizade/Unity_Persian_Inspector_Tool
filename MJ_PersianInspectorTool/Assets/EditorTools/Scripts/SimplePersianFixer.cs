using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MJ.EditorTools
{
    public static class SimplePersianFixer
    {
        public static string Fix(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            string[] lines = str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string shaped = Shape(lines[i]);

                string reversed = ReverseText(shaped);

                lines[i] = FixEnglishOrientation(reversed);
            }

            return string.Join("\n", lines);
        }

        private static string ReverseText(string input)
        {
            char[] array = input.ToCharArray();
            System.Array.Reverse(array);
            return new string(array);
        }

        private static string FixEnglishOrientation(string input)
        {
            string pattern = @"[a-zA-Z0-9\+\-\*\/\.\,\!\?\:\;\'\""\(\)\[\]\@\#\$\%\&\=_\<\>]+(?:\s+[a-zA-Z0-9\+\-\*\/\.\,\!\?\:\;\'\""\(\)\[\]\@\#\$\%\&\=_\<\>]+)*";

            return Regex.Replace(input, pattern, m => 
            {
                char[] arr = m.Value.ToCharArray();
                System.Array.Reverse(arr);
                return new string(arr);
            });
        }

        private static string Shape(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char current = input[i];
                char prev = (i > 0) ? input[i - 1] : ' ';
                char next = (i < input.Length - 1) ? input[i + 1] : ' ';

                if (PersianMap.ContainsKey(current))
                {
                    bool prevJoins = CanJoinNext(prev);
                    bool nextJoins = CanJoinPrev(next);

                    int formIndex = 0;
                    if (!prevJoins && !nextJoins) formIndex = 0;
                    else if (prevJoins && !nextJoins) formIndex = 1;
                    else if (!prevJoins && nextJoins) formIndex = 2;
                    else if (prevJoins && nextJoins) formIndex = 3;

                    int[] forms = PersianMap[current];
                    if (formIndex >= forms.Length) formIndex = 0;

                    sb.Append((char)forms[formIndex]);
                }
                else
                {
                    sb.Append(current);
                }
            }

            return sb.ToString();
        }

        private static bool CanJoinNext(char c) =>
            (PersianMap.ContainsKey(c) && PersianMap[c].Length == 4) || (c == 0x0640);

        private static bool CanJoinPrev(char c) => PersianMap.ContainsKey(c);

        private static readonly Dictionary<char, int[]> PersianMap = new Dictionary<char, int[]>
        {
            { 'آ', new int[] { 0xFE81, 0xFE82 } },
            { 'ا', new int[] { 0xFE8D, 0xFE8E } },
            { 'ب', new int[] { 0xFE8F, 0xFE90, 0xFE91, 0xFE92 } },
            { 'پ', new int[] { 0xFB56, 0xFB57, 0xFB58, 0xFB59 } },
            { 'ت', new int[] { 0xFE95, 0xFE96, 0xFE97, 0xFE98 } },
            { 'ث', new int[] { 0xFE99, 0xFE9A, 0xFE9B, 0xFE9C } },
            { 'ج', new int[] { 0xFE9D, 0xFE9E, 0xFE9F, 0xFEA0 } },
            { 'چ', new int[] { 0xFB7A, 0xFB7B, 0xFB7C, 0xFB7D } },
            { 'ح', new int[] { 0xFEA1, 0xFEA2, 0xFEA3, 0xFEA4 } },
            { 'خ', new int[] { 0xFEA5, 0xFEA6, 0xFEA7, 0xFEA8 } },
            { 'د', new int[] { 0xFEA9, 0xFEAA } },
            { 'ذ', new int[] { 0xFEAB, 0xFEAC } },
            { 'ر', new int[] { 0xFEAD, 0xFEAE } },
            { 'ز', new int[] { 0xFEAF, 0xFEB0 } },
            { 'ژ', new int[] { 0xFB8A, 0xFB8B } },
            { 'س', new int[] { 0xFEB1, 0xFEB2, 0xFEB3, 0xFEB4 } },
            { 'ش', new int[] { 0xFEB5, 0xFEB6, 0xFEB7, 0xFEB8 } },
            { 'ص', new int[] { 0xFEB9, 0xFEBA, 0xFEBB, 0xFEBC } },
            { 'ض', new int[] { 0xFEBD, 0xFEBE, 0xFEBF, 0xFEC0 } },
            { 'ط', new int[] { 0xFEC1, 0xFEC2, 0xFEC3, 0xFEC4 } },
            { 'ظ', new int[] { 0xFEC5, 0xFEC6, 0xFEC7, 0xFEC8 } },
            { 'ع', new int[] { 0xFEC9, 0xFECA, 0xFECB, 0xFECC } },
            { 'غ', new int[] { 0xFECD, 0xFECE, 0xFECF, 0xFED0 } },
            { 'ف', new int[] { 0xFED1, 0xFED2, 0xFED3, 0xFED4 } },
            { 'ق', new int[] { 0xFED5, 0xFED6, 0xFED7, 0xFED8 } },
            { 'ک', new int[] { 0xFED9, 0xFEDA, 0xFEDB, 0xFEDC } },
            { 'ك', new int[] { 0xFED9, 0xFEDA, 0xFEDB, 0xFEDC } },
            { 'گ', new int[] { 0xFB92, 0xFB93, 0xFB94, 0xFB95 } },
            { 'ل', new int[] { 0xFEDD, 0xFEDE, 0xFEDF, 0xFEE0 } },
            { 'م', new int[] { 0xFEE1, 0xFEE2, 0xFEE3, 0xFEE4 } },
            { 'ن', new int[] { 0xFEE5, 0xFEE6, 0xFEE7, 0xFEE8 } },
            { 'و', new int[] { 0xFEEE, 0xFEEF } },
            { 'ه', new int[] { 0xFEE9, 0xFEEA, 0xFEEB, 0xFEEC } },
            { 'ی', new int[] { 0xFBFC, 0xFBFD, 0xFBFE, 0xFBFF } },
            { 'ي', new int[] { 0xFBFC, 0xFBFD, 0xFBFE, 0xFBFF } },
            { 'ئ', new int[] { 0xFE89, 0xFE8A, 0xFE8B, 0xFE8C } }
        };
    }
}