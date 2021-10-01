using System;
using Xunit;
using Models;

namespace Tests
{
    public class ModelTests
    {
        [Fact]
        public void CustomerShouldCreate()
        {
            //Arrange 
            Customer test = new Customer();

            //Act and Assert
            Assert.NotNull(test); 
        }

        [Fact]
        public void CustomerShouldSetValidData()
        {
            //Arrange
            Customer test = new Customer();
            string testString = "testName";

            //Act
            test.Name = testString;

            //Assert
            Assert.Equal(test.Name, testString);
        }

        [Fact]
        public void CustomerPasswordsShouldBeEqual()
        {
            //Arrange
            Customer testPassword = new Customer();
            Customer testPassword2 = new Customer();

            //Act
            testPassword.Password = "12345";
            testPassword.Password2 = "12345";

            //Assert
            Assert.Equal(testPassword.Password, testPassword.Password2);
        }

        [Theory]
        [InlineData("dfd")]
        public void CustomerPasswordShouldBeAbove4Characters(string input)
        {
            //Arrange
            Customer test = new Customer();
            
            //Act and Arrange
            Assert.Throws<InputInvalidException>(() => test.Password = input);

        }

        [Theory]
        [InlineData("dfdf")]
        [InlineData("1234")]
        public void CustomerShouldNotAllowInvalidPhoneNumber(string input)
        {
            //Arrange
            Customer test = new Customer();
            
            //Act and Arrange
            Assert.Throws<InputInvalidException>(() => test.Phonenumber = input);

        }

        [Theory]
        [InlineData("")]
        [InlineData("{]+=")]
        public void CustomerNameShouldNotAllowInvalidInput(string input){
            //Arrange
            Customer test = new Customer();

            //Act and Assert
            Assert.Throws<InputInvalidException>(() => test.Name = input);
        }

        [Theory]
        [InlineData("")]
        public void StoreNumberShouldNotBeBlank(string input){
            //Arrange
            Store test = new Store();

            //Act and Assert
            Assert.Throws<InputInvalidException>(() => test.Number = input);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+=#^")]
        public void StoreLocationShouldNotAllowInvalidInput(string input){
            //Arrange
            Store test = new Store();

            //Act and Assert
            Assert.Throws<InputInvalidException>(() => test.Location = input);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+=#^")]
        [InlineData("234")]
        public void StoreZipcodeShouldNotAllowInvalidInput(string input){
            //Arrange
            Store test = new Store();

            //Act and Assert
            Assert.Throws<InputInvalidException>(() => test.Zipcode = input);
        }
    }
}