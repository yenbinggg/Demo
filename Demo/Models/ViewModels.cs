using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

// View Models ----------------------------------------------------------------

#nullable disable warnings

public class TeacherVM
{
    public string? Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }
}

public class SubjectVM
{
    public string? Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }
}
