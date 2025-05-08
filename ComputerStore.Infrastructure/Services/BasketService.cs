using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BasketResultDto> CalculateDiscountAsync(List<BasketItemDto> basket)
        {
            decimal total = 0;
            decimal totalWithDiscount = 0;

            var productIds = basket.Select(x => x.ProductId).ToList();
            var products = await _context.Products
                .Include(p => p.Categories)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var item in basket)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product ID {item.ProductId} not found.");

                var stock = await _context.Stocks
                    .Where(s => s.ProductId == product.Id)
                    .SumAsync(s => s.Quantity);

                if (item.Quantity > stock)
                    throw new InvalidOperationException($"Not enough stock for '{product.Name}'.");

                decimal itemTotal = product.Price * item.Quantity;
                total += itemTotal;

                
                bool qualifiesForDiscount = products
                    .Where(p => p.Id != product.Id)
                    .Any(p => p.Categories.Any(c => product.Categories.Select(pc => pc.Id).Contains(c.Id)));

                if (qualifiesForDiscount)
                {
                    
                    var discounted = product.Price * 0.95m;
                    var rest = product.Price * (item.Quantity - 1);
                    totalWithDiscount += discounted + rest;
                }
                else
                {
                    totalWithDiscount += itemTotal;
                }
            }

            return new BasketResultDto
            {
                TotalWithoutDiscount = total,
                TotalWithDiscount = totalWithDiscount
            };
        }
    }
}
