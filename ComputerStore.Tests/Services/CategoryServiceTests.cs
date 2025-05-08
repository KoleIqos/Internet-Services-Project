using AutoMapper;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Infrastructure.Services;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ComputerStore.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public CategoryServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCategory()
        {
            
            var dto = new CategoryDto { Name = "TestCategory", Description = "TestDesc" };

            using var context = new AppDbContext(_dbContextOptions);
            var service = new CategoryService(context, _mapper);

            
            var result = await service.CreateAsync(dto);

            
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("TestCategory");

            var saved = await context.Categories.FirstOrDefaultAsync(c => c.Id == result.Id);
            saved.Should().NotBeNull();
        }
    }
}
