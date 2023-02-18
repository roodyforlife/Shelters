using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public Shelter Shelter { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int ShelterId { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Items { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
