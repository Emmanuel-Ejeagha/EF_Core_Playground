using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace InstallEFCore.Commands;

public class EditRecipeBase
{
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Range(0, 23)]
    public int TimeToCookHrs { get; set; }
    [Range(0, 59)]
    public int TimeToCookMins { get; set; }
    [Required]
    public required string Method { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }
}
