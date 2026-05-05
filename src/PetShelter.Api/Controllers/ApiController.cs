using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using PetShelter.Api.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetShelter.Api.Controllers
{
    [ApiController]
    public class ApiController(ILogger<ApiController> logger) : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            logger.LogWarning("API call resulted in errors: {Errors}", string.Join(", ", errors.Select(e => e.Description)));
            if (errors.Count == 0)
                return Problem();
                
            if (errors.All(error => error.Type == ErrorType.Validation))
                return ValidationProblem(errors);

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            return Problem(errors[0]);
        }

        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
