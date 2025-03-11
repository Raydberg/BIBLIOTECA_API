using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        //-> La propiedad es requerida
        [Required(ErrorMessage = "El campo {0} es requerido")] //-> uso de placeholder {0} que se reemplaza por el nombre del campo
        [StringLength(10, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        //[FirstLetterMayuscula]
        public required string Nombre { get; set; }


        //Propiedad de Navegacion - Traer Listado de lIbros
        public List<Libro> Libros { get; set; } = new List<Libro>();

        [Range(18, 120)]
        public int Edad { get; set; }
        [CreditCard] //-> Valida el formato de una tarjeta de credito
        public string? TarjetaDeCredito { get; set; }
        [Url] //-> Valida que tenga la estructura de una URL valida
        public string? URL { get; set; }

        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var firstLetter = Nombre[0].ToString();
                if (firstLetter != firstLetter.ToUpper() && Edad > 40)
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula - por modelo",
                    new string[] { nameof(Nombre) });
                }
            }
        }
    }
}
