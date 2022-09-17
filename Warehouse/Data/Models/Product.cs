﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{
    public class Product : BaseEntity
    {

        [Required]
        public string Name { get; set; }
        public float buyPrice { get; set; }
        public float sellPrice { get; set; }

        [ForeignKey("Category")]
        public int category_Id { get; set; }

        public Category Category { get; set; }
        public List<Transaction> Transactions { get; set; }




    }
}
