using System;
// ASP.NET CORE Libraries
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
// Project Models
using beltexam2.Models;
// LINQ library
using System.Linq;
using System.Collections.Generic;

namespace beltexam2.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        // Attach the Database Context to the class
        private ProfessionalContext _context;
        public ProfileController(ProfessionalContext context)
        {
            // This attaches the Quote Database Context to the controller
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // Run the index dynamic method
            return index();
        }
        // GET: /Users/
        [HttpGet]
        [Route("users")]
        public IActionResult Users()
        {
            // Get the current user details
            User user = fetchuser();
            // Make sure the user is logged in
            if (user == null)
            {
                return user_login();
            }
            // Run the show_users dynamic method
            ICollection<User> users = _context.PopulateUsersAllNotAssociated(user.Id);
            return View(users);
        }
        // GET: invite/{userid}/add
        [HttpGet]
        [Route("invite/{userid}/add")]
        public IActionResult Add(int userid)
        {
            // Get the current user details
            User user = fetchuser();
            // Get the invited user
            User invited_user = _context.PopulateUserSingle(userid);
            // Make sure the user is logged in
            if (user == null)
            {
                return user_login();
            }
            // Navbar variables
            ViewBag.email = user.Email;
            // Create a new network for the invitee and add the properties
            Network invited_network = new Network();
            invited_network.NetworkUserId = user.Id;
            invited_network.NetworkRelatedUserId = userid;
            // Create a new network for the inviter and add the properties
            Network inviter_network = new Network();
            inviter_network.NetworkUserId = userid;
            inviter_network.NetworkRelatedUserId = user.Id;
            // Handle the network list changes
            user.Networks.Add(invited_network);
            invited_user.Networks.Add(inviter_network);
            // Get the needed records for the remove action
            Invitation invitee = user.Invitations.FirstOrDefault(x => x.InvitationUserId == user.Id && x.InvitationRelatedUserId == userid);
            user.Invitations.Remove(invitee);
            // Save the table changes
            _context.SaveChanges();
            // Render the View with the User model
            return RedirectToAction("Index");
        }
        // GET: invite/{userid}/ignore
        [HttpGet]
        [Route("invite/{userid}/ignore")]
        public IActionResult Ignore(int userid)
        {
            // Get the current user details
            User user = fetchuser();
            // Make sure the user is logged in
            if (user == null)
            {
                return user_login();
            }
            // Navbar variables
            ViewBag.email = user.Email;
            // Get the needed records for the actions
            Invitation invitee = user.Invitations.FirstOrDefault(x => x.InvitationUserId == user.Id && x.InvitationRelatedUserId == userid);
            // Handle the list changes
            user.Invitations.Remove(invitee);
            // Save the table changes
            _context.SaveChanges();
            // Render the View with the User model
            return RedirectToAction("Index");
        }
        // GET: invite/{userid}/send
        [HttpGet]
        [Route("invite/{userid}/send")]
        public IActionResult Send(int userid)
        {
            // Get the current user details
            User currentuser = fetchuser();
            // Make sure the user is logged in
            if (currentuser == null)
            {
                return user_login();
            }
            // Navbar variables
            ViewBag.email = currentuser.Email;
            // Get the user we are going to invite
            User user = _context.PopulateUserSingle(userid);
            // Create the new invitation and assign the properties
            Invitation invitee = new Invitation();
            invitee.InvitationUserId = user.Id;
            invitee.InvitationRelatedUserId = currentuser.Id;
            // Handle the list changes
            user.Invitations.Add(invitee);
            // Save the table changes
            _context.SaveChanges();
            // Render the View with the User model
            return RedirectToAction("Users");
        }
        /*  This is the user login check method
            Takes in a view name and a model for prepping the view returned */
        public dynamic user_login()
        {
            // Pull the controller and action out of the HTTP Context
            var currentcontroller = RoutingHttpContextExtensions.GetRouteData(this.HttpContext).Values["controller"];
            var currentaction = RoutingHttpContextExtensions.GetRouteData(this.HttpContext).Values["action"];
            var currentid = RoutingHttpContextExtensions.GetRouteData(this.HttpContext).Values["id"];
            // Redirect to the login page with the returnURL passed as parameters
            return RedirectToAction("Login", "User", new { ReturnNamedRoute = currentaction, ReturnController = currentcontroller, ReturnID = currentid });
        }
        // Fetch the user object
        public User fetchuser()
        {
            // Get the user id from the Session if it exists
            int? user_id = HttpContext.Session.GetInt32("user");
            // Return the user or null
            if (user_id != null)
            {
                return _context.PopulateUserSingle((int)user_id);
            }
            else
            {
                return null;
            }
        }
        // This handles the index logic
        private dynamic index()
        {
            User user = fetchuser();
            if (user == null)
            {
                Console.WriteLine("Doing user login");
                // Check to make sure user is logged in
                return user_login();
            }
            else
            //if (user != null)
            {
                // Navbar ViewBag
                ViewBag.user_name = user.name();
                //ViewBag.user_wallet = user.Money;
                ViewBag.userid = user.Id;
            }
            // General Viewbag settings
            ViewBag.dashboard = true;
            // Handle the expired auctions before we View them (Candidate for async)
            //_context.ProcessExpiredAuctions();
            Console.WriteLine("This is the user");
            Console.WriteLine(user);
            Console.WriteLine(user.Description);
            Console.WriteLine(user.Networks);
            foreach (var networkuser in user.Networks)
            {
                Console.WriteLine("This is the network User");
                Console.WriteLine(networkuser);
            }
            // Return the view of index with list of auctions attached
            return View(user);
        }
    }
}
