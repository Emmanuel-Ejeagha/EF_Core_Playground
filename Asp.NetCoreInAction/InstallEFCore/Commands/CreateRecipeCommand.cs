using System;
using InstallEFCore.Models;

namespace InstallEFCore.Commands;

public class CreateRecipeCommand : EditRecipeBase
{
    public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

    public Recipe ToRecipe()
    {
        return new Recipe
        {
            Name = Name,
            TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
            Method = Method,
            IsVegetarian = IsVegetarian,
            IsVegan = IsVegan,
            Ingredients = Ingredients.Select(x => x.ToIngredient()).ToList()
        };
    }
}
