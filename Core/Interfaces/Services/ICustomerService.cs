using Core.DTOs.Entity;

namespace Core.Interfaces.Services;

public interface ICustomerService
{
    Task<ResponseCreateEntityDTO> CreateEntity(int CustomerId, CreateEntityDTO entity);
    Task<List<ResponseEntityDTO>> GetEntities(int CustomerId);
}
