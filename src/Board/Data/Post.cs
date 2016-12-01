using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Data
{
    public class Post
    {
        [Key]
        public Int32 key { get; set; }
        [Required]
        public string BoardId { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool Important { get; set; }

        public string Text { get; set; }
        [Required]
        public string HeaderText { get; set; }

        public List<Tag> Tags {get; set;}
        
    }
}
