using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ArcheageCraft.Models;

namespace ArcheageCraft.Controllers
{
    public class ItemsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Items
        [ActionName("")]
        public async Task<IEnumerable<ItemViewModel>> GetItems()
        {
            return (await db.Items.OrderBy(i=>i.Name).ToListAsync()).Select(i => new ItemViewModel(i));
        }

        // GET: api/Items/5
        [ResponseType(typeof(Item))]
        [ActionName("")]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        [ActionName("")]
        public async Task<IHttpActionResult> PutItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ItemId)
            {
                return BadRequest();
            }

            var conflicts = await db.Items.AnyAsync(i => i.Name == item.Name && i.ItemId != item.ItemId);
            if (conflicts)
            {
                return BadRequest(string.Format("Item of name {0} already exist.", item.Name));
            }
            

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Items
        [ResponseType(typeof(Item))]
        [ActionName("")]
        public async Task<IHttpActionResult> PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingItem = await db.Items.FirstOrDefaultAsync(i => i.Name == item.Name);
            if(existingItem != null)
            {
                return BadRequest(string.Format("Item of name {0} already exist.",item.Name));
            }
            db.Items.Add(item);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = item.ItemId }, item);
        }

        // DELETE: api/Items/5
        [ResponseType(typeof(Item))]
        [ActionName("")]
        public async Task<IHttpActionResult> DeleteItem(int id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        [HttpGet]
        [ActionName("recipes")]
        public async Task<IEnumerable<RecipeViewModel>> Recipes(int id)
        {
            return (await db.Crafts.Include("CraftItems").Where(c => c.ItemId == id).ToListAsync()).Select(c => new RecipeViewModel(c)).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.ItemId == id) > 0;
        }
    }
    public class ItemViewModel
    {


        public ItemViewModel(Item i)
        {
            // TODO: Complete member initialization
            this.ItemId = i.ItemId;
            this.Name = i.Name;
        }
        public int ItemId { get; set; }
        public string Name { get; set; }
    }
    public class RecipeViewModel
    {
        public RecipeViewModel(Craft c)
        {
            Id = c.CraftId;
            LaborCost = c.LaborCost;
            ProfessionId = c.ProfessionId;
            Production = c.Production;
            Ingredients = c.CraftItems.Select(ci => new IngredientViewModel(ci)).ToList();
        }
        public int Id { get; set; }


        public int LaborCost { get; set; }

        public int ProfessionId { get; set; }

        public int Production { get; set; }

        public List<IngredientViewModel> Ingredients { get; set; }
    }
    public class IngredientViewModel
    {
        public IngredientViewModel() { }

        public IngredientViewModel(CraftItem ci)
        {
            ItemId = ci.ItemId;
            Name = ci.Item.Name;
            Count = ci.Count;
        }
        public int ItemId { get; set; }
        public string Name { get; set; }

        public int Count { get; set; }

    }

}