using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Intento.SDK.Exceptions;

namespace Intento.SDK.Validation
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validate options
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns></returns>
        public static ValidationResult Validate(this BaseOptions options)
        {
            var properties = options.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var errors = new Dictionary<string, string>();
            foreach (var prop in properties)
            {
                if (!CheckRequired(prop, options, out string error))
                {
                    errors.Add(prop.Name, error);
                }
            }
            return errors.Count > 0 ? 
                new ValidationResult("Options is not valid", errors.Select(e => $"{e.Key}: {e.Value}").ToArray()) :
                ValidationResult.Success;
        }

        /// <summary>
        /// Validate options and create exception if validation is false
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="IntentoValidationException"></exception>
        public static void ValidateAndThrow(this BaseOptions options)
        {
            var result = Validate(options);
            if (result != ValidationResult.Success)
            {
                throw new IntentoValidationException(result.ErrorMessage, result.MemberNames);
            }
        }

        private static bool CheckRequired(PropertyInfo info, BaseOptions options, out string error)
        {
            error = null;
            var attr = info.GetCustomAttribute<RequiredAttribute>();
            if (attr == null)
            {
                return true;
            }
            var res = info.GetValue(options) != null;
            if (!res)
            {
                error = attr.ErrorMessage;
            }
            return res;
        }
    }
}
