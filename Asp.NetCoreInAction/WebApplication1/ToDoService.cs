using System;

namespace Company.ClassLibrary1;

public record ToDoListModel(string Category, string Title);
public class ToDoService
{
    private static readonly List<ToDoListModel> _items = new List<ToDoListModel>
    {
        new ToDoListModel("Simple", "Bread"),
        new ToDoListModel("Simple", "Milk"),
        new ToDoListModel("Simple", "Get Gas"),
        new ToDoListModel("Long", "Write Book"),
        new ToDoListModel("Long", "Build Application"),
    };

    public List<ToDoListModel> GetItemsForCategory(string category)
    {
        return _items.Where(x => x.Category == category).ToList();
    }
}
