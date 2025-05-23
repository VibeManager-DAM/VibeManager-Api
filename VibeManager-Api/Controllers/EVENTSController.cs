﻿using System;
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
    public class EVENTSController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/events
        [HttpGet]
        [Route("api/events")]
        public async Task<IHttpActionResult> GetAllEvents()
        {
            var baseImageUrl = "http://10.0.3.148/api/Imgs/";
            var events = await db.EVENTS
                .Include(e => e.id_organizer)
                .Select(e => new
                {
                    e.id,
                    e.title,
                    e.description,
                    e.date,
                    e.time,
                    image = baseImageUrl + e.image, 
                    e.capacity,
                    e.seats,
                    e.num_rows,
                    e.num_columns,
                    e.price,
                    Organizer = new
                    {
                        e.id_organizer,
                        e.USERS.fullname,
                    }
                })
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet]
        [Route("api/events/{id}")]
        public async Task<IHttpActionResult> GetEventDetails(int id)
        {
            var ev = await db.EVENTS
                .Include(e => e.id_organizer)
                .Where(e => e.id == id)
                .Select(e => new
                {
                    e.id,
                    e.title,
                    e.description,
                    e.date,
                    e.time,
                    e.image,
                    e.capacity,
                    e.seats,
                    e.num_rows,
                    e.num_columns,
                    e.price,
                    Organizer = new
                    {
                        e.USERS.id,
                        e.USERS.fullname
                    }
                })
                .FirstOrDefaultAsync();

            if (ev == null)
                return NotFound();

            return Ok(ev);
        }

        // GET: api/events/{id}/seats
        [HttpGet]
        [Route("api/events/{id}/seats")]
        public async Task<IHttpActionResult> GetEventSeats(int id)
        {
            var ev = await db.EVENTS.FindAsync(id);
            if (ev == null || !ev.seats)
                return NotFound();

            var takenSeats = await db.TICKETS
                .Where(t => t.id_event == id)
                .Select(t => new { t.num_row, t.num_col })
                .ToListAsync();

            var seats = new List<object>();
            for (int row = 1; row <= ev.num_rows; row++)
            {
                for (int col = 1; col <= ev.num_columns; col++)
                {
                    bool isTaken = takenSeats.Any(s => s.num_row == row && s.num_col == col);
                    seats.Add(new
                    {
                        Row = row,
                        Column = col,
                        IsTaken = isTaken
                    });
                }
            }

            return Ok(seats);
        }

        // GET: api/events/organizer/{id}
        [HttpGet]
        [Route("api/events/organizer/{id}")]
        public async Task<IHttpActionResult> GetAllEvents(int id)
        {
            try
            {
                var events = await (from e in db.EVENTS
                                    join r in db.RESERVES on e.id equals r.id_event
                                    join s in db.SPACES on r.id_space equals s.id
                                    where e.id_organizer == id
                                    select new
                                    {
                                        e.id,
                                        e.title,
                                        e.description,
                                        e.date,
                                        e.time,
                                        e.image,
                                        e.capacity,
                                        e.seats,
                                        e.num_rows,
                                        e.num_columns,
                                        s.name
                                    }).ToListAsync();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al cargar eventos: " + ex.Message));
            }
        }

        // POST: api/events
        [HttpPost]
        [Route("api/events")]
        public async Task<IHttpActionResult> CreateEvent(EventCreateDTO dto)
        {
            var ev = new EVENTS
            {
                title = dto.Title,
                description = dto.Description,
                date = dto.Date,
                time = dto.Time,
                image = dto.Image,
                capacity = dto.Capacity,
                seats = dto.Seats,
                num_rows = dto.NumRows,
                num_columns = dto.NumColumns,
                id_organizer = dto.IdOrganizer,
                price = dto.price
            };

            db.EVENTS.Add(ev);
            await db.SaveChangesAsync();


            var chat = new CHAT
            {
                id_event = ev.id,
                id_user = dto.IdOrganizer
            };

            db.CHAT.Add(chat);
            await db.SaveChangesAsync();

            return Ok(new { ev.id, ev.title, ev.date });
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