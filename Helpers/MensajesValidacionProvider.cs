using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AppDonnyCuevas20210074.Helpers;

// Traduce al español los mensajes de validación de los atributos de los modelos
// ([Required], [StringLength], [Range]) sin tener que modificar cada modelo.
public class MensajesValidacionProvider : IValidationMetadataProvider
{
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        foreach (var atributo in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>())
        {
            if (!string.IsNullOrEmpty(atributo.ErrorMessage) || atributo.ErrorMessageResourceType != null)
                continue;

            atributo.ErrorMessage = atributo switch
            {
                RequiredAttribute => "Este campo es obligatorio.",
                StringLengthAttribute => "Este campo admite como máximo {1} caracteres.",
                RangeAttribute => "El valor debe estar entre {1} y {2}.",
                _ => atributo.ErrorMessage
            };
        }
    }
}
