using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services
{
    public interface IPersonService
    {
        Task<List<Person>> Get();
        Task<Person?> Get(int id);
        Task<bool> Edit(Person person);
        Task<bool> Create(Person person);
    }
}