using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Omu.Awem.Utils
{
    /// <summary>
    /// info about input validation state
    /// </summary>
    public class ValidState
    {
        readonly IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        /// <summary>
        /// is valid
        /// </summary>
        public bool IsValid()
        {
            return errors.Count == 0;
        }

        /// <summary>
        /// add error message
        /// </summary>
        public void Add(string name, string message)
        {
            if (!errors.ContainsKey(name))
            {
                errors.Add(name, new[] { message });
            }
            else
            {
                errors[name] = errors[name].Concat(new[] { message }).ToArray();
            }
        }

        /// <summary>
        /// add error messages
        /// </summary>
        /// <param name="results"></param>
        public void AddResults(ICollection<ValidationResult> results)
        {
            foreach (var vr in results)
            {
                foreach (var name in vr.MemberNames)
                {
                    Add(name, vr.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// get errors in the format required for the batch inline edit grid mod
        /// </summary>
        /// <returns></returns>
        public object ToInlineErrors()
        {
            return new { _errs = errors };
        }
    }
}