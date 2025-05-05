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
    public class ITEMSController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/ITEMS
        public IQueryable<ITEMS> GetITEMS()
        {
            return db.ITEMS;
        }

        // GET: api/ITEMS/5
        [ResponseType(typeof(ITEMS))]
        public async Task<IHttpActionResult> GetITEMS(int id)
        {
            ITEMS iTEMS = await db.ITEMS.FindAsync(id);
            if (iTEMS == null)
            {
                return NotFound();
            }

            return Ok(iTEMS);
        }

        // PUT: api/ITEMS/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutITEMS(int id, ITEMS iTEMS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != iTEMS.id)
            {
                return BadRequest();
            }

            db.Entry(iTEMS).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ITEMSExists(id))
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

        // POST: api/ITEMS
        [ResponseType(typeof(ITEMS))]
        public async Task<IHttpActionResult> PostITEMS(ITEMS iTEMS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ITEMS.Add(iTEMS);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = iTEMS.id }, iTEMS);
        }

        // DELETE: api/ITEMS/5
        [ResponseType(typeof(ITEMS))]
        public async Task<IHttpActionResult> DeleteITEMS(int id)
        {
            ITEMS iTEMS = await db.ITEMS.FindAsync(id);
            if (iTEMS == null)
            {
                return NotFound();
            }

            db.ITEMS.Remove(iTEMS);
            await db.SaveChangesAsync();

            return Ok(iTEMS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ITEMSExists(int id)
        {
            return db.ITEMS.Count(e => e.id == id) > 0;
        }
    }
}