using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mentalHealth.Models;
using System.Threading.Tasks;
using mentalHealth.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System;

namespace mentalHealth.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db) => _db = db;

        // Public Pages
        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
        public IActionResult Login() => View();
        public IActionResult Register() => View();

        // Therapists List with Filtering
        public async Task<IActionResult> Therapists(string filter)
        {
            var query = _db.Therapists.AsQueryable();

            if (filter == "preferences" && User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userId, out int id))  // Convert userId to int
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
                    if (user != null) query = ApplyPreferenceFilters(query, user);
                }
            }

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> TherapistDetails(int id)
        {
            var therapist = await _db.Therapists.FindAsync(id);
            return therapist != null ? View(therapist) : NotFound();
        }

        // Profile and Preferences (temporary testing versions)
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int id))  // Convert userId to int
            {
                var user = await _db.Users.FindAsync(id);
                return user != null ? View(user) : NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult> Preferences()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int id))  // Convert userId to int
            {
                var user = await _db.Users.FindAsync(id);
                return user != null ? View(user) : NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Preferences(User model)
        {
            TempData["Success"] = "Preferences updated (simulated)!";
            return RedirectToAction(nameof(Preferences));
        }

        // Book a session
        [Authorize]
        public IActionResult BookSession(int therapistId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (int.TryParse(userId, out int clientId))  // Convert userId to int (ClientId type)
            {
                var model = new Session
                {
                    TherapistId = therapistId,
                    ClientId = clientId,  // Use clientId as int
                    StartTime = DateTime.Now.AddDays(1).Date.AddHours(10),
                    Duration = TimeSpan.FromHours(1),
                    Status = SessionStatus.Booked
                };

                return View(model);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookSession(Session model)
        {
            if (!ModelState.IsValid) return View(model);

            switch (model.Type)
            {
                case SessionType.VideoCall:
                    model.MeetingLink = $"https://meet.example.com/{Guid.NewGuid()}";
                    break;
                case SessionType.Chat:
                    model.ChatAccessToken = Guid.NewGuid().ToString();
                    break;
                case SessionType.InPerson:
                    model.Location = "123 Therapy St, Mental Health City";
                    break;
            }

            model.Status = SessionStatus.Booked;
            _db.Sessions.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Session booked successfully!";
            return RedirectToAction(nameof(Sessions));
        }

        // Show all sessions for logged-in client
        [Authorize]
        public async Task<IActionResult> Sessions()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(clientId, out int parsedClientId))  // Convert clientId to int
            {
                var sessions = await _db.Sessions
                    .Include(s => s.Therapist)
                    .Include(s => s.Client)
                    .Where(s => s.ClientId == parsedClientId)  // Use parsedClientId as int
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();

                return View(sessions);
            }
            return Unauthorized();
        }

        // Cancel session
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelSession(int id)
        {
            var session = await _db.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            session.Status = SessionStatus.Cancelled;
            await _db.SaveChangesAsync();

            TempData["Message"] = "Session cancelled successfully";
            return RedirectToAction(nameof(Sessions));
        }

        // Progress Tracking (public testing)
        public IActionResult TrackProgress()
        {
            var model = new ProgressTracking
            {
                RecordedDate = DateTime.Now,
                UserId = 1,
                Mood = MoodRating.Neutral
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TrackProgress(ProgressTracking model)
        {
            TempData["ProgressSuccess"] = $"SIMULATED: Mood recorded as {model.Mood}";
            return RedirectToAction(nameof(TrackProgress));
        }

        // Helper method for filtering therapists based on preferences
        private IQueryable<Therapist> ApplyPreferenceFilters(IQueryable<Therapist> query, User user)
        {
            if (user.PreferredTherapistGender.HasValue)
                query = query.Where(t => t.Gender.ToString() == user.PreferredTherapistGender.ToString());

            query = user.SessionPreference switch
            {
                SessionPreference.Online => query.Where(t => t.ProvidesOnlineSessions),
                SessionPreference.Offline => query.Where(t => t.ProvidesOfflineSessions),
                _ => query
            };

            if (user.TimePreference != TimePreference.Any)
                query = query.Where(t => t.AvailableTimes.Contains(user.TimePreference.ToString()));

            return query;
        }

        // Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
