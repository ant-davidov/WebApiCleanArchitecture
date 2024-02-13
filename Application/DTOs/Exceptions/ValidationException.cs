using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; } = new();
        public ValidationException(ValidationResult validationResult)
        {
            Errors = validationResult.Errors
                        .Select(er => er.ErrorMessage)
                        .ToList();
        }
    }
}
