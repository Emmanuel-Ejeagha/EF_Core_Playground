using System;

namespace ToDoList;

public class ToDoService
{
    public ICollection<ToDoModel> GetToDoItems(string category, string username)
    {
        return _toDos
            .Where(x => string.Equals(x.State, category, StringComparison.OrdinalIgnoreCase))
            .Where(x => string.Equals(x.Login, username, StringComparison.OrdinalIgnoreCase))
            .ToList();

    }

    /// <summary>This represents our 'database'of stroed values. We are using a C# 7 tuple as the key value</summary>
    private readonly List<ToDoModel> _toDos
        = new List<ToDoModel>
        {
            new ToDoModel
            {
                Number = 103,
                Title = "Change the bedsheet",
                Login = "Emma",
                State = "open"
            },
            new ToDoModel
            {
                Number = 110,
                Title = "Learn AWS Cloud Pratisioner",
                Login = "bikky",
                State = "open"
            },
            new ToDoModel
            {
                Number = 320,
                Title = "The .gitignore is far too long",
                Login = "Andrew",
                State = "open"
            },
             new ToDoModel {
                Number= 97,
                Title= "TL;DR",
                Login= "andrew",
                State="closed",
                },
            new ToDoModel {
                Number= 96,
                Title= "Added analyzers: Sonar.Lint, FxCop, StyleCop, NDepend",
                Login= "james",
                State="open",
                },
            new ToDoModel {
                Number= 95,
                Title= "size of source files",
                Login= "andrew",
                State="open",
                },
            new ToDoModel{
                Number= 94,
                Title= "System.Ben is layered incorrectly",
                Login= "james",
                State="open",  },
            new ToDoModel {
                Number= 93,
                Title= "Fix dotnet-cli errors from `dotnet test`",
                Login= "james",
                State="open",  },
            new ToDoModel {
                Number= 92,
                Title= "Allocate a new Ben is too slow",
                Login= "slodge",
                State="open",
                },

        };
}
