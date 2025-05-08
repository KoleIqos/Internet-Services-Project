using AutoMapper;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.Include(p => p.Categories).ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Categories)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            
            if (dto.Categories != null && dto.Categories.Any())
            {
                product.Categories = await _context.Categories
                    .Where(c => dto.Categories.Select(dc => dc.Id).Contains(c.Id))
                    .ToListAsync();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductDto dto)
        {
            var product = await _context.Products.Include(p => p.Categories)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;

            
            product.Categories = await _context.Categories
                .Where(c => dto.Categories.Select(dc => dc.Id).Contains(c.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
