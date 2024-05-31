using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagemetApi.Services; 
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTestProj
{
    internal class AnalyticsBLTest
    {
        private IRepository<int, Borrowed> _borrowedRepository;
        private IAnalyticsService _analyticsService; 
        private LibraryManagementContext _context;


        [SetUp]
        public async Task Setup()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();
            _borrowedRepository = new BorrowedRepository(_context);
            _analyticsService = new AnaylticsService(_borrowedRepository); 
        }
        [Test]
        public async Task GetAnalyticsTest()
        {
            AnalyticsDTO dto = new AnalyticsDTO()
            {
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now
            };
            var result = await _analyticsService.GetAnalytics(dto);
            Assert.That(result.Count(), Is.EqualTo(1));
        }
        [Test]
        public async Task OverDueAnalytics()
        {
            AnalyticsDTO dto = new AnalyticsDTO()
            {
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now
            };
            var result = await _analyticsService.returnODAnalyticsDTOs(dto);
            Assert.That(result.Count(), Is.EqualTo(1));
        }
    }
}
