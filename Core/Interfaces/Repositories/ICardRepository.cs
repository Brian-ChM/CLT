using Core.DTOs.Card;
using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface ICardRepository
{
    Task<ResponseCardDto> AddCard(CreateCardDTO card);
    DetailedCardDTO GetById(Card entity);
    Task<Card> VerifyExists(int Id);
}
