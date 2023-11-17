using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Omnitool.Models
{
    public class Crypto
    {
        [Key]
        public int CryptotId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public float Price { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        
    }
     public class Finance
    {
        [Key]
        public int FinancetId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public float Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        
    }
     public class Currency
    {
        [Key]
        public int CurrencytId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public float Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        
    }

    public class Chat
    {
        [Key]
        public int Chatid {get; set; }

        [Required]
        public string? Writer { get; set; }

        [Required]
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

    }

    public class MyTask
    {
        [Key]
        public int taskId {get;set;}

        [Required]
        public string? tasks { get; set; }

        [Required]
        public string? createdForDate { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }


    }
}