using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fantasy.Models
{
    public class InventoryModel
    {
        public int MaxSpace { get; set; }
        public int FreeSpace { get; set; }
        public int OwnerID { get; set; }
    }
}