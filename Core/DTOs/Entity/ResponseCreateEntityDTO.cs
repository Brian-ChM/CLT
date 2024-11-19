using Core.DTOs.Product;

namespace Core.DTOs.Entity;

public class ResponseCreateEntityDTO
{
    public string Entidad { get; set; } = string.Empty;
    public ResponseProductDTO Product { get; set; } = new();
}
