using Core.DTOs.Product;

namespace Core.DTOs.Entity;

public class CreateEntityDTO
{
    public string EntityName { get; set; } = string.Empty;

    public CreateProductInEntityDTO Product { get; set; } = new CreateProductInEntityDTO();
}
