using Core.DTOs.Entity;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Infraestructura.Services;

public class CustomerService : ICustomerService
{
    private readonly IEntityRepository _entityRepository;

    public CustomerService(IEntityRepository entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<ResponseCreateEntityDTO> CreateEntity(int CustomerId, CreateEntityDTO entity)
    {
        var customer = await _entityRepository.VerifyExists(CustomerId);
        return await _entityRepository.CreateEntity(customer, entity);
    }

    public async Task<List<ResponseEntityDTO>> GetEntities(int CustomerId)
    {
        return await _entityRepository.GetEntities(CustomerId);
    }
}
