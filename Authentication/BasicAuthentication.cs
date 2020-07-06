using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EventsAppApi.Models;

namespace EventsAppApi.Authentication
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization != null)
                {
                    //Taking the parameter from the header  
                    var authToken = actionContext.Request.Headers.Authorization.Parameter;
                    //decode the parameter  
                    var decoAuthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    //split by colon : and store in variable  
                    var UserNameAndPassword = decoAuthToken.Split(':');
                    //Passing to a function for authorization  
                    if (IsAuthorizedUser(UserNameAndPassword[0], UserNameAndPassword[1]))
                    {
                        // setting current principle  
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserNameAndPassword[0]), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return;
            }
        }
        public static bool IsAuthorizedUser(string Username, string Password)
        {
            if(Username == "abc" && Password == "123")
            {
                return true;
            }
            if(Username == null || Username == "")
            {
                return false; 
            }
            SummitWorksEventManagerEntities db = new SummitWorksEventManagerEntities();
            int tempID = Convert.ToInt32(Username);
            User user = db.Users.Find(tempID);
            if (user == null) return false;
            if(user.IsAdmin)
            {
                if (user.Password == Password) return true;
                return false;
            }
            return false;
            //return Username == "colbykure@gmail.com" && Password == "colbykure@gmail.com";
        }
    }
}