using FluentValidation.Results;

namespace OrderAndCargo.Application.Exceptions
{
    public class ApiValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ApiValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
