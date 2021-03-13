using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApp
{

    [Table("Books")]
    public class Book
    {
        public int BookId { get; set; }
        [MaxLength(120)]
        [Required]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Publisher { get; set; }
    }
}
