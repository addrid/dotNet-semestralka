using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fantasy.Models
{
    public class BeastModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int BeastID { get; set; }
        public string BeastName { get; set; }
        public string BeastBio { get; set; }
        public int BeastHp { get; set; }
        public int BeastAttack { get; set; }
        public int BeastDefense { get; set; }
        public int BeastLocation { get; set; }
        public int BeastTypeID { get; set; }
    }
}