using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fantasy.Other
{
    public class Professions
    {
        // pevně daný select list s předurčenými hodnotami pro všechny povolání postav
        public List<SelectListItem> HeroClasses = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Warrior", Value="Warrior"},
            new SelectListItem() {Text="Mage", Value="Mage"},
            new SelectListItem() {Text="Paladin", Value="Paladin"},
            new SelectListItem() {Text="Cleric", Value="Cleric"},
            new SelectListItem() {Text="Bard", Value="Bard"}
        };

        // vrácení listu s povoláním hráčů
        public List<SelectListItem> GetList()
        {
            return HeroClasses;
        }
    }
    
}