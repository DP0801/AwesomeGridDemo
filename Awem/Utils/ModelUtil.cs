using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Omu.Awem.Utils
{
    /// <summary>
    /// Util used to validate input objects
    /// </summary>
    public static class ModelUtil
    {
        /// <summary>
        /// Validate input 
        /// </summary>
        /// <returns></returns>
        public static ValidState Validate(object input)
        {
            var res = new ValidState();

            var context = new ValidationContext(input);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(input, context, validationResults, true);

            res.AddResults(validationResults);
            return res;
        }
    }
}