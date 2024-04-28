using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    [Table("tblUsers")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string FistName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [StringLength(150)]
        public string ? Image { get; set; } = string.Empty;
    }
}
