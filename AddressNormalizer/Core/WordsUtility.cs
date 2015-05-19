using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AddressNormalizer.Core
{
    public class WordsUtility
    {
        public readonly static List<WORDS> Words;
        public readonly static List<FLAG_WORDS> FlagWords;
        static WordsUtility()
        {
            Words = new List<WORDS>();
            FlagWords = new List<FLAG_WORDS>();
            InitWords("AddressNormalizer.Dictionaries.words.dic");
            InitFlagWords();
        }
        protected static void InitFlagWords()
        {
            FlagWords.Add(new FLAG_WORDS() { KEY = 1, LEVEL = new int[] { 1 }, WORD = "省" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 2, LEVEL = new int[] { 1 }, WORD = "特别行政区" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 3, LEVEL = new int[] { 2, 3, 4 }, WORD = "区" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 4, LEVEL = new int[] { 2, 3, 4 }, WORD = "地区" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 5, LEVEL = new int[] { 1, 2 }, WORD = "市" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 6, LEVEL = new int[] { 1 }, WORD = "自治区" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 7, LEVEL = new int[] { 2 }, WORD = "自治州" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 8, LEVEL = new int[] { 3 }, WORD = "自治县" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 9, LEVEL = new int[] { 2, 3, 4 }, WORD = "县" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 10, LEVEL = new int[] { 3, 4 }, WORD = "镇" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 11, LEVEL = new int[] { 3, 4 }, WORD = "乡" });
            FlagWords.Add(new FLAG_WORDS() { KEY = 12, LEVEL = new int[] { 3, 4 }, WORD = "街道" });
        }
        protected static void InitWords(string dicPath)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(WordsUtility));
            using (Stream stream = assembly.GetManifestResourceStream(dicPath))
            {
                while (true)
                {
                    WORDS w = new WORDS();
                    byte[] intByte = new byte[4];
                    stream.Read(intByte, 0, 4);
                    w.KEY = BitConverter.ToInt32(intByte, 0);
                    if (w.KEY == 0) return;
                    stream.Read(intByte, 0, 4);
                    int len = BitConverter.ToInt32(intByte, 0);
                    byte[] str = new byte[len];
                    stream.Read(str, 0, len);
                    w.WORD = Encoding.Unicode.GetString(str);
                    stream.Read(intByte, 0, 4);
                    len = BitConverter.ToInt32(intByte, 0);
                    w.TREE_PATH = new int[len];
                    for (int i = 0; i < len; i++)
                    {
                        stream.Read(intByte, 0, 4);
                        w.TREE_PATH[i] = BitConverter.ToInt32(intByte, 0);
                    }
                    stream.Read(intByte, 0, 4);
                    w.RANK = BitConverter.ToSingle(intByte, 0);
                    stream.Read(intByte, 0, 4);
                    w.FLAG_KEY = BitConverter.ToInt32(intByte, 0);
                    Words.Add(w);
                }
            }
        }
        public FLAG_WORDS GetExt(int i)
        {
            if (i <= 0 || i > 12)
                return new FLAG_WORDS();
            return FlagWords[i - 1];
        }
        public WORDS Find(int key)
        {
            if (Words == null) { throw new ArgumentException("array is null"); }
            if (Words[Words.Count() - 1].KEY == key) { return Words[Words.Count() - 1]; }
            return Find(0, Words.Count() - 1, key);
        }
        private WORDS Find(int beginIndex, int endIndex, int value)
        {
            int middle = (beginIndex + endIndex) / 2;
            if (endIndex == beginIndex)
            {
                return Words[beginIndex].KEY == value ? Words[beginIndex] : null;
            }
            else if (endIndex == beginIndex + 1)
            {
                if (Words[beginIndex].KEY == value)
                {
                    return Words[beginIndex];
                }
                else if (Words[endIndex].KEY == value)
                {
                    return Words[endIndex];
                }
                else
                {
                    return null;
                }
            }
            if (Words[middle].KEY == value)
            {
                return Words[middle];
            }
            else if (Words[middle].KEY > value)
            {
                return Find(beginIndex, middle, value);
            }
            else if (Words[middle].KEY < value)
            {
                return Find(middle, endIndex, value);
            }
            return null;
        }
    }
}
