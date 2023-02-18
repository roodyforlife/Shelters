using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public DateTime Birthdate { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Skills { get; set; }
        public List<ShelterVolunteer> ShelterVolunteers { get; set; }
    }
}
