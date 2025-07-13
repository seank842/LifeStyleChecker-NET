using LifestyleChecker.ApiService.Attrbutes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace LifestyleChecker.ApiService.Conventions
{
    public class PatientAuthoriseProducesConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    if (action.Filters.OfType<PatientAuthoriseAttribute>().Any())
                    {
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                    }
                }
            }
        }
    }
}
