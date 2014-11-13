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
    public class CraftsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Crafts
        public IQueryable<Craft> GetCrafts()
        {
            return db.Crafts;
        }

        // GET: api/Crafts/5
        [ResponseType(typeof(Craft))]
        [ActionName("")]
        public async Task<IHttpActionResult> GetCraft(int id)
        {
            Craft craft = await db.Crafts.FindAsync(id);
            if (craft == null)
            {
                return NotFound();
            }

            return Ok(craft);
        }

        public async Task<IEnumerable<IngredientViewModel>> Ingredients(int id)
        {
            return (await db.CraftItems.Where(ci => ci.CraftId == id).ToListAsync()).Select(ci =>new IngredientViewModel(ci));
        }

        // PUT: api/Crafts/5
        [ResponseType(typeof(void))]
        [ActionName("")]
        public async Task<IHttpActionResult> PutCraft(int id, Craft craft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != craft.CraftId)
            {
                return BadRequest();
            }

            db.Entry(craft).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CraftExists(id))
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

        // POST: api/Crafts
        [ResponseType(typeof(Craft))]
        public async Task<IHttpActionResult> PostCraft(Craft craft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Crafts.Add(craft);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = craft.CraftId }, craft);
        }

        // DELETE: api/Crafts/5
        [ResponseType(typeof(Craft))]
        [ActionName("")]
        public async Task<IHttpActionResult> DeleteCraft(int id)
        {
            Craft craft = await db.Crafts.FindAsync(id);
            if (craft == null)
            {
                return NotFound();
            }

            db.Crafts.Remove(craft);
            await db.SaveChangesAsync();

            return Ok(craft);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CraftExists(int id)
        {
            return db.Crafts.Count(e => e.CraftId == id) > 0;
        }
    }
}