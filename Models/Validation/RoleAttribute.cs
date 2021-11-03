using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using ShopAutenticacao.Models.Enumeration;

namespace ShopAutenticacao.Models.Validation
{
    public class RoleAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = Enum.GetNames(typeof(EnumRoles)).ToList();
            if (list.Any(s => s.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))) 
                return ValidationResult.Success;
            
            return new ValidationResult($"{validationContext.MemberName} é inválido.");
        }
    }
}