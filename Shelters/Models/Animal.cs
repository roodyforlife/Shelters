using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class Animal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Species { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Description { get; set; }
        [ForeignKey("ShelterId")]
        public Shelter Shelter { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int ShelterId { get; set; }
        [ForeignKey("OwnerId")]
        public Owner Owner { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? OwnerId { get; set; }
    }
}
