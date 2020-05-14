using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace laba_8
{
    class Program
    {
        public const string path = @"D:\laba7.txt";
        static void Main(string[] args)
        {
            Console.WriteLine("Задание 1:");
            int count = 0;
            Console.Write("Введите искомое число: ");
            string search = Console.ReadLine();
            TimeSpan line;
            DateTime startTime = DateTime.Now;
            string tmp;
            string[] array = new string[100];
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    tmp = sr.ReadLine();
                    array[i] = tmp;
                }
            }
            for (int i = 0; i < array.Length; i++)
            {
                count = i;
                if (array[i] == search)
                {

                    DateTime endTime = DateTime.Now;
                    line = endTime - startTime;
                    Console.WriteLine("Позиция: " + count + "\nВремя поиска:" + line);
                    break;
                }
            }
            int[] arrayI = array.Select(ch => int.Parse(ch.ToString())).ToArray();
            int[] array2 = array.Select(ch => int.Parse(ch.ToString())).ToArray();
            int seatch = Convert.ToInt32(search);
            Search(seatch, arrayI, count, out line);
            if (count == 99)
                Console.WriteLine("Error");
            else
            Console.WriteLine("Позиция: " + count + "\nВремя поиска:" + line);
            intSearch(array2, seatch, count, out line);
            Console.WriteLine("Позиция: " + count + "\nВремя поиска:" + line);
            Console.WriteLine("Задание 2:");
            long count1 = 0;
            string text = String.Empty;
            string path1 = @"D:\text.txt";
            using (StreamReader readText = new StreamReader(path1))
            {
                text = readText.ReadToEnd();
            }
            int index = 0;
            Console.Write("Enter a substring to search for: ");
            string substring = Console.ReadLine();
            simpleSerach(text, substring, out index, out count1, out line);
            KMP(text, substring, out index, out count1, out line);
            BM(text, substring, out index, out count1, out line);
            Console.WriteLine("Индекс подстроки: " + index);
            Console.WriteLine("Ничего не найдено");
            Console.WriteLine("Кол-во сравнений: " + count1);
            Console.WriteLine("Время работы: " + line.TotalMilliseconds);
            line = TimeSpan.Parse("0");
            Console.ReadKey();
        }
        static int Search(int seatch, int[] arrayI, int count, out TimeSpan line)
        {
            int N = 100;
            int l = 0, r = N - 1;
            DateTime startTime = DateTime.Now;
            DateTime endTime;
            while (r >= l)
            {
                int mid = (l + r) / 2;

                if (arrayI[mid] == seatch)
                {
                    count = mid;
                    endTime = DateTime.Now;
                    line = endTime - startTime;
                    return mid;
                }
                if (arrayI[mid] > seatch)
                {
                    r = mid - 1;
                    count = mid;
                }
                else
                {
                    l = mid + 1;
                    count = mid;
                }
            }
            endTime = DateTime.Now;
            line = endTime - startTime;
            return -1;
        }
        static int intSearch(int[] array2, int seatch, int count, out TimeSpan line)
        {
            int N = 100;
            int l = 0, r = N - 1;
            DateTime startTime = DateTime.Now;
            DateTime endTime;
            count = 0;
            while (r >= l)
            {
                int mid = l + (r - l) * (seatch - array2[l]) / (array2[r] - array2[l]);

                if (seatch < array2[mid])
                {
                    r = mid - 1;
                    count = mid;
                }
                else if (seatch > array2[mid])
                {
                    l = mid + 1;
                    count = mid;
                }
                else
                {
                    count = mid;
                    endTime = DateTime.Now;
                    line = endTime - startTime;
                    return mid;
                }
            }
            endTime = DateTime.Now;
            line = endTime - startTime;
            return -1;
        }
            public static void simpleSerach(string text, string substring, out int index, out long count1, out TimeSpan line)
            {
                index = -1;
                count1 = 0;
                DateTime startTime = DateTime.Now;
                for (int i = 0; i < text.Length - substring.Length; i++)
                {
                    for (int j = 0; substring[j] == text[i + j]; j++, count1++)
                        if (j == substring.Length - 1)
                        {
                            index = i;
                            break;
                        }
                    count1++;
                    if (index == i)
                        break;
                }
                DateTime endTime = DateTime.Now;
                line = endTime - startTime;
            }
            public static int[] cpf(string s)
            {
                int[] pi = new int[s.Length];
                int j = 0;
                pi[0] = 0;
                for (int i = 1; i < s.Length; i++)
                {
                    while (j > 0 && s[j] != s[i])
                        j = pi[j - 1];
                    if (s[j] == s[i])
                        j++;
                    pi[i] = j;
                }
                return pi;
            }
            public static void KMP(string text, string substring, out int index, out long count1, out TimeSpan line)
            {
                count1 = 0;
                DateTime startTime = DateTime.Now;
                int[] prefix = cpf(substring);
                int j = 0;
                index = -1;
                for (int i = 1; i <= text.Length; i++)
                {
                    while (j > 0 && substring[j] != text[i - 1])
                    {
                        j = prefix[j - 1];
                        count1++;
                    }
                    if (substring[j] == text[i - 1])
                    {
                        j++;
                        count1++;
                    }
                    if (j == substring.Length)
                    {
                        index = i - substring.Length;
                        break;
                    }
                }
                DateTime endTime = DateTime.Now;
                line = endTime - startTime;
            }
            public static int[] badCharactersTable(string substring)
            {
                int[] badShift = new int[256];
                for (int i = 0; i < 256; i++)
                    badShift[i] = -1;
                for (int i = 0; i < substring.Length - 1; i++)
                    badShift[(int)substring[i]] = i;
                return badShift;
            }
            public static int[] suffixesCreate(string substring)
            {
                int[] suffixes = new int[substring.Length];
                suffixes[substring.Length - 1] = substring.Length;
                int right = substring.Length - 1, left = 0;
                for (int i = right - 1; i >= 0; --i)
                {
                    if (i > right && suffixes[i + substring.Length - 1 - left] < i - right)
                        suffixes[i] = suffixes[i + substring.Length - 1 - left];
                    else if (i < right)
                        right = i;
                    left = i;
                    while (right >= 0 && substring[right] == substring[right + substring.Length - 1 - left])
                        right--;
                    suffixes[i] = left - right;
                }
                return suffixes;
            }
            public static int[] goodSuffixTable(string substring)
            {
                int m = substring.Length;
                int[] suffixes = suffixesCreate(substring);
                int[] goodSuffixes = new int[substring.Length];
                for (int i = 0; i < substring.Length; i++)
                    goodSuffixes[i] = substring.Length;
                for (int i = substring.Length - 1; i >= 0; i--)
                    if (suffixes[i] == i + 1)
                        for (int j = 0; j < substring.Length - i - 1; j++)
                            if (goodSuffixes[j] == substring.Length)
                                goodSuffixes[j] = substring.Length - i - 1;
                for (int i = 0; i < substring.Length - 2; i++)
                    goodSuffixes[substring.Length - 1 - suffixes[i]] = substring.Length - i - 1;
                return goodSuffixes;
            }
            public static void BM(string text, string substring, out int shift, out long count1, out TimeSpan line)
            {
                count1 = 0;
                DateTime startTime = DateTime.Now, endTime;
                line = TimeSpan.MinValue;
                if (substring.Length > text.Length)
                    shift = -1;
                else
                {
                    int[] badShift = badCharactersTable(substring);
                    int[] goodSuffix = goodSuffixTable(substring);
                    shift = 0;
                    while (shift <= text.Length - substring.Length)
                    {
                        int i;
                        for (i = substring.Length - 1; i >= 0 && substring[i] == text[i + shift]; i--, count1++) ;
                        if (i < 0)
                        {
                            endTime = DateTime.Now;
                            line = endTime - startTime;
                            return;
                        }
                        shift += Math.Max(i - badShift[(int)text[shift + i]], goodSuffix[i]);
                    }
                    shift = -1;
                    endTime = DateTime.Now;
                    line = endTime - startTime;
                }
            }
        }
    }
