using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers;

public class TeacherController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public TeacherController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
    }

    // GET: Teacher/Index
    public IActionResult Index()
    {
        ViewBag.Teachers = db.Teachers.Include(t => t.Subjects);
        return View();
    }

    // POST: Teacher/Index
    [HttpPost]
    public IActionResult Index(TeacherVM vm)
    {
        if (ModelState.IsValid)
        {
            vm.Id = NextId();

            db.Teachers.Add(new()
            {
                Id = vm.Id,
                Name = vm.Name,
            });
            db.SaveChanges();
            
            TempData["Info"] = $"Teacher {vm.Id} inserted.";
            return RedirectToAction("Index");
        }

        ViewBag.Teachers = db.Teachers.Include(t => t.Subjects);
        return View();
    }

    // Manually generate next id
    private string NextId()
    {
        string max = db.Teachers.Max(t => t.Id) ?? "T000";
        int n = int.Parse(max[1..]);
        return (n + 1).ToString("'T'000");
    }

    // POST: Teacher/Delete
    [HttpPost]
    public IActionResult Delete(string? id)
    {
        var t = db.Teachers.Find(id);

        if (t != null)
        {
            db.Teachers.Remove(t);
            db.SaveChanges();
            TempData["Info"] = $"Teacher {t.Id} deleted.";
        }

        return RedirectToAction("Index");
    }

    // GET: Teacher/Assign
    public IActionResult Assign(string? id)
    {
        var t = db.Teachers
                  .Include(t => t.Subjects)
                  .FirstOrDefault(t => t.Id == id);

        if (t == null)
        {
            return RedirectToAction("Index");
        }

        var selected = t.Subjects.Select(s => s.Id);
        ViewBag.SubjectList = new MultiSelectList(db.Subjects, "Id", "Name", selected);
        
        return View(t);
    }

    // POST: Teacher/Assign
    [HttpPost]
    public IActionResult Assign(string? id, string[] subjects)
    {
        var t = db.Teachers
                  .Include(t => t.Subjects)
                  .FirstOrDefault(t => t.Id == id);

        if (t == null)
        {
            return RedirectToAction("Index");
        }

        t.Subjects = db.Subjects.Where(s => subjects.Contains(s.Id)).ToList();
        db.SaveChanges();

        TempData["Info"] = "Subject(s) assigned.";
        return RedirectToAction("Assign");
    }
}
