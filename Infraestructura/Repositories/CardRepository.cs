﻿using Core.DTOs.Card;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infraestructura.Context;
using Mapster;

namespace Infraestructura.Repositories;

public class CardRepository : ICardRepository
{
    private readonly ApplicationDbContext _context;

    public CardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseCardDto> AddCard(CreateCardDTO card)
    {
        var entity = card.Adapt<Card>();
        _context.Cards.Add(entity);

        await _context.SaveChangesAsync();
        return entity.Adapt<ResponseCardDto>();
    }

    public DetailedCardDTO GetById(Card entity)
    {
        return entity.Adapt<DetailedCardDTO>();
    }

    public async Task<Card> VerifyExists(int Id)
    {
        return await _context.Cards.FindAsync(Id) ??
            throw new Exception("No encontró la tarjeta con el Id solicitado.");
    }
}
