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
    public class SPACESController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/SPACES
        public IQueryable<SPACES> GetSPACES()
        {
            return db.SPACES;
        }

        // GET: api/SPACES/5
        [ResponseType(typeof(SPACES))]
        public async Task<IHttpActionResult> GetSPACES(int id)
        {
            SPACES sPACES = await db.SPACES.FindAsync(id);
            if (sPACES == null)
            {
                return NotFound();
            }

            return Ok(sPACES);
        }

        // PUT: api/SPACES/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSPACES(int id, SPACES sPACES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sPACES.id)
            {
                return BadRequest();
            }

            db.Entry(sPACES).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SPACESExists(id))
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

        // POST: api/SPACES
        [ResponseType(typeof(SPACES))]
        public async Task<IHttpActionResult> PostSPACES(SPACES sPACES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SPACES.Add(sPACES);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sPACES.id }, sPACES);
        }

        // DELETE: api/SPACES/5
        [ResponseType(typeof(SPACES))]
        public async Task<IHttpActionResult> DeleteSPACES(int id)
        {
            SPACES sPACES = await db.SPACES.FindAsync(id);
            if (sPACES == null)
            {
                return NotFound();
            }

            db.SPACES.Remove(sPACES);
            await db.SaveChangesAsync();

            return Ok(sPACES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SPACESExists(int id)
        {
            return db.SPACES.Count(e => e.id == id) > 0;
        }
    }
}