using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonServiceTests
    {
        private readonly PersonManagerContext _personContext;
        private readonly IPersonService _personService;
        public PersonServiceTests()
        {
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            _personContext = new PersonManagerContext(options);
            _personService = new PersonService(_personContext);
        }
        
        [Theory]
        [InlineData("Alex", "No 1", true)]
        [InlineData("Tom", "No 2", true)]
        [InlineData("Bill", "No 3", true)]
        [InlineData("Bob", "No 4", true)]
        [InlineData("BEn", "No 5", false)]
        public async Task Get_Persons_ReturnPeople(string name, string address, bool find)
        {
            // arrange
            _personContext.People.Add(new Person { Name = "Alex", Address = "No 1" });
            _personContext.People.Add(new Person { Name = "Tom", Address = "No 2" });
            _personContext.People.Add(new Person { Name = "Bill", Address = "No 3" });
            _personContext.People.Add(new Person { Name = "Bob", Address = "No 4" });
            await _personContext.SaveChangesAsync();
            
            // act
            var results = await _personService.Get();
            
            // assert
            Assert.NotNull(results);
            Assert.True(results.Any());
            Assert.Equal(4, results.Count);

            var person = results.Find(p => p.Name == name);
            if (find)
            {
                Assert.NotNull(person);
                Assert.Equal(name, person.Name);
                Assert.Equal(address, person.Address);
            }
            else
            {
                Assert.Null(person);
            }
        }

        [Fact]
        public async Task Get_NoPeopleExists_Success()
        {
            // act
            var results = await _personService.Get();

            // assert 
            Assert.NotNull(results);
            Assert.False(results.Any());
        }
    }
}