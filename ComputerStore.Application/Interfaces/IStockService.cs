using ComputerStore.Application.DTOs;

namespace ComputerStore.Application.Interfaces
{
    public interface IStockService
    {
        Task ImportAsync(List<StockImportDto> stockList);
    }
}
