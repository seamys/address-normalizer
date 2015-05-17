using Yiim.AreaFormat;
using Yiim.AreaFormat.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiim.TestAreaFormat
{
    class Program
    {
        protected string str = "server=.;database=shifenzheng;user id=sa;password=87189100;";
        protected Format format = new Format();
        static void Main(string[] args)
        {
            new Program().TestWord("兴安县兴安镇城区青龙新村二巷3号252号");
        }
        public void TestWord(string word)
        {
            string outs = format.Standard(word);
            Console.Write(word + ":");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(outs);
            Console.ResetColor();
            Console.ReadKey();
        }
        public void Test(string file)
        {
            string[] line = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Files/" + file);
            foreach (string item in line)
            {
                string outs = format.Standard(item, x => Insert(x, item));
                Console.Write(item + ":");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(outs);
                Console.ResetColor();
            }
        }
        public string Insert(Address x, string source)
        {
            if (x == null) return "";
            string SQL = @"INSERT INTO [dbo].[Result](
                   [Source],[Level1],[Level2],[Level3],[Level4],[Level5],[Score],[LastKey])
                   VALUES (@Source,@Level1,@Level2,@Level3,@Level4,@Level5,@Score,@LastKey)";
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                conn.Execute(SQL, new
                {
                    Source = source,
                    Level1 = x.Line[0] == null ? "" : x.Line[0].WORD,
                    Level2 = x.Line[1] == null ? "" : x.Line[1].WORD,
                    Level3 = x.Line[2] == null ? "" : x.Line[2].WORD,
                    Level4 = x.Line[3] == null ? "" : x.Line[3].WORD,
                    Level5 = x.Line[4] == null ? "" : x.Line[4].WORD,
                    Score = x.TotalRank,
                    LastKey = x.Line.Last(z => z != null).KEY
                });
            }
            return string.Format("{0} {1} {2} {3} {4} ({5})",
                   x.Line[0] != null ? x.Line[0].WORD + "(" + x.Line[0].KEY + ")" : "",
                   x.Line[1] != null ? x.Line[1].WORD + "(" + x.Line[1].KEY + ")" : "",
                   x.Line[2] != null ? x.Line[2].WORD + "(" + x.Line[2].KEY + ")" : "",
                   x.Line[3] != null ? x.Line[3].WORD + "(" + x.Line[3].KEY + ")" : "",
                   x.Line[4] != null ? x.Line[4].WORD + "(" + x.Line[4].KEY + ")" : "",
                   x.TotalRank.ToString("0.00000"));
        }
    }
}
