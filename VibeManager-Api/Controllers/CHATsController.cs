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
    public class CHATsController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/CHATs
        public IQueryable<CHAT> GetCHAT()
        {
            return db.CHAT;
        }

        // GET: api/CHATs/5
        [ResponseType(typeof(CHAT))]
        public async Task<IHttpActionResult> GetCHAT(int id)
        {
            CHAT cHAT = await db.CHAT.FindAsync(id);
            if (cHAT == null)
            {
                return NotFound();
            }

            return Ok(cHAT);
        }

        // PUT: api/CHATs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCHAT(int id, CHAT cHAT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cHAT.id)
            {
                return BadRequest();
            }

            db.Entry(cHAT).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CHATExists(id))
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

        // POST: api/CHATs
        [ResponseType(typeof(CHAT))]
        public async Task<IHttpActionResult> PostCHAT(CHAT cHAT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CHAT.Add(cHAT);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cHAT.id }, cHAT);
        }

        // DELETE: api/CHATs/5
        [ResponseType(typeof(CHAT))]
        public async Task<IHttpActionResult> DeleteCHAT(int id)
        {
            CHAT cHAT = await db.CHAT.FindAsync(id);
            if (cHAT == null)
            {
                return NotFound();
            }

            db.CHAT.Remove(cHAT);
            await db.SaveChangesAsync();

            return Ok(cHAT);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CHATExists(int id)
        {
            return db.CHAT.Count(e => e.id == id) > 0;
        }
    }
}