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

namespace TestProj
{
    public class UserRepoTest
    {
        DbContextOptions<LibraryManagementContext> options = new DbContextOptionsBuilder<LibraryManagementContext>()
            .UseInMemoryDatabase(databaseName: "LibraryManagement")
            .Options;

        IRepository<int, User> userRepo;
        IRepository<int , Role> roleRepo;
        Role role;
        int roleId; 
        
        [SetUp]
        public async Task Setup()
        {
            userRepo = new UserRepository(new LibraryManagementContext(options));
            roleRepo = new RoleRepository(new LibraryManagementContext(options));
            role = new Role
            {
                Name = "Admin"
            };
            await roleRepo.Insert(role);
            roleId = role.Id;
        }
        [Test]
        public async Task AddUser()
        {
            User user = new User
            {
                Username = "ramu",
                Email = "ramu@gmail.com",
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 1, 2, 3, 4 },
                Active = true,
                RoleId = roleId
            }; 
            var result = await  userRepo.Insert(user);
            Assert.AreEqual(user, result);
        }
        [Test]
        public async Task GetUserByEmail()
        {
            User user = new User
            {
                Username = "ramu",
                Email = "ramu@gmail.com",
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 1, 2, 3, 4 },
                Active = true,
                RoleId = roleId
            };
            var result = await userRepo.Insert(user);
            var userByEmail = await ((UserRepository)userRepo).GetUserByEmail(user.Email); 
            Assert.IsNotNull(userByEmail);
        }
        [Test]
        public async Task GetUserByEmailThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await ((UserRepository)userRepo).GetUserByEmail(""));
        }
        [Test]
        public async Task UpdateUser()
        {
            User user = new User
            {
                Username = "ramu",
                Email = "ramu@gmail.com",
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 1, 2, 3, 4 },
                Active = true,
                RoleId = roleId
            };
            var result = await userRepo.Insert(user);
            result.Active = false;
            var updatedUser = await userRepo.Update(result);
            Assert.IsFalse(updatedUser.Active);
        }
        [Test]
        public async Task DeleteUser()
        {
            User user = new User
            {
                Username = "ramu",
                Email = "ramu@gmail.com",
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 1, 2, 3, 4 },
                Active = true,
                RoleId = roleId
            };
            var result = await userRepo.Insert(user);
            var deletedUser = await userRepo.Delete(result.Id);
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepo.GetOneById(deletedUser.Id));
        }
        [Test]
        public async Task GetUsers()
        {
            var users = userRepo.Get();
            Assert.IsNotNull(users);
        }

        [Test]
        public async Task DelteuserThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepo.Delete(0));
        }

    }
}
