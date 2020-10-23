using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPlutus.Domain.Models
{
    [Table("Employee")]
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }

        [Required]
        public double Temperature { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
