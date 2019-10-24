using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class BusinessType
    {
        public int Id { get; set; }

        [Display(Name = "Property Type")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }


        [Display(Name = " Description Business")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string DescriptionBusiness { get; set; }

        [Display(Name = " Home Service")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string HomeService { get; set; }

        [Display(Name = " Ambulante or in local and location ")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string WalkingOrLocal { get; set; }

        [Display(Name = " Working Hours ")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string WorkingHours { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}

