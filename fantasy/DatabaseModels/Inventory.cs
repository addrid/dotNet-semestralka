//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace fantasy.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory
    {
        public int idinventory { get; set; }
        public int maxspace { get; set; }
        public int freespace { get; set; }
        public int ownerid { get; set; }
    
        public virtual Character Character { get; set; }
    }
}
