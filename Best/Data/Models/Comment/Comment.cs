using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Comment
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateData { get; set; }
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual BestUser BestUser { get; set; }
    }
}
