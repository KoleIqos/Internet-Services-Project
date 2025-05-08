using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(List<StockImportDto> stockList)
        {
            await _stockService.ImportAsync(stockList);
            return Ok(new { Message = "Stock import successful." });
        }
    }
}
