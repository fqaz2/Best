using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Comment
{
    public class PostComment : Comment
    {
        [ForeignKey("Post")]
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
