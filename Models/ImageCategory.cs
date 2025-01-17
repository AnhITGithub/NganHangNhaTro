﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEB_NC.Models
{
    public class ImageCategory
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Url { get; set; }
        [Column(TypeName = "varchar(50)")]
        public Guid HouseId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
