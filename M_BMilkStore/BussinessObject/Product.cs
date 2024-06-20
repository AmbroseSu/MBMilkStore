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
        public int ProductId { get; set; }
        [StringLength(30, MinimumLength =5, ErrorMessage ="{0} must be length between {2} and {1} characters" )]
        public string Name { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = "{0} must be length between {2} and {1} characters")]
        public string Description { get; set; }
        [Range (10000, 100000000, ErrorMessage = "{0} Wrong, must be between {1} and {2}")]
        public double Price { get; set; }
        public string Image { get; set; }
        public bool Status {  get; set; }
        public bool IsDelete {  get; set; }
        public int ProductBrandId { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual ICollection<OrderDetail> ?ListOrderDetail { get; set; }
        public virtual ICollection<ProductLine> ?ListProductLine { get; set; }
        public virtual ProductBrand? ProductBrand { get; set; }
        public virtual ProductCategory? ProductCategory { get; set; }

    }
}
