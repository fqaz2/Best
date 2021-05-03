using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class Img
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
    }
}
