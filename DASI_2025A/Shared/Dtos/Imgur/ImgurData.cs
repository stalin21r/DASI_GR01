using System.ComponentModel.DataAnnotations;

namespace Shared;
public class ImgurData
{

  [Url(ErrorMessage = "La imagen debe ser una URL válida")]
  public string? Link { get; set; } = "No disponible";

  [StringLength(100)]
  public string? DeleteHash { get; set; } = "No disponible";
}