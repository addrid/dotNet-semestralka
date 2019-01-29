using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using fantasy.DataObjects;
using fantasy.Models;
using Microsoft.Owin.Security;
using fantasy.Other;
using fantasy.DatabaseModels;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace fantasy.Controllers
{
    public class HomeController : Controller
    {
        // zobrazení stránky Index
        public ActionResult Index()
        {
            return View();
        }

        // zobrazení stránky Other
        public ActionResult Other()
        {
            ViewBag.Message = "Other funcionality.";
            OtherModel model = new OtherModel();

            // nastavení počáteční hodnoty náhodné čísla na 0
            model.RandomNum = 0; 
            return View(model);
        }

        // zaslání HTTP požadavku POST na stránce Other
        [HttpPost]
        public ActionResult Other(OtherModel model, string submitButton)
        {
            // switch zjišťující, jaké z tlačítek bylo zvoleno
            switch (submitButton)
            {
                case "Six":
                    model.RandomNum = Six(); 
                    break;
                case "Twelve":
                    model.RandomNum = Twelve();
                    break;
                default:
                    return (View());
            }
            return View(model);
        }

        // privátní metoda generující šestistěnnou kostku
        private int Six()
        {
            Random rand = new Random();
            int value   = rand.Next(1, 7);

            return (value);
        }

        // privátní metoda generující dvanáctistěnnou kostku
        private int Twelve()
        {
            Random rand = new Random();
            int value   = rand.Next(1, 13);

            return (value);
        }


        // zobrazení stránky Locations
        public async Task<ActionResult> Locations()
        {
            ViewBag.Message = "Locations page.";

            List<TownsDO> towns =
                   await TownsDO.GetTownsAsync();

            List<LocationsDO> locations =
                   await LocationsDO.GetLocationsAsync(towns[0].TownID);

            ViewBag.Town = towns
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.TownID.ToString()
                   })
            .ToList();

            ViewBag.Locations = locations;

            LocationModel model = new LocationModel();
            model.TownID        = towns[0].TownID;

            return View(model);
        }

        // zaslání HTTP požadavku POST na stránce Locations
        [HttpPost]
        public async Task<ActionResult> Locations(LocationModel model)
        {
            ViewBag.Message = "Locations page.";

            List<TownsDO> towns =
                   await TownsDO.GetTownsAsync();

            List<LocationsDO> locations =
                   await LocationsDO.GetLocationsAsync(model.TownID);

            ViewBag.Town = towns
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.TownID.ToString()
                   })
            .ToList();

            ViewBag.Locations = locations;

            return View(model);
        }

        // zobrazení stránky Bestiary
        public async Task<ActionResult> Bestiary()
        {
            ViewBag.Message = "Bestiary page.";

            List<BeastTypeDO> beastType =
                   await BeastTypeDO.GetBeastTypeAsync();

            List<BeastsDO> bestiary =
                   await BeastsDO.GetBeastsAsync(beastType[0].Idbeasttype);

            ViewBag.beastType = beastType
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.Idbeasttype.ToString()
                   })
            .ToList();

            ViewBag.Bestiary = bestiary;

            BeastModel model = new BeastModel();
            model.BeastTypeID = beastType[0].Idbeasttype;

            return View(model);
        }


        // zaslání HTTP požadavku POST na stránce Bestiary
        [HttpPost]
        public async Task<ActionResult> Bestiary(BeastModel model)
        {
            List<BeastTypeDO> beastType =
                   await BeastTypeDO.GetBeastTypeAsync();

            List<BeastsDO> bestiary =
                   await BeastsDO.GetBeastsAsync(model.BeastTypeID);

            ViewBag.beastType = beastType
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.Idbeasttype.ToString()
                   })
            .ToList();

            ViewBag.Bestiary = bestiary;

            return View(model);
        }

        // zkopírovaná metoda pro přihlášení
        [AllowAnonymous]
        public ActionResult LogOn(string returnAddress)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnAddress))
                {
                    return Redirect(returnAddress);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ReturnAddress = returnAddress;
                return View();
            }
        }

        // zkopírovaná metoda pro přihlášení - požadavek HTTP POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOn(
            LogOnModel model,
            string returnAddress)
        {
            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager =
                    new UserManager<IdentityUser>(new UserStore());

                var user =
                    await userManager.FindAsync(model.UserName, model.Password);

                if (user != null)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(
                        DefaultAuthenticationTypes.ExternalCookie);

                    var identity = await userManager.CreateIdentityAsync(
                        user,
                        DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties()
                        {
                            IsPersistent = model.RememberMe
                        },
                        identity);

                    if (Url.IsLocalUrl(returnAddress))
                    {
                        return Redirect(returnAddress);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Invalid user name or password.");
                }
            }

            return View(model);
        }

        // zkopírovaná metoda pro odhlášení
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index");
        }


        // zobrazení stránky NewCharacter
        public ActionResult NewCharacter()
        {
            ViewBag.Message = "New character page.";

            return View();
        }

        // zaslaný HTTP požadavek POST na stránce NewCharacter - vytvoření postavy
        [HttpPost]
        public ActionResult NewCharacter(CharModel model)
        {
            if (ModelState.IsValid)
            {
                using (fantasyDbEntities context = new fantasyDbEntities())
                {
                    // vytvoření nové entity postavy
                    Character player = new Character();
                    player.charname = model.CharName;
                    player.charbio = model.CharBio;

                    // hardcoded starting values
                    player.charhp = 20;
                    player.charlevel = 1;
                    player.charmp = 10;
                    player.charxp = 0;              
                    player.strength = 5;
                    player.agility = 5;
                    player.chardurability = 5;
                    player.intelligence = 5;

                    // pokud uživatel nevybrat žádnou profesi
                    if (model.CharClass == null)
                    {
                        // nastaví profesi za uživatele
                        player.charclass = "Warrior";

                    }
                    else
                    {
                        player.charclass = model.CharClass;
                    }

                    player.playername = User.Identity.GetUserName();
                    // přidání a uložení nové postavy
                    context.Character.Add(player);
                    context.SaveChanges();
                
                    // automatické založení inventáře pro každou novou postavu
                    Inventory inventory = new Inventory();
                    inventory.maxspace = 10;
                    inventory.freespace = 10;
                    inventory.ownerid = player.idchar;
                    context.Inventory.Add(inventory);
                    context.SaveChanges();
                }
            }
            else
            {
                return RedirectToAction("Characters");
            }
            
            return RedirectToAction("Characters");
        }
        
        // zobrazení stránky Characters
        public async Task<ActionResult> Characters(LogOnModel model)
        {
            List<CharactersDO> characters =
                   await CharactersDO.GetCharactersAsync(User.Identity.GetUserName());

            ViewBag.Characters = characters;

            return View(model);
        }

        // zobrazení stránky EditCharacter - zasílající zvolené ID hráče
        public ActionResult EditCharacter(int id)
        {
            using (fantasyDbEntities context = new fantasyDbEntities())
            {
                var character = context.Character.Where(x => x.idchar == id).FirstOrDefault();

                // vytvoření instance CharModelu a načtení všech aktuálních hodnot
                CharModel model = new CharModel();
                model.CharID = id;
                model.CharName = character.charname;
                model.CharBio = character.charbio;
                model.CharClass = character.charclass;
                model.CharLevel = character.charlevel;
                model.CharHp = character.charhp;
                model.CharMp = character.charmp;
                model.CharStr = (int) character.strength;
                model.CharDurability = (int) character.chardurability;

                return View(model);
            }
        }

        // HTTP Post požadavek na stránce EditCharacter - potvrzení úpravy
        [HttpPost]
        public ActionResult EditCharacter(CharModel model, int id)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    using (fantasyDbEntities context = new fantasyDbEntities())
                    {
                        Character character = new Character();
                        character = context.Character.Find(id);

                        // kontrola, zda string hodnoty nejsou null
                        if (model.CharName != null)
                        {
                            character.charname = model.CharName;
                        }
                        if (model.CharBio != null)
                        {
                            character.charbio = model.CharBio;
                        }
                        if (model.CharClass != null)
                        {
                            character.charclass = model.CharClass;
                        }
                        character.charlevel = model.CharLevel;
                        character.charhp = model.CharHp;
                        character.charmp = model.CharMp;
                        character.strength = model.CharStr;
                        character.chardurability = model.CharDurability;
                        character.idchar = id;
                        try
                        {
                            // uložení všech změněných hodnot
                            context.Entry(character).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var error = ex.ToString();
                            return View();
                        }
                    }

                    return RedirectToAction("Characters");
                }
                catch (Exception exception)
                {
                    var error = exception.ToString();
                    return View();
                }
            }
            return View();
        }

        // zobrazení stránky EditBeast
        public ActionResult EditBeast(int id)
        {
            using (fantasyDbEntities context = new fantasyDbEntities())
            {
                var beast = context.Bestiary.Where(x => x.idbeast == id).FirstOrDefault();

                // vytvoření instance BeastModelu a načtení aktuálních hodnot
                BeastModel model = new BeastModel();
                model.BeastID = id;
                model.BeastName = beast.name;
                model.BeastBio = beast.bio;
                model.BeastTypeID = beast.beasttypeid;

                return View(model);
            }
        }

        // zaslaný HTTP Post požadavek na úpravu bestie
        [HttpPost]
        public ActionResult EditBeast(BeastModel model, int id)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    using (fantasyDbEntities context = new fantasyDbEntities())
                    {
                        Bestiary beast = new Bestiary();
                        beast = context.Bestiary.Find(id);
                        beast.idbeast = id;
                        beast.beasttypeid = model.BeastTypeID;
                        
                        // kontrola stringových hodnot - nezměněné hodnoty jsou NULL
                        if(model.BeastName != null)
                        {
                            beast.name = model.BeastName;
                        }
                        if(model.BeastBio != null)
                        {
                            beast.bio = model.BeastBio;
                        }
                        try
                        {
                            context.Entry(beast).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var error = ex.ToString();
                            return View();
                        }
                    }

                    return RedirectToAction("Bestiary");
                }
                catch (Exception exception)
                {
                    var error = exception.ToString();
                    return View();
                }
            }
            return View();
        }
        
        // zobrazení stránky Inventory - využívá zaslané ID hráče
        public ActionResult Inventory(int id)
        {
            using (fantasyDbEntities context = new fantasyDbEntities())
            {
                var inventory = context.Inventory.Where(x => x.ownerid == id).FirstOrDefault();

                // vytvoření InventoryModelu a načtení aktuálních hodnot
                InventoryModel model = new InventoryModel();
                model.OwnerID   = inventory.ownerid;
                model.MaxSpace  = inventory.maxspace;
                model.FreeSpace = inventory.freespace;

                return View(model);
            }
        }

        // zaslaný HTTP Post požadavek o změnu inventáře
        [HttpPost]
        public ActionResult Inventory(InventoryModel model, string submitButton)
        {
            ModelState.Clear();

            // switch kontrolující, jaké z tlačítek bylo stlačeno
            switch (submitButton)
            {
                case "AddItem": // hráč se rozhodl přidat předmět
                    if (model.FreeSpace > 1)
                        model.FreeSpace -= 1;
                    break;
                case "RemoveItem": // hráč se rozhodl odebrat předmět
                    if (model.FreeSpace < model.MaxSpace)
                        model.FreeSpace += 1;
                    break;
                case "IncreaseSize": // hráč se rozhodl rozšířit inventář
                        model.MaxSpace  += 1;
                        model.FreeSpace += 1;
                    break;
                case "DecreaseSize": // hráč se rozhodl zmenšit inventář
                    if (model.MaxSpace > 1 && model.FreeSpace > 1)
                    {
                        model.MaxSpace  -= 1;
                        model.FreeSpace -= 1;
                    }
                    break;
                default:
                    // vrácení v případě, že byl zaslán POST požadavek bez kliknutí na tlačítka
                    return (View());
            }

            if (ModelState.IsValid)
            { 
                try
                {
                    using (fantasyDbEntities context = new fantasyDbEntities())
                    {
                        // Vytvoření entity inventory a načtení hodnot z modelu
                        Inventory inventory = new Inventory();
                        inventory = context.Inventory.Find(model.OwnerID);
                        inventory.freespace = model.FreeSpace;
                        inventory.maxspace  = model.MaxSpace;
                        try
                        {
                            // uložení změn do db
                            context.Entry(inventory).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var error = ex.ToString();
                            return View();
                        }
                    }
                    return View(model);
                }
                catch (Exception exception)
                {
                    var error = exception.ToString();
                    return View();
                }
            }
            return View();
        }
        
        // zobrazení stránky NewBeast
        public async Task<ActionResult> NewBeast()
        {
            List<BeastTypeDO> beastType =
                   await BeastTypeDO.GetBeastTypeAsync();

            // hardcoded - allowing to add beasts only outside of towns
            List<LocationsDO> locations =
                   await LocationsDO.GetLocationsAsync(1);

            ViewBag.beastType = beastType
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.Idbeasttype.ToString()
                   })
            .ToList();

            ViewBag.Location = locations
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.LocationID.ToString()
                   })
            .ToList();

            BeastModel model = new BeastModel();
            return View(model);
        }

        // zaslaný HTTP Post požadavek na vytvoření nové bestie
        [HttpPost]
        public async Task<ActionResult> NewBeast(BeastModel model)
        {
            List<BeastTypeDO> beastType =
                   await BeastTypeDO.GetBeastTypeAsync();

            // hardcoded - povoluje přidat bestie pouze mimo města
            List<LocationsDO> locations =
                   await LocationsDO.GetLocationsAsync(0);

            ViewBag.beastType = beastType
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.Idbeasttype.ToString()
                   })
            .ToList();

            ViewBag.Location = locations
                   .Select(x => new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.LocationID.ToString()
                   })
            .ToList();

            if(ModelState.IsValid)
            {
                if (model.BeastName != null && model.BeastBio != null)
                {
                    using (fantasyDbEntities context = new fantasyDbEntities())
                    {
                        // Vytvoření nové bestie a načtení hodnot zvolených uživatelem
                        Bestiary beast    = new Bestiary();
                        beast.name        = model.BeastName;
                        beast.bio         = model.BeastBio;
                        beast.hp          = model.BeastHp;
                        beast.attack      = model.BeastAttack;
                        beast.defense     = model.BeastDefense;
                        beast.locationid  = model.BeastLocation;
                        beast.beasttypeid = model.BeastTypeID;
                        // Přidání řádku tabulky a uložení změn
                        context.Bestiary.Add(beast);
                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Bestiary");
        }

        // zobrazení stránky Map
        public ActionResult Maps()
        {
            ViewBag.Message = "Maps page.";

            return View();
        }

    }
}