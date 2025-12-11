using System;
using System.ComponentModel.DataAnnotations;
using InstallEFCore.Models;

namespace InstallEFCore.Commands;

public class CreateIngredientCommand
{
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Range(0, int.MaxValue)]
    public decimal Quantity { get; set; }
    [Required, StringLength(100)]
    public required string Unit { get; set; }

    public Ingredient ToIngredient() => new Ingredient
    {
        Name = Name,
        Quantity = Quantity,
        Unit = Unit
    };
}
