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
    public class MESSAGESController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/MESSAGES
        public IQueryable<MESSAGES> GetMESSAGES()
        {
            return db.MESSAGES;
        }

        // GET: api/MESSAGES/5
        [ResponseType(typeof(MESSAGES))]
        public async Task<IHttpActionResult> GetMESSAGES(int id)
        {
            MESSAGES mESSAGES = await db.MESSAGES.FindAsync(id);
            if (mESSAGES == null)
            {
                return NotFound();
            }

            return Ok(mESSAGES);
        }

        // PUT: api/MESSAGES/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMESSAGES(int id, MESSAGES mESSAGES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mESSAGES.id)
            {
                return BadRequest();
            }

            db.Entry(mESSAGES).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MESSAGESExists(id))
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

        // POST: api/MESSAGES
        [ResponseType(typeof(MESSAGES))]
        public async Task<IHttpActionResult> PostMESSAGES(MESSAGES mESSAGES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MESSAGES.Add(mESSAGES);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mESSAGES.id }, mESSAGES);
        }

        // DELETE: api/MESSAGES/5
        [ResponseType(typeof(MESSAGES))]
        public async Task<IHttpActionResult> DeleteMESSAGES(int id)
        {
            MESSAGES mESSAGES = await db.MESSAGES.FindAsync(id);
            if (mESSAGES == null)
            {
                return NotFound();
            }

            db.MESSAGES.Remove(mESSAGES);
            await db.SaveChangesAsync();

            return Ok(mESSAGES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MESSAGESExists(int id)
        {
            return db.MESSAGES.Count(e => e.id == id) > 0;
        }
    }
}