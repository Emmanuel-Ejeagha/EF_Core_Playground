using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ChangingConvention;

public class PrefixingPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        var selectors = model.Selectors
            .Select(x => new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = AttributeRouteModel.CombineTemplates("page", x.AttributeRouteModel!.Template),
                }
            })
            .ToList();

        foreach(var newSelector in selectors)
        {
            model.Selectors.Add(newSelector);
        }
    }
}
