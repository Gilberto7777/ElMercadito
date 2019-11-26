using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElMercadito.Web.Data.Entities;

namespace ElMercadito.Web.Models
{
    public class OfferViewModel : Offer
    {
        public int MerchantId { get; set; }

        public int ProductId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Offer")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Clients.")]
        public int OfferId { get; set; }


        [Display(Name = "Offer Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public string OfferName { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available?")]
        public string IsAvailable { get; set; }

        public string Remarks { get; set; }
    }
}
