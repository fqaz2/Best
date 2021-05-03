using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class PostImg : Img
    {
        public virtual Post Post { get; set; }
    }
}
