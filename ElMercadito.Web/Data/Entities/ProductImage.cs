using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public Product Product { get; set; }


        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl) 
            ? null 
            : $"https://TBD.azurewebsites.net{ImageUrl.Substring(1)}";

        
    }

}

