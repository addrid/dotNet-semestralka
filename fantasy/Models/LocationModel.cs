using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fantasy.Models
{
    public class LocationModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int LocationID { get; set; }
        public string Name { get; set; }
        public int TownID { get; set; }
    }
}