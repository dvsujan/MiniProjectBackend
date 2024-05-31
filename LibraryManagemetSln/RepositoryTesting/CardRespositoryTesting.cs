using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagemetApi.Contexts;

namespace RepositoryTesting
{
    public  class CardRespositoryTesting
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Card> cardRepo;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            cardRepo = new CardRepository(context);
        }

        [Test]
        public async Task AddCard()
        {
            var card = new Card { CardNumber = "1234567890", ExpDate = DateTime.Now, CVV = 123, UserId = 1 };
            var result = await cardRepo.Insert(card);
            Assert.That(result, Is.EqualTo(card));
        }

        [Test]
        public async Task GetCardById()
        {
            var card = new Card { CardNumber = "1234567890", ExpDate = DateTime.Now, CVV = 123, UserId = 1 };
            await cardRepo.Insert(card);
            var result = await cardRepo.GetOneById(card.Id);
            Assert.That(result, Is.EqualTo(card));
        }

        [Test]
        public async Task GetAllCards()
        {
            var card = new Card { CardNumber = "1234567890", ExpDate = DateTime.Now, CVV = 123, UserId = 1 };
            await cardRepo.Insert(card);
            var result = await cardRepo.Get();
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task UpdateCard()
        {
            var card = new Card { CardNumber = "1234567890", ExpDate = DateTime.Now, CVV = 123, UserId = 1 };
            await cardRepo.Insert(card);
            card.CVV = 456;
            var result = await cardRepo.Update(card);
            Assert.That(result.CVV, Is.EqualTo(456));
        }

        [Test]
        public async Task DeleteCard()
        {
            var card = new Card { CardNumber = "1234567890", ExpDate = DateTime.Now, CVV = 123, UserId = 1 };
            await cardRepo.Insert(card);
            var result = await cardRepo.Delete(card.Id);
            Assert.That(result, Is.EqualTo(card));
        }
        [Test] 
        public async Task DeleteCardThrowsNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(() => cardRepo.Delete(0));
        }
    }
}
