using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class ShelterVolunteer
    {
        public int Id { get; set; }
        public Shelter Shelter { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int ShelterId { get; set; }
        public Volunteer Volunteer { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int VolunteerId { get; set; }
    }
}
