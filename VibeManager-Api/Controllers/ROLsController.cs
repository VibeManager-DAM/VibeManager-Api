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
using VibeManager_Api.Models;

namespace VibeManager_Api.Controllers
{
    public class ROLsController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/ROLs
        public IQueryable<ROL> GetROL()
        {
            return db.ROL;
        }

        // GET: api/ROLs/5
        [ResponseType(typeof(ROL))]
        public async Task<IHttpActionResult> GetROL(int id)
        {
            ROL rOL = await db.ROL.FindAsync(id);
            if (rOL == null)
            {
                return NotFound();
            }

            return Ok(rOL);
        }

        // PUT: api/ROLs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutROL(int id, ROL rOL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rOL.id)
            {
                return BadRequest();
            }

            db.Entry(rOL).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ROLExists(id))
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

        // POST: api/ROLs
        [ResponseType(typeof(ROL))]
        public async Task<IHttpActionResult> PostROL(ROL rOL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ROL.Add(rOL);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rOL.id }, rOL);
        }

        // DELETE: api/ROLs/5
        [ResponseType(typeof(ROL))]
        public async Task<IHttpActionResult> DeleteROL(int id)
        {
            ROL rOL = await db.ROL.FindAsync(id);
            if (rOL == null)
            {
                return NotFound();
            }

            db.ROL.Remove(rOL);
            await db.SaveChangesAsync();

            return Ok(rOL);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ROLExists(int id)
        {
            return db.ROL.Count(e => e.id == id) > 0;
        }
    }
}