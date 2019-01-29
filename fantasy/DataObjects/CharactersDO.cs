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
    public class CharactersDO
    {
        public int CharID { get; set; }
        public string PlayerName { get; set; }
        public string CharName { get; set; }
        public string CharClass { get; set; }
        public string CharBio { get; set; }
        public int CharHp { get; set; }
        public int CharMp { get; set; }
        public int CharXp { get; set; }
        public int CharLevel { get; set; }
        public int? CharStr { get; set; }
        public int? CharAgi { get; set; }
        public int? CharInt { get; set; }
        public int? CharDurability { get; set; }

        // asynchroní metoda pro načtení objednávek
        public static async Task<List<CharactersDO>> GetCharactersAsync(
               string playerName)
        {
            using (fantasyDbEntities context =
               new fantasyDbEntities())
            {

                // return obstarávající select a join tabulek pro získání dat
                return await context.Character
                    .Where(x => x.playername == playerName)
                    .Select(x => new CharactersDO()
                    {
                        CharID = x.idchar,
                        PlayerName = x.playername,
                        CharName = x.charname,
                        CharClass = x.charclass,
                        CharBio = x.charbio,
                        CharHp = x.charhp,
                        CharXp = x.charxp,
                        CharMp = x.charmp,
                        CharLevel = x.charlevel,
                        CharStr = x.strength,
                        CharAgi = x.agility,
                        CharDurability = x.chardurability,
                        CharInt = x.intelligence,

                    })
                    .ToListAsync();
            }
        }
    }
}
    