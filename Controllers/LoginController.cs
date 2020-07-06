using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
//using EventsAppApi.Repository;
using EventsAppApi.Models;


namespace LoginService.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Validate(string token, string username)
        {
            //bool exists = new UserRepository().GetUser(username) != null;
            SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();
            int userid = Convert.ToInt32(username);
            bool exists = db.Users.Find(userid) != null;

            if (!exists) return Request.CreateResponse(HttpStatusCode.NotFound,
                 "The user was not found.");
            string tokenUsername = TokenManager.ValidateToken(token);
            if (username.Equals(tokenUsername))
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public HttpResponseMessage Login(User user)
        {
            //User u = new UserRepository().GetUser(user.Username);

            SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();
            //string Userid = "6";
            //int tempID = Convert.ToInt32(Userid);
            //User u = db.Users.Find(tempID);

            User u = db.Users.Find(user.UserId);
            if (u == null)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                     "The user was not found.");
            bool credentials = u.Password.Equals(user.Password);
            if (!credentials)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, 
                    "The username/password combination was wrong.");
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                 TokenManager.GenerateToken(u.UserId.ToString()));
        }
    }
}
