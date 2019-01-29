using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using fantasy.DatabaseModels;

namespace fantasy.DataObjects
{
    public class LocationsDO
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string TownName { get; set; }

        public static async Task<List<LocationsDO>> GetLocationsAsync(int townid)
        {
            using (fantasyDbEntities context =
               new fantasyDbEntities())
            {
                return await context.Location
                   .Where(location => location.townid == townid)
                   .Join(context.Town,
                                    location => location.townid, // cizí klíč z tab objednávek
                                   town => town.idtown, // prim. klíč z tab zaměstnanců
                         (location, town) => new LocationsDO()
                         {
                             LocationID = location.idlocation,
                             Name = location.name,
                             TownName = town.name,
                         })
                         .ToListAsync();
            }
        }
    }
}