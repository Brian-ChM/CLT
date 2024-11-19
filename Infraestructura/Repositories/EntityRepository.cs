using Core.DTOs.Entity;
using Core.DTOs.Product;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infraestructura.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories;

public class EntityRepository : IEntityRepository
{
    private readonly ApplicationDbContext _context;


    public EntityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseCreateEntityDTO> CreateEntity(Customer customer, CreateEntityDTO entity)
    {

        var entities = await _context.Entities.ToListAsync();

        var EntityExists = entities.FirstOrDefault(e => e.EntityName.ToLower().Equals(entity.EntityName.ToLower()));

        var AddEntity = new Entity
        {
            EntityName = entity.EntityName
        };

        var AddProduct = new Product
        {
            Entity = EntityExists ?? AddEntity,
            Customer = customer,
            CustomerId = customer.Id,
            ProductName = entity.Product.Producto,
            Description = entity.Product.Description,
            Status = "Active",
            StartDate = DateTime.UtcNow,
        };

        var AddCustomerEntity = new CustomerEntity
        {
            Customer = customer,
            Entity = EntityExists ?? AddEntity,
        };

        if (EntityExists is null)
        {
            _context.Entities.Add(AddEntity);
            _context.CustomerEntities.Add(AddCustomerEntity);
        }

        _context.Products.Add(AddProduct);

        await _context.SaveChangesAsync();

        return new ResponseCreateEntityDTO
        {
            Entidad = entity.EntityName,
            Product = new ResponseProductDTO
            {
                Product = AddProduct.ProductName,
                StartDate = AddProduct.StartDate.ToShortDateString(),
            }
        };
    }

    public async Task<List<ResponseEntityDTO>> GetEntities(int CustomerId)
    {
        var entitiesWithProducts = await _context.CustomerEntities
                .Where(e => e.CustomerId == CustomerId)
                .Include(e => e.Entity)
                .ThenInclude(e => e.Products.Where(e => e.CustomerId == CustomerId))
                .Select(e => e.Entity)
            .ToListAsync();


        return entitiesWithProducts.Select(e => new ResponseEntityDTO
        {
            Entidad = e.EntityName,
            Products = e.Products.Select(p => new ResponseProductDTO
            {
                Product = p.ProductName,
                Description = p.Description,
                StartDate = p.StartDate.ToShortDateString()
            }).ToList()
        }).ToList();
    }

    public async Task<Customer> VerifyExists(int Id)
    {
        return await _context.Customers.FindAsync(Id) ??
            throw new Exception("No encontró el cliente con el Id solicitado.");
    }
}
