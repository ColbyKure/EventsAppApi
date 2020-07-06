using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Description;
using EventsAppApi.Models;

namespace EventsAppApi.Controllers
{
    public class EventsController : ApiController
    {
        
        private SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();

        // GET: api/Events
        //[Authentication.BasicAuthentication]
        [HttpGet]
        //public IQueryable<Event> GetEvents(string Username, string token)
        //public IHttpActionResult GetEvents(string Username, string token)
        public IHttpActionResult GetEvents(string Username, string token)
        {
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            }

            return Ok(db.Events);
        }

        // GET: api/Events/5
        //[Authentication.BasicAuthentication]
        [HttpGet]
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(int id, string Username, string token)
        {
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if(!validationResult)
            {
                return Unauthorized();
            }

            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PutEvent(int id, Event @event, string Username, string token)
        {
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventID)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(@event);
        }

        // POST: api/Events
        [ResponseType(typeof(Event))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PostEvent(Event @event, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @event.EventID }, @event);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult DeleteEvent(int id, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token

            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventID == id) > 0;
        }
    }
}