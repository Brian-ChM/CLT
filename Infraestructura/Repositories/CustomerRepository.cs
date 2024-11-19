﻿using Core.DTOs.Card;
using Core.DTOs.Customer;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Request;
using FluentValidation;
using Infraestructura.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context, IValidator<CreateCustomerDTO> createValidator)
    {
        _context = context;
    }

    public async Task<List<CustomerDTO>> List(PaginationRequest request, CancellationToken cancellationToken)
    {
        var entities = await _context.Customers
            .Include(c => c.Accounts)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return entities.Adapt<List<CustomerDTO>>();
    }

    public async Task<CustomerDTO> GetById(int Id)
    {
        var entity = await VerifyExists(Id);
        return entity.Adapt<CustomerDTO>();
    }

    public async Task<CustomerDTO> AddCustomer(CreateCustomerDTO CreateCustomer)
    {
        var entity = CreateCustomer.Adapt<Customer>();
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Adapt<CustomerDTO>();
    }

    public async Task<CustomerDTO> UpdateCustomer(UpdateCustomerDTO UpdateCustomer)
    {
        var entity = await VerifyExists(UpdateCustomer.Id);

        UpdateCustomer.Adapt(entity);
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
        return entity.Adapt<CustomerDTO>();
    }

    public async Task<CustomerDTO> DeleteCustomer(int Id)
    {
        var entity = await VerifyExists(Id);

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();

        return entity.Adapt<CustomerDTO>();
    }

    public async Task<List<CardCustomerDTO>> GetCardsByCustomer(int Id)
    {
        var entity = await _context.Customers.Include(c => c.Cards).FirstOrDefaultAsync(c => c.Id == Id) ??
            throw new Exception("No hay tarjetas para el usuario solicitado o el usuario no existe.");

        return entity.Cards.Adapt<List<CardCustomerDTO>>();
    }

    private async Task<Customer> VerifyExists(int id)
    {
        return await _context.Customers.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == id) ??
            throw new Exception("No se encontró con el id solicitado");
    }
}
