using Yiim.AreaFormat.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yiim.AreaFormat
{
    public class Format
    {
        protected WordsUtility Utility;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Format()
        {
            Utility = new WordsUtility();
        }
        /// <summary>
        /// 传入包含地址信息的字符串，返回由系统格式化的字符串
        /// </summary>
        /// <param name="content">包含地址信息的字符串</param>
        /// <returns>返回格式化的字符串（中国 湖北省 荆门市 钟祥市 磷矿镇 (1.25000)）</returns>
        public string Standard(string content)
        {
            return Standard(content, x =>
            {
                if (x == null) return "";
                return string.Format("{0} {1} {2} {3} {4} ({5})",
                       x.Line[0] != null ? x.Line[0].WORD + "(" + x.Line[0].KEY + ")" : "",
                       x.Line[1] != null ? x.Line[1].WORD + "(" + x.Line[1].KEY + ")" : "",
                       x.Line[2] != null ? x.Line[2].WORD + "(" + x.Line[2].KEY + ")" : "",
                       x.Line[3] != null ? x.Line[3].WORD + "(" + x.Line[3].KEY + ")" : "",
                       x.Line[4] != null ? x.Line[4].WORD + "(" + x.Line[4].KEY + ")" : "",
                       x.TotalRank.ToString("0.00000"));
                //return string.Format("{0} {1} {2} {3} {4} ({5})",
                //        x.Line[0] != null ? x.Line[0].WORD : "",
                //        x.Line[1] != null ? x.Line[1].WORD : "",
                //        x.Line[2] != null ? x.Line[2].WORD : "",
                //        x.Line[3] != null ? x.Line[3].WORD : "",
                //        x.Line[4] != null ? x.Line[4].WORD : "",
                //        x.TotalRank.ToString("0.00000"));
            });
        }
        /// <summary>
        /// 传入包含地址信息的字符串，返回自定义类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="content">包含地址信息的字符串</param>
        /// <param name="fun">自定义地址格式化方法</param>
        /// <returns>返回自定义类型</returns>
        public T Standard<T>(string content, Func<Address, T> fun)
        {
            return Standard<T>(content, fun, true);
        }
        /// <summary>
        /// 传入包含地址信息的字符串，返回自定义类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="content">包含地址信息的字符串</param>
        /// <param name="fun">自定义地址格式化方法</param>
        /// <param name="fix">是否修复未包含的上级地区（省，市，国家）</param>
        /// <returns>返回自定义类型</returns>
        public T Standard<T>(string content, Func<Address, T> fun, bool fix)
        {
            return Standard<T>(content, fun, fix, null);
        }
        /// <summary>
        ///  传入包含地址信息的字符串，返回自定义类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="content">包含地址信息的字符串</param>
        /// <param name="fun">自定义地址格式化方法</param>
        /// <param name="fix">是否修复未包含的上级地区（省，市，国家）</param>
        /// <param name="hitCall">自定义处理匹配的所有词汇</param>
        /// <returns>返回自定义类型</returns>
        public T Standard<T>(string content, Func<Address, T> fun, bool fix, Func<List<HitWords>, List<HitWords>> hitCall)
        {
            return Standard<T>(content, fun, fix, hitCall, null);
        }
        /// <summary>
        /// 传入包含地址信息的字符串，返回自定义类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="content">包含地址信息的字符串</param>
        /// <param name="fun">自定义地址格式化方法</param>
        /// <param name="fix">是否修复未包含的上级地区（省，市，国家）</param>
        /// <param name="hitCall">自定义处理匹配的所有词汇</param>
        /// <param name="scoreCall">自定义返回地址线</param>
        /// <returns>返回自定义类型</returns>
        public T Standard<T>(string content, Func<Address, T> fun, bool fix, Func<List<HitWords>, List<HitWords>> hitCall, Func<List<Address>, Address> scoreCall)
        {
            if (string.IsNullOrWhiteSpace(content)) return default(T);
            List<HitWords> hits = HitWords(content);
            hits = (hitCall ?? (x => x))(hits);
            scoreCall = scoreCall ?? (x => x.OrderByDescending(z => z.TotalRank).FirstOrDefault());
            Address ls = GetAddress(hits, scoreCall);
            if (fix) FixAddressLine(ls);
            return fun(ls);
        }
        protected List<HitWords> HitWords(string content)
        {
            int index;
            List<HitWords> hits = new List<HitWords>();
            foreach (WORDS item in WordsUtility.Words)
            {
                if (item.TREE_PATH.Length == 1) continue;
                index = content.IndexOf(item.WORD);
                if (index == -1)
                {
                    if (item.FLAG_KEY <= 0 || item.FLAG_KEY >= 12)
                        continue;
                    FLAG_WORDS flag = Utility.GetExt(item.FLAG_KEY);
                    string alias = item.WORD.TrimEnd(flag.WORD);
                    if (alias.Length <= 1) continue;
                    index = content.IndexOf(alias);
                    if (index == -1) continue;
                    HitWords hit = new HitWords(item);
                    if (hits.Exists(x => x.WORD.Contains(alias)))
                        hit.SCORE = 0.0F;
                    hits.Add(hit);
                }
                else
                {
                    HitWords hit = new HitWords(item);
                    hit.SCORE += 1.0F / item.TREE_PATH.Length;
                    hits.Add(hit);
                }
            }
            return hits;
        }
        protected Address GetAddress(List<HitWords> hits, Func<List<Address>, Address> scoreCall)
        {
            List<Address> line = new List<Address>();
            hits.ForEach(x =>
            {
                bool isPut = line.Exists(l => l.PutLine(x));
                if (!isPut)
                    line.Add(new Address(x));
            });
            return scoreCall(line);
        }
        protected void FixAddressLine(Address address)
        {
            if (address == null) return;
            HitWords word = address.Line.LastOrDefault(t => t != null);
            for (int i = 0; i < word.TREE_PATH.Length - 1; i++)
            {
                if (address.Line[i] == null)
                {
                    WORDS w = Utility.Find(word.TREE_PATH[i + 1]);
                    if (w != null)
                        address.Line[i] = new HitWords(w);
                }
            }
        }
    }
}
