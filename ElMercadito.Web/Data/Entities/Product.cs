using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available?")]
        public string IsAvailable { get; set; }

        public string Remarks { get; set; }

        public BusinessType BusinessType { get; set; }

        public Merchant Merchant { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

        public ICollection<Offer> Offers { get; set; }

    }
}
