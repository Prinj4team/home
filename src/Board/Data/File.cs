using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Data
{
    public class File
    {
        [Key]
        public Int32 key { get; set; }

        public Int32 postKey { get; set; }

        public string URL { get; set; }
    }
}
