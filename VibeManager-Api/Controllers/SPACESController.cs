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

        // GET: api/spaces
        [HttpGet]
        [Route("api/spaces")]
        public async Task<IHttpActionResult> GetAllSpaces()
        {
            var spaces = await db.SPACES
                .Select(s => new
                {
                    s.id,
                    s.name,
                    s.square_meters,
                    s.capacity,
                    s.address,
                    s.latitude,
                    s.longitude
                })
                .ToListAsync();

            return Ok(spaces);
        }

        // GET: api/spaces/{id}
        [HttpGet]
        [Route("api/spaces/{id}")]
        public async Task<IHttpActionResult> GetSpaceById(int id)
        {
            var space = await db.SPACES
                .Where(s => s.id == id)
                .Select(s => new
                {
                    s.id,
                    s.name,
                    s.square_meters,
                    s.capacity,
                    s.address,
                    s.latitude,
                    s.longitude
                })
                .FirstOrDefaultAsync();

            if (space == null)
                return NotFound();

            return Ok(space);
        }

        // GET: api/spaces/{id}/items
        [HttpGet]
        [Route("api/spaces/{id}/items")]
        public async Task<IHttpActionResult> GetItemsBySpaceId(int id)
        {
            var spaceExists = await db.SPACES.AnyAsync(s => s.id == id);
            if (!spaceExists)
                return NotFound();

            var items = await db.ITEMS
                .Where(i => i.id_space == id)
                .Select(i => new
                {
                    i.id,
                    i.name_item,
                    i.amount
                })
                .ToListAsync();

            return Ok(items);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}