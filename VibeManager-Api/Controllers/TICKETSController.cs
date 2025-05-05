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
using VibeManager_Api.DTO;
using VibeManager_Api.Models;

namespace VibeManager_Api.Controllers
{
    public class TICKETSController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/TICKETS/{id}
        [HttpGet]
        [Route("api/tickets/{id}")]
        public async Task<IHttpActionResult> GetTicket(int id)
        {
            var ticket = await db.TICKETS
                .Include(t => t.EVENTS)
                .Where(t => t.id_event == id)
                .Select(t => new
                {
                    t.id,
                    t.date,
                    t.time,
                    t.num_row,
                    t.num_col,
                    Event = new
                    {
                        t.EVENTS.id,
                        t.EVENTS.title
                    }
                })
                .FirstOrDefaultAsync();

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }


        // POST: api/tickets
        [HttpPost]
        [Route("api/tickets")]
        public async Task<IHttpActionResult> ReserveTicket(TicketCreateDTO dto)
        {
            var ticket = new TICKETS
            {
                date = dto.Date,
                time = dto.Time,
                num_col = dto.NumCol,
                num_row = dto.NumRow,
                id_event = dto.IdEvent,
                id_user = dto.IdUser
            };

            db.TICKETS.Add(ticket);

            var exists = await db.CHAT.AnyAsync(c => c.id_event == dto.IdEvent && c.id_user == dto.IdUser);
            if (!exists)
            {
                var chat = new CHAT
                {
                    id_event = dto.IdEvent,
                    id_user = dto.IdUser
                };
                db.CHAT.Add(chat);
            }

            await db.SaveChangesAsync();

            return Ok(new { ticket.id, ticket.id_event, ticket.id_user });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TICKETSExists(int id)
        {
            return db.TICKETS.Count(e => e.id == id) > 0;
        }
    }
}