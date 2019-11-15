using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ElMercadito.Web.Data.Entities;

namespace ElMercadito.Web.Models
{
    public class ProductImageViewModel : ProductImage
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
