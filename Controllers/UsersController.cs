using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventsAppApi.Models;

namespace EventsAppApi.Controllers
{
    public class UsersController : ApiController
    {
        private SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();

        // GET: api/Users
        //[Authentication.BasicAuthentication]
        [HttpGet]
        public IHttpActionResult GetUsers(string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            return Ok(db.Users);
        }

        // GET: api/Users/5
        //[Authentication.BasicAuthentication]
        [HttpGet]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PutUser(int id, User user, string Username, string token)
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

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PostUser(User user, string Username, string token)
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

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult DeleteUser(int id, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}