using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PumoxTest.DataBase.Entities
{
    public class Company
    {
        public Company()
        {
            Employees = new List<Employe>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int EstablishmentYear { get; set; }
        [Required]
        public ICollection<Employe> Employees { get; set; }
    }
}
