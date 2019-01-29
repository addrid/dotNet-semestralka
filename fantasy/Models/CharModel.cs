using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fantasy.Models
{
    public class CharModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int CharID { get; set; }
        public string CharName { get; set; }
        public string CharClass { get; set; }
        public string CharBio { get; set; }
        public int CharLevel { get; set; }
        public int CharHp { get; set; }
        public int CharMp { get; set; }
        public int CharStr { get; set; }
        public int CharAgi { get; set; }
        public int CharDurability { get; set; }
        public int CharInt { get; set; }
        public string PlayerName { get; set; }
    }
}