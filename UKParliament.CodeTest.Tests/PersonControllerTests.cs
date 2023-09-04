using Xunit;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Tests
{
    public class PersonControllerTests
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PersonController> _logger;
        private readonly PersonController _personController;
        public PersonControllerTests()
        {
            _personService = A.Fake<IPersonService>();
            _logger = A.Fake<ILogger<PersonController>>();
            
            _personController = new PersonController(_logger, _personService);
        }
        
        [Fact]
        public async Task Get_WhenPeopleDoNotExist_OkResult()
        {
            // Arrange
            A.CallTo(() => _personService.Get()).Returns(new List<Person>());
            
            // Act
            var actionResult = await _personController.Get();
            var okResult = actionResult as OkObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Get()).MustHaveHappened();
            
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(typeof(List<PersonViewModel>), okResult.Value.GetType());
            Assert.Empty(okResult.Value as List<PersonViewModel>);
        }
        
        [Fact]
        public async Task Get_WhenPeopleExist_OkResult()
        {
            // Arrange
            A.CallTo(() => _personService.Get()).Returns(new List<Person>
            {
                new ()
                {
                    Address = "England", DateOfBirth = new DateOnly(1992,10,7), Id = 1, Name = "Alex"
                }, 
                new ()
                {
                    Address = "Scotland", DateOfBirth = new DateOnly(1992,10,7), Id = 2, Name = "Tom"
                }
            });
            
            // Act
            var actionResult = await _personController.Get();
            var okResult = actionResult as OkObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Get()).MustHaveHappened();
            
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(typeof(List<PersonViewModel>), okResult.Value.GetType());
            Assert.Equal(2, (okResult.Value as List<PersonViewModel>).Count);
        }
        
        [Fact]
        public async Task GetById_SinglePersonDoesNotExist_BadResult()
        {
            // Arrange
            const int id = 1;
            A.CallTo(() => _personService.Get(id)).Returns((Person?)null);
            
            // Act
            var actionResult = await _personController.GetById(id);
            var result = actionResult as BadRequestObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Get(id)).MustHaveHappened();
            
            Assert.NotNull(result);
            
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Person unable to be found for id 1", result.Value);
        }
        
        [Fact]
        public async Task GetById_SinglePersonWhoExists_OkResult()
        {
            // Arrange
            const int id = 1;
            A.CallTo(() => _personService.Get(id)).Returns(new Person
            { 
                Address = "England", DateOfBirth = new DateOnly(1992,10,7), Id = 1, Name = "Alex"
            });
            
            // Act
            var actionResult = await _personController.GetById(id);
            var okResult = actionResult as OkObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Get(id)).MustHaveHappened();
            
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            
            var person = okResult.Value as PersonViewModel;
            
            Assert.Equal("Alex", person.Name);
            Assert.Equal("England", person.Address);
            Assert.Equal(1, person.Id);
        }
        
        [Fact]
        public async Task Update_UpdatePerson_OkResult()
        {
            // Arrange
            const int id = 1;
            var personViewModel = new PersonViewModel { Address = "England", DateOfBirth = new DateOnly(1992, 10, 7), Name = "Alex" };
            var person = new Person { Address = "England", DateOfBirth = new DateOnly(1992, 10, 7), Id = 1, Name = "Alex" };

            A.CallTo(() => _personService.Edit(person)).Returns(true);
            
            // Act
            var actionResult = await _personController.Update(id, personViewModel);
            var okResult = actionResult as OkObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Edit(A<Person>.Ignored)).MustHaveHappened();
            
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            
            Assert.Equivalent(new { message = "Person updated" }, okResult.Value);
        }
        
        [Fact]
        public async Task Post_CreatePerson_OkResult()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Address = "England", DateOfBirth = new DateOnly(1992, 10, 7), Name = "Alex" };
            var person = new Person { Address = "England", DateOfBirth = new DateOnly(1992, 10, 7), Id = 1, Name = "Alex" };

            A.CallTo(() => _personService.Create(person)).Returns(true);
            
            // Act
            var actionResult = await _personController.Post(personViewModel);
            var okResult = actionResult as OkObjectResult;
            
            // Assert
            A.CallTo(() => _personService.Create(A<Person>.Ignored)).MustHaveHappened();
            
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            
            Assert.Equivalent(new { message = "Person created" }, okResult.Value);
        }
    }
}