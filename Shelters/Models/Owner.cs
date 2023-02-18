using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Description { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
