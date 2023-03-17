using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Task4.Areas.Identity.Data;
using Task4.Models;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private string redirectUrl;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(string[] selectedUsers)
        {
            ApplicationUser currentUser = new ApplicationUser();
            var name = HttpContext.User.Identity.Name;
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (string user in selectedUsers)
            {
                users.Add(_context.Users.Where(u => u.UserName == user).FirstOrDefault());
            }
            currentUser = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
            foreach (var user in users)
            {
                if (user != currentUser)
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                    user.Status = "Deleted";
                    await _userManager.UpdateSecurityStampAsync(user);
                    redirectUrl = "~/";
                }
                else
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                    user.Status = "Deleted";
                    await _userManager.UpdateSecurityStampAsync(user);
                    redirectUrl = "~/Identity/Account/Login";
                }
            }
            await _context.SaveChangesAsync();
            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<ActionResult> BlockAsync(string[] selectedUsers)
        {
            ApplicationUser currentUser = new ApplicationUser();
            var name = HttpContext.User.Identity.Name;
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (string user in selectedUsers)
            {
                users.Add(_context.Users.Where(u => u.UserName == user).FirstOrDefault());
            }
            currentUser = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
            foreach (var user in users)
            {
                if (user != currentUser)
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                    user.Status = "Blocked";
                    await _userManager.UpdateSecurityStampAsync(user);
                    redirectUrl = "~/";
                }
                else
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                    user.Status = "Blocked";
                    await _userManager.UpdateSecurityStampAsync(user);
                    redirectUrl = "~/Identity/Account/Login";
                }
            }
            await _context.SaveChangesAsync();
            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<ActionResult> UnblockAsync(string[] selectedUsers)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (string user in selectedUsers)
            {
                users.Add(_context.Users.Where(u => u.UserName == user).FirstOrDefault());
            }
            foreach (var user in users)
            {
                if (!user.Status.Equals("Deleted"))
                {
                    user.LockoutEnd = null;
                    user.Status = "Active";
                }
            }
            await _context.SaveChangesAsync();
            return Redirect("~/");
        }

        public IActionResult Index()
        {
            IList<ApplicationUser> users = _context.Users.ToList();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}