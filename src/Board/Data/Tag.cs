using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Data
{
    public class Tag
    {
        [Key]
        public Int32 key { get; set; }

        public string TagName { get; set; }
        
        public string BoardId { get; set; }
    }
}
