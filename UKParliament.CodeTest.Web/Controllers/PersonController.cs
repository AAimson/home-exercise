using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            // return single person here
            var person = await _personService.Get(id);

            if (person == null)
                return BadRequest($"Person unable to be found for id {id}");
            
            return Ok(new PersonViewModel
            {
                Name = person.Name,
                Address = person.Address,
                Id = person.Id,
                DateOfBirth = person.DateOfBirth
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return all person here
            var people = await _personService.Get();
            
            return Ok(people.Select(person => new PersonViewModel
            {
                Name = person.Name,
                Address = person.Address,
                Id = person.Id,
                DateOfBirth = person.DateOfBirth
            }).ToList());
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(PersonViewModel personViewModel)
        {
            var person = new Person
            {
                Address = personViewModel.Address,
                DateOfBirth = personViewModel.DateOfBirth,
                Name = personViewModel.Name
            };

            await _personService.Create(person);
            return Ok(new { message = "Person created"} );
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PersonViewModel personViewModel)
        {
            var person = new Person
            {
                Address = personViewModel.Address,
                DateOfBirth = personViewModel.DateOfBirth,
                Name = personViewModel.Name,
                Id = id
            };
            
            await _personService.Edit(person);
            return Ok(new { message = "Person updated" });
        }
    }
}