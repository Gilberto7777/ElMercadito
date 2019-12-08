using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class BusinessType
    {
        public int Id { get; set; }

        [Display(Name = "Business Type")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }


        

        public ICollection<Product> Products { get; set; }
    }

}

