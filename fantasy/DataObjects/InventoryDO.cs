using fantasy.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace fantasy.DataObjects
{
    public class InventoryDO
    {
        public int MaxSpace { get; set; }
        public int FreeSpace { get; set; }

        public static async Task<List<InventoryDO>> GetInventoryAsync(int charid)
        {
            using (fantasyDbEntities context =
                new fantasyDbEntities())
            {
                return await context.Inventory
                    .Where(inventory => inventory.ownerid == charid)
                    .Select(x => new InventoryDO()
                    {
                        MaxSpace = x.maxspace,
                        FreeSpace = x.freespace
                    })
                    .ToListAsync();
            }
        }
    }
}