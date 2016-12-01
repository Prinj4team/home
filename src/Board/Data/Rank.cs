using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Data
{
    public class Rank
    {
        [Key]
        public Int32 Key { get; set; }

        public int Position { get; set; }
        [Required]
        public string BoardId { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }
    }
}
