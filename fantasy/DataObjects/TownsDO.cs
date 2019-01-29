using fantasy.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace fantasy.DataObjects
{
    public class TownsDO
    {
        public int TownID { get; set; }
        public string Name { get; set; }

        public static async Task<List<TownsDO>> GetTownsAsync()
        {
            using (fantasyDbEntities context =
                new fantasyDbEntities())
            {
                return await context.Town
                    .Select(x => new TownsDO()
                    {
                        TownID = x.idtown,
                        Name = x.name
                    })
                    .ToListAsync();
            }
        }
    }
}