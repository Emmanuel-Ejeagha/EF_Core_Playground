using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

var app = builder.Build();

app.MapPost("/users", (CreateUserModel user) => user.ToString()).WithParameterValidation();

app.MapGet("/user/{id}", (GetUserModel model) => model.Id.ToString()).WithParameterValidation();

app.MapGet("/", () => "Hello World!");

app.Run();


public record CreateUserModel : IValidatableObject
{
    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    // [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(PhoneNumber))
        {
            yield return new ValidationResult("You must provide either an email or a phone number", new[] { nameof(Email), nameof(PhoneNumber) });
        }
    }
}

struct GetUserModel
{
    [Range(1, 10)]
    public int Id { get; set; }
}