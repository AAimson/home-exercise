using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly PersonManagerContext _context;

        public PersonService(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> Get()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person?> Get(int id)
        {
            return await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Edit(Person person)
        {
            _context.People.Update(person);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Create(Person person)
        {
            _context.People.Add(person);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}