using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Campaing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Bonuses { get; set; }
        public string BestUserId { get; set; }//Не верный код
        public virtual Topic Topic { get; set; }
    }
}
