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
    public class USERSController : ApiController
    {
        private VibeEntities db = new VibeEntities();
        

        // GET: api/users
        [HttpGet]
        [Route("api/users")]
        public IQueryable<object> GetUsers()
        {
            db.Configuration.LazyLoadingEnabled = false;

            return db.USERS.Select(u => new
            {
                u.id,
                u.fullname,
                u.email,
                rol = u.ROL.name
            });
        }

        // GET: api/users/{id}
        [HttpGet]
        [Route("api/users/{id}")]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var user = await db.USERS
                .Where(u => u.id == id)
                .Select(u => new
                {
                    u.id,
                    u.fullname,
                    u.email,
                    rol = u.ROL.name
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/users/{id}/events
        [HttpGet]
        [Route("api/users/{id}/events")]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetUserWithEvents(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var userWithEvents = await db.USERS
                .Where(u => u.id == id)
                .Select(u => new
                {
                    u.id,
                    u.fullname,
                    u.email,
                    rol = u.ROL.name,
                    events = u.EVENTS.Select(e => new
                    {
                        e.id,
                        e.title,
                        e.date,
                        e.time,
                        e.capacity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (userWithEvents == null)
                return NotFound();

            return Ok(userWithEvents);
        }

        // GET: api/users/{id}/tickets
        [HttpGet]
        [Route("api/users/{id}/tickets")]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetUserTickets(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var baseImageUrl = "http://10.0.3.148/api/Imgs/";

            var userTickets = await db.USERS
                .Where(u => u.id == id)
                .Select(u => new
                {
                    u.id,
                    u.fullname,
                    u.email,
                    tickets = u.TICKETS.Select(t => new
                    {
                        t.id,
                        t.date,
                        t.time,
                        t.num_row,
                        t.num_col,
                        eventInfo = new
                        {
                            t.EVENTS.id,
                            t.EVENTS.title,
                            t.EVENTS.date,
                            t.EVENTS.time,
                            t.EVENTS.capacity,
                            image = baseImageUrl + t.EVENTS.image,
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (userTickets == null)
                return NotFound();

            return Ok(userTickets);
        }


        // GET: api/users/{id}/chats
        [HttpGet]
        [Route("api/users/{id}/chats")]
        public async Task<IHttpActionResult> GetUserChats(int id)
        {
            var baseImageUrl = "http://10.0.3.148/api/Imgs/";
            var chats = await db.CHAT
                .Where(c => c.id_user == id)
                .Select(c => new
                {
                    ChatId = c.id,
                    EventId = c.EVENTS.id,
                    EventTitle = c.EVENTS.title,
                    image = baseImageUrl + c.EVENTS.image,
                })
                .ToListAsync();

            if (!chats.Any())
            {
                return NotFound();
            }
                

            return Ok(chats);
        }




        // POST: api/users/login
        [HttpPost]
        [Route("api/users/login")]
        public async Task<IHttpActionResult> Login([FromUri] string email, [FromUri] string password)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var user = await db.USERS
                .Where(u => u.email == email && u.password == password)
                .Select(u => new
                {
                    u.id,
                    u.fullname,
                    u.email,
                    u.password,
                    u.id_rol
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        // POST: api/users/register
        [HttpPost]
        [Route("api/users/register")]
        public async Task<IHttpActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await db.USERS.AnyAsync(u => u.email == dto.email))
                return BadRequest("El email ya está registrado.");

            var newUser = new USERS
            {
                fullname = dto.fullname,
                email = dto.email,
                password = dto.password, 
                id_rol = dto.id_rol
            };

            db.USERS.Add(newUser);
            await db.SaveChangesAsync();

            return Ok(new
            {
                newUser.id,
                newUser.fullname,
                newUser.email
            });
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