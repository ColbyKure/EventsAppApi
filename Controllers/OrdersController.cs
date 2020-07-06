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
    public class OrdersController : ApiController
    {
        private SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();

        // GET: api/Orders
        //[Authentication.BasicAuthentication]
        [HttpGet]
        public IHttpActionResult GetOrders(string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            return Ok(db.Orders);
        }

        // GET: api/Orders/5
        //[Authentication.BasicAuthentication]
        [HttpGet]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PutOrder(int id, Order order, string Username, string token)
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

            if (id != order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(order);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult PostOrder(Order order, string Username, string token)
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

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        //[Authentication.BasicAuthentication]
        public IHttpActionResult DeleteOrder(int id, string Username, string token)
        {
            // check token
            bool validationResult = TokenManager.ValidateUserToken(Username, token);
            if (!validationResult)
            {
                return Unauthorized();
            } //end check token
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}