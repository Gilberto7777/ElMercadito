using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Data.Entities
{
    public class Offer
    {
        public int Id { get; set; }

        [Display(Name = "Offer Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal OfferName { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }

        public string Remarks { get; set; }

        public Merchant Merchant { get; set; }

        public Client Client { get; set; }

        public Product Product { get; set; }

    }
}
