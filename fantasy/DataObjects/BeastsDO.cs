using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using fantasy.Models;
using fantasy.DatabaseModels;

namespace fantasy.DataObjects
{
    public class BeastsDO
    {
        public int Idbeast { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string BeastTypeName { get; set; }

        // asynchroní metoda pro načtení objednávek
        public static async Task<List<BeastsDO>> GetBeastsAsync(
               int idbeasttype)
        {
            using (fantasyDbEntities context =
               new fantasyDbEntities())
            {

                // return obstarávající select a join tabulek pro získání dat
                return await context.Bestiary
                    .Where(bestiary => bestiary.beasttypeid == idbeasttype)
                    .Join(context.BeastType,
                                     bestiary => bestiary.beasttypeid, // cizí klíč z tab objednávek
                                    beastType => beastType.idbeasttype, // prim. klíč z tab zaměstnanců
                          (bestiary, beastType) => new BeastsDO()
                          {
                              Idbeast = bestiary.idbeast,
                              Name = bestiary.name,
                              Attack = bestiary.attack,
                              Defense = bestiary.defense,
                              BeastTypeName = beastType.name,
                              Bio = bestiary.bio,
                          })
                         .ToListAsync();
            }
        }
    }
}