﻿

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEB_NC.Models
{
    public class House
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? HouseTitle { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? Address { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string? OfLocationId { get; set; }
        public int Acreage { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Price { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Desciption { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? HouseStatus { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? HouseType { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? OwnerName { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string? OwnerPhone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
