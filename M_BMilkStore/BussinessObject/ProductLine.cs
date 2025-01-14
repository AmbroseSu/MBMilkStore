﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class ProductLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ProductLineId { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime ExpiredDate { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

    }
}