using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fantasy.Other
{
    public class BeastTypes
    {
        // pevně daný select list s předurčenými hodnotami
        public List<SelectListItem> types = new List<SelectListItem>()
        {
            new SelectListItem() {Text="Lizard", Value="1"},
            new SelectListItem() {Text="Bird", Value="2"},
            new SelectListItem() {Text="Critter", Value="3"},
            new SelectListItem() {Text="Mammal", Value="4"},
            new SelectListItem() {Text="Magical", Value="5"}
        };

        // get metoda pro vrácení listu s typy bestií.
        public List<SelectListItem> GetList()
        {
            return types;
        }
    }
}