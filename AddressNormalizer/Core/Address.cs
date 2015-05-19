using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddressNormalizer.Core
{
    public class Address
    {
        public Address(HitWords word)
        {
            Line = new HitWords[5];
            Put(word);
        }
        protected bool Put(HitWords word)
        {
            Line[word.TREE_PATH.Length - 2] = word;
            TotalRank = Line.Sum(x => x != null ? x.SCORE : 0);
            return true;
        }
        public double TotalRank
        {
            get;
            private set;
        }
        public HitWords[] Line { get; private set; }
        public bool PutLine(HitWords word)
        {
            HitWords each = Line.LastOrDefault(x => x != null);
            int eLength = each.TREE_PATH.Length;
            int wLength = word.TREE_PATH.Length;
            //同级别无匹配
            if (eLength == wLength) return false;
            int i = 0;
            while (i < eLength && i < wLength)
            {
                if (each.TREE_PATH[i] != word.TREE_PATH[i])
                    return false;
                i++;
            }
            return Put(word);
        }
    }
}
