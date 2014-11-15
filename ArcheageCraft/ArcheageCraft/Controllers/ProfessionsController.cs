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
    public class ProfessionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Professions
        [ActionName("")]
        public IQueryable<Profession> GetProfessions()
        {
            return db.Professions.OrderBy(p=>p.Name);
        }

        // GET: api/Professions/5
        [ResponseType(typeof(Profession))]
        [ActionName("")]
        public async Task<IHttpActionResult> GetProfession(int id)
        {
            Profession profession = await db.Professions.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }

            return Ok(profession);
        }

        // PUT: api/Professions/5
        [ResponseType(typeof(void))]
        [ActionName("")]
        public async Task<IHttpActionResult> PutProfession(int id, Profession profession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profession.ProfessionId)
            {
                return BadRequest();
            }

            db.Entry(profession).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessionExists(id))
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

        // POST: api/Professions
        [ResponseType(typeof(Profession))]
        [ActionName("")]
        public async Task<IHttpActionResult> PostProfession(Profession profession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Professions.Add(profession);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profession.ProfessionId }, profession);
        }

        // DELETE: api/Professions/5
        [ResponseType(typeof(Profession))]
        [ActionName("")]
        public async Task<IHttpActionResult> DeleteProfession(int id)
        {
            Profession profession = await db.Professions.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }

            db.Professions.Remove(profession);
            await db.SaveChangesAsync();

            return Ok(profession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfessionExists(int id)
        {
            return db.Professions.Count(e => e.ProfessionId == id) > 0;
        }
    }
}