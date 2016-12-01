using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Board.Data
{
    public class Board
    {
        [Key]
        public string BoardId { get; set; }

        public  List<Graduate> Graduates { get; set; }

        public  List<Rank> Ranks { get; set; }

        public List<Post> Posts { get; set; }
    }
}
