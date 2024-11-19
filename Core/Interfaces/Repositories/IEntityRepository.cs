using Core.DTOs.Entity;
using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IEntityRepository
{
    Task<ResponseCreateEntityDTO> CreateEntity(Customer customer, CreateEntityDTO entity);
    Task<List<ResponseEntityDTO>> GetEntities(int CustomerId);
    Task<Customer> VerifyExists(int Id);
}
