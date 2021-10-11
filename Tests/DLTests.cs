using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DL;
using Models;

namespace Tests
{
    public class DLTests
    {
        private readonly DbContextOptions<ShoppingDBContext> options;

        public DLTests()
        {
            options = new DbContextOptionsBuilder<ShoppingDBContext>()
                .UseSqlite("Filename=Test.db").Options;
            Seed();
        }

        [Fact]
        public void GetLoggedInCustomerShouldGetCustomer()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);
                Models.Customer customer = new Models.Customer()
                {
                    Name = "King",
                    Phonenumber = "1234123413",
                    Password = "1234",
                    Password2 = "1234"
                };

                repo.AddCustomer(customer);
            }

            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);

                //Act
                Customer cust = repo.GetLoggedInCustomer("1234123413", "1234");

                //Assert
                Assert.Equal("King", cust.Name);
            }
        }

        [Fact]
        public void GetManagerStoresShouldGetStores()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);

                //Act
                var stores = repo.GetManagerStores("1234561234");

                //Assert
                Assert.Single(stores);
            }
        }

        [Fact]
        public void GetStoresShouldGetStores()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);

                //Act
                var stores = repo.GetCustomerStores();

                //Assert
                Assert.Single(stores);
            }
        }

        [Fact]
        public void GetProductsShouldGetProducts()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);

                //Act
                var products = repo.GetProducts("Javas");

                //Assert
                Assert.Single(products);
            }
        }

        [Fact]
        public void AddingACustomerShouldAddACustomer()
        {
            using(var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);
                Models.Customer customer = new Models.Customer()
                {
                    Name = "King",
                    Phonenumber = "1234123412",
                    Password = "1234",
                    Password2 = "1234"
                };

                Models.Customer customer1 = new Models.Customer()
                {
                    Id = 1,
                    Name = "John D",
                    Phonenumber = "1234123412",
                    Password = "12345",
                    Password2 = "12345"
                };

                //Act
                repo.AddCustomer(customer);
                repo.AddCustomer(customer1);
            }

            using(var context = new ShoppingDBContext(options))
            {
                //Assert
                Customer cust = context.Customers.FirstOrDefault(c => c.Phonenumber == "1234123412");
                Customer cust1 = context.Customers.FirstOrDefault(c => c.Name == "John D");

                Assert.Null(cust1);

                Assert.NotNull(cust);
                Assert.Equal("King", cust.Name);
                Assert.Equal("1234123412", cust.Phonenumber);
                Assert.Equal("1234", cust.Password);
                Assert.Equal("1234", cust.Password2);
            }
        }

        [Fact]
        public void AddingAStoreShouldAddAStore()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);
                Models.Store store = new Models.Store()
                {
                    Number = "Fidodido",
                    Location = "Uhawk",
                    Zipcode = "02934",
                    ManagerPhone = "1234561234"
                };

                //Act
                Store st = repo.AddStore(store);
            }

            using (var context = new ShoppingDBContext(options))
            {
                //Assert
                Store store1 = context.Stores.FirstOrDefault(c => c.Number == "Fidodido");

                Assert.NotNull(store1);
                Assert.Equal("Fidodido", store1.Number);
                Assert.Equal("Uhawk", store1.Location);
                Assert.Equal("02934", store1.Zipcode);
            }
        }

        [Fact]
        public void AddingAProductShouldAddAProduct()
        {
            using (var context = new ShoppingDBContext(options))
            {
                //Arrange
                IRepo repo = new DBRepo(context);
                Models.Product product = new Models.Product()
                {
                    Id = 1,
                    Name = "Cookies",
                    Quantity = 4,
                    UnitPrice = 5.4M,
                    StoreID = "Javas"
                };

                //Act
                Product p = repo.AddProduct(product);
            }

            using (var context = new ShoppingDBContext(options))
            {
                //Assert
                Product p = context.Products.FirstOrDefault(c => c.Name == "Cookies");

                Assert.NotNull(p);
                Assert.Equal("Cookies", p.Name);
                Assert.Equal(4, p.Quantity);
                Assert.Equal(5.4M, p.UnitPrice);
            }
        }

        private void Seed()
        {
            using (var context = new ShoppingDBContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Customers.AddRange(
                    new Customer()
                    {
                        Name = "John",
                        Phonenumber = "7871231234",
                        Password = "12345",
                        Password2 = "12345"
                    }
                );

                context.Stores.AddRange(
                    new Store()
                    {
                        Id = 1,
                        Number = "Javas",
                        Location = "Watertown",
                        Zipcode = "02345",
                        ManagerPhone = "1234561234"
                    }
                );

                context.Products.AddRange(
                    new Product()
                    {
                        Id = 1,
                        Name = "Pringles",
                        Quantity = 4,
                        UnitPrice = 5.4M,
                        StoreID = "Javas"
                    }
                );

                context.SaveChanges();
            }
         
        }
    }
}
