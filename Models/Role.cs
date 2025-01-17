﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEB_NC.Models
{
    public class Role
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string? Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
