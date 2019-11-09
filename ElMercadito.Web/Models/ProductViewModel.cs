using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElMercadito.Web.Data.Entities;

namespace ElMercadito.Web.Models
{
    public class ProductViewModel : Product
    {
        public int MerchantId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Business Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a property type.")]
        public int BusinessTypeId { get; set; }

        public IEnumerable<SelectListItem> BusinessTypes { get; set; }

    }
}
