using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Models
{
    public class Donation
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string DonorName { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string DonorSurname { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public string Comment { get; set; }
        public Shelter Shelter { get; set; }
        [Required(ErrorMessage = "Field should be not emtpy")]
        public int ShelterId { get; set; }
    }
}
