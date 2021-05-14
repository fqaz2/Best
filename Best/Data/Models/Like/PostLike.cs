using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Like
{
    public class PostLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [ForeignKey("Post")]
        public string PostId { get; set; }
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual Post Post { get; set; }
        public virtual BestUser BestUser { get; set; }
    }
}
