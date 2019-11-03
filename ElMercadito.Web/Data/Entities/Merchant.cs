using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class Merchant
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Offer> Offers { get; set; }
    }

}

