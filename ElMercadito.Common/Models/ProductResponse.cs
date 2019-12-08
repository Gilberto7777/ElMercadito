using System;
using System.Collections.Generic;
using System.Text;

namespace ElMercadito.Common.Models
{
    public class ProductResponse
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string IsAvailable { get; set; }

        public string Remarks { get; set; }

        public string BusinessType { get; set; }

        public ICollection<ProductImageResponse> ProductImages { get; set; }

       

    }
}
