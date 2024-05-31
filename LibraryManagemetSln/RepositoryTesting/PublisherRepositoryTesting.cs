using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RepositoryTesting
{
    public class PublisherRepositoryTesting
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Publisher> publisherRepo;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            publisherRepo = new PublisherRepository(context);
        }

        [Test]
        public async Task AddPublisher()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            var result = await publisherRepo.Insert(publisher);
            Assert.That(result, Is.EqualTo(publisher));
        }

        [Test]
        public async Task GetPublisherById()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            await publisherRepo.Insert(publisher);
            var result = await publisherRepo.GetOneById(publisher.Id);
            Assert.That(result, Is.EqualTo(publisher));
        }

        [Test]
        public async Task GetPublisherByName()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            await publisherRepo.Insert(publisher);
            var result = await ((PublisherRepository)publisherRepo).GetPublisherByName(publisher.Name);
            Assert.That(result.Name, Is.EqualTo(publisher.Name));
        }

        [Test]
        public async Task GetAllPublishers()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            await publisherRepo.Insert(publisher);
            var result = await publisherRepo.Get();
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task UpdatePublisher()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            await publisherRepo.Insert(publisher);
            publisher.Name = "Publisher2";
            var result = await publisherRepo.Update(publisher);
            Assert.That(result.Name, Is.EqualTo(publisher.Name));
        }

        [Test]
        public async Task DeletePublisher()
        {
            var publisher = new Publisher { Name = "Publisher1" };
            await publisherRepo.Insert(publisher);
            var res = await publisherRepo.Delete(publisher.Id);
            Assert.That(res, Is.EqualTo(publisher));
        }

        [Test]
        public async Task DeletePublisherThrowEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await publisherRepo.Delete(0));
        }
    }
}
