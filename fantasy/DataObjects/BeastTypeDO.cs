using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using fantasy.DatabaseModels;

namespace fantasy.DataObjects
{
    public class BeastTypeDO
    {
        public int Idbeasttype { get; set; }
        public string Name { get; set; }

        public static async Task<List<BeastTypeDO>> GetBeastTypeAsync()
        {
            using (fantasyDbEntities context =
                new fantasyDbEntities())
            {
                return await context.BeastType
                    .Select(x => new BeastTypeDO()
                    {
                        Idbeasttype = x.idbeasttype,
                        Name = x.name
                    })
                    .ToListAsync();
            }
        }
    }
}