using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class SubjectController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public SubjectController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
    }

    // GET: Subject/Index
    public ActionResult Index()
    {
        ViewBag.Subjects = db.Subjects;
        return View();
    }

    // POST: Subject/Index
    [HttpPost]
    public ActionResult Index(SubjectVM vm)
    {
        if (ModelState.IsValid)
        {
            vm.Id = NextId();

            db.Subjects.Add(new()
            {
                Id = vm.Id,
                Name = vm.Name
            });
            db.SaveChanges();

            TempData["Info"] = $"Subject {vm.Id} inserted.";
            return RedirectToAction("Index");
        }

        ViewBag.Subjects = db.Subjects;
        return View(vm);
    }

    // Manually generate next id
    private string NextId()
    {
        string max = db.Subjects.Max(s => s.Id) ?? "S000";
        int n = int.Parse(max[1..]);
        return (n + 1).ToString("'S'000");
    }

    // POST: Subject/Delete
    [HttpPost]
    public ActionResult Delete(string? id)
    {
        var s = db.Subjects.Find(id);

        if (s != null)
        {
            db.Subjects.Remove(s);
            db.SaveChanges();
            TempData["Info"] = $"Subject {s.Id} deleted.";
        }

        return RedirectToAction("Index");
    }
}
