using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Web.Extensions;

public static class WebHostExtensions
{
    public static WebApplication InitialiseData(this WebApplication host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetService<PersonManagerContext>();

        context?.People.Add(new Person
            { Id = 1, Name = "Alex", Address = "20 Old Edinburgh Road", DateOfBirth = new DateOnly(1992, 10, 7) });
        context?.People.Add(new Person
            { Id = 2, Name = "Tom", Address = "49 Kingsway North", DateOfBirth = new DateOnly(1993, 10, 7) });
        context?.People.Add(new Person
            { Id = 3, Name = "Bob", Address = "8 St Andrews Lane", DateOfBirth = new DateOnly(1994, 10, 7) });
        context?.People.Add(new Person
            { Id = 4, Name = "William", Address = "24 Ponteland Rd", DateOfBirth = new DateOnly(1995, 10, 7) });
        context?.People.Add(new Person
            { Id = 5, Name = "James", Address = "38 Maidstone Road", DateOfBirth = new DateOnly(1996, 10, 7) });

        context?.SaveChanges();
        return host;
    }
}