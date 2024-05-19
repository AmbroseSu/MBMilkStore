using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual ICollection<OrderDetail> ListOrderDetail { get; set; }
        public virtual ProductBrand? ProductBrand { get; set; }
        public virtual ProductCategory? ProductCategory { get; set; }

    }
}
