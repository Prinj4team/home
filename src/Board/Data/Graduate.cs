using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Data
{
    public class Graduate
    {
        [Key]
        public Int32 key { get; set; }
        [Required]
        public string BoardId { get; set; }

        public DateTime FinishDate { get; set; }

        public string Town { get; set; }

        public string Firm { get; set; }

        public string Name { get; set; }

        public string History { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }

    }
}
