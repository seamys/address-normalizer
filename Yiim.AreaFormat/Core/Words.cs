using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiim.AreaFormat.Core
{
    public class WORDS
    {
        /// <summary>
        /// 标示Key
        /// </summary>
        public int KEY { get; set; }
        /// <summary>
        /// 地区字符
        /// </summary>
        public string WORD { get; set; }
        /// <summary>
        /// 位置地址
        /// </summary>
        public int[] TREE_PATH { get; set; }
        /// <summary>
        /// 权值
        /// </summary>
        public float RANK { get; set; }
        /// <summary>
        /// 标示KEY
        /// </summary>
        public int FLAG_KEY { get; set; }
    }

    public class FLAG_WORDS
    {
        /// <summary>
        /// 标示KEY
        /// </summary>
        public int KEY { get; set; }
        /// <summary>
        /// 应用级别
        /// </summary>
        public int[] LEVEL { get; set; }
        /// <summary>
        /// 字符
        /// </summary>
        public string WORD { get; set; }
    }
}
