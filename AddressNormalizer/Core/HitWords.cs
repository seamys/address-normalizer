using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressNormalizer.Core
{
    public class HitWords
    {
        public HitWords(WORDS word)
        {
            this.KEY = word.KEY;
            int len = word.TREE_PATH.Length;
            this.TREE_PATH = new int[len + 1];
            Array.Copy(word.TREE_PATH, this.TREE_PATH, word.TREE_PATH.Length);
            this.TREE_PATH[len] = word.KEY;
            this.WORD = word.WORD;
            this.SCORE = word.RANK;
        }
        public int KEY { get; set; }
        public int[] TREE_PATH { get; set; }
        public float SCORE { get; set; }
        public string WORD { get; set; }
    }
}
