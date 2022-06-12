using MiniValidation;
using System.ComponentModel.DataAnnotations;

var users = new List<User>();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/user", (int skip, int take) =>
{
    return Results.Ok(users.OrderBy(user => user.Email).Skip(skip).Take(take));

}).WithName("GetUsers").Produces(200);

app.MapPost("/user", (User user) =>
{
    if (!MiniValidator.TryValidate(user, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    users.Add(user);

    return Results.Created("/user", user);

}).WithName("AddUser").ProducesValidationProblem(400).Produces(201);


app.Run();


public record User
{
    [Required, MaxLength(64)]
    public string? FirstName { get; set; }

    [Required, MaxLength(64)]
    public string? LastName { get; set; }

    [Required, DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}


// The reason why you need this partial class definition, is that by default the Program.cs
// file is compiled into a private class Program, which can not be accessed by other projects.
// By adding this public partial class, the test project will get access to Program
// and lets you write tests against it.
public partial class Program { }