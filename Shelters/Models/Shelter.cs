using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class Shelter
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string City { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Website { get; set; }
        public List<ShelterVolunteer> ShelterVolunteers { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<Animal> Animals { get; set; } = new List<Animal>();
    }
}
