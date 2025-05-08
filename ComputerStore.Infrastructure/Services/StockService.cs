using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _context;

        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportAsync(List<StockImportDto> stockList)
        {
            foreach (var item in stockList)
            {
                
                var categories = new List<Category>();
                foreach (var catName in item.Categories)
                {
                    var trimmed = catName.Trim();
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == trimmed);
                    if (category == null)
                    {
                        category = new Category { Name = trimmed };
                        _context.Categories.Add(category);
                    }
                    categories.Add(category);
                }

                
                var product = await _context.Products
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync(p => p.Name == item.Name);

                if (product == null)
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Description = "", 
                        Price = item.Price,
                        Categories = categories
                    };
                    _context.Products.Add(product);
                }

                
                var stock = new Stock
                {
                    Product = product,
                    Quantity = item.Quantity
                };
                _context.Stocks.Add(stock);
            }

            await _context.SaveChangesAsync();
        }
    }
}
