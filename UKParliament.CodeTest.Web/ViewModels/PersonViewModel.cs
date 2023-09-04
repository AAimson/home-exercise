using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "A name must be provided")]
    public string Name { get; set; }
    [Required(ErrorMessage = "A DOB must be provided")]
    public DateOnly DateOfBirth { get; set; }
    [Required(ErrorMessage = "An address must be provided")]
    public string Address { get; set; }
}