using AddressNormalizer.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class Program
    {
        string str = "server=.;database=shifenzheng;user id=sa;password=87189100;";
        static void Main(string[] args)
        {
            new Program().WriteDictionary();
        }
        public void WriteDictionary()
        {
            using (Stream stream = new FileStream("words.dic", FileMode.OpenOrCreate))
            {
                List<Region> ls = GetAreas();
                StringBuilder builder = new StringBuilder();
                List<FLAG_WORDS> order = WordsUtility.FlagWords.OrderByDescending(x => x.WORD.Length).ToList();
                foreach (Region region in ls)
                {
                    string[] ids = region.TreePath.Split(',');
                    int[] id = new int[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        id[i] = int.Parse(ids[i]);
                    }
                    WORDS w = new WORDS()
                    {
                        KEY = region.Id,
                        WORD = region.Name,
                        TREE_PATH = id,
                    };
                    w.RANK = 0.1F;
                    FLAG_WORDS fw = order.FirstOrDefault(z => region.Name.EndsWith(z.WORD));
                    w.RANK = w.RANK / ls.Sum(x => x.Name.Contains(w.WORD) ? 1 : 0);
                    if (fw != null)
                    {
                        w.FLAG_KEY = fw.KEY;
                        w.RANK += w.RANK / ls.Sum(x => x.Name.Contains(w.WORD.TrimEnd(fw.WORD)) ? 1 : 0);
                    }
                    w.RANK += 1.0F / w.TREE_PATH.Length;
                    stream.Write(BitConverter.GetBytes(w.KEY), 0, 4);
                    byte[] bw = Encoding.Unicode.GetBytes(w.WORD);
                    stream.Write(BitConverter.GetBytes(bw.Length), 0, 4);
                    stream.Write(bw, 0, bw.Length);
                    stream.Write(BitConverter.GetBytes(w.TREE_PATH.Length), 0, 4);
                    foreach (int n in w.TREE_PATH)
                        stream.Write(BitConverter.GetBytes(n), 0, 4);
                    stream.Write(BitConverter.GetBytes(w.RANK), 0, 4);
                    stream.Write(BitConverter.GetBytes(w.FLAG_KEY), 0, 4);
                    string line = string.Format("{0}:【{1}】 （{2}） 【{3}】", w.KEY, w.WORD, w.RANK, region.TreePath);
                    builder.AppendLine(line);
                    Console.WriteLine(line);
                }
                File.AppendAllText("result.txt", builder.ToString());
            }
        }
        public List<Region> GetAreas()
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                return conn.Query<Region>("SELECT * FROM [shifenzheng].[dbo].[VehicleRegion]").ToList();
            }
        }
    }
}
