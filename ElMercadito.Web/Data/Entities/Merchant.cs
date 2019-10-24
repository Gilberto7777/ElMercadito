using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class Merchant
    {
        public int Id { get; set; }

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "Cell Phone")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string CellPhone { get; set; }

        [Display(Name = "Township and colony, (other information by security by cell phone).")]
        [MaxLength(30, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Address { get; set; }

        [Display(Name = "Merchant Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Merchant Name")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

       // public ICollection<Product> Products { get; set; }

        //public ICollection<Offer> Offers { get; set; }
    }

}

