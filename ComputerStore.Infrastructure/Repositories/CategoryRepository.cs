using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync() => await _context.Categories.ToListAsync();
        public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);
        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> UpdateAsync(int id, Category updated)
        {
            var existing = await _context.Categories.FindAsync(id);
            if (existing == null) return null;

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

