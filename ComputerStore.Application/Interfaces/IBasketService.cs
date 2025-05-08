using ComputerStore.Application.DTOs;

namespace ComputerStore.Application.Interfaces
{
    public interface IBasketService
    {
        Task<BasketResultDto> CalculateDiscountAsync(List<BasketItemDto> basket);
    }
}
