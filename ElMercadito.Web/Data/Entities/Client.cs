﻿using System.ComponentModel.DataAnnotations;

namespace ElMercadito.Web.Data.Entities
{
    public class Client
    {

        public int Id { get; set; }

        public User User { get; set; }
    }

}