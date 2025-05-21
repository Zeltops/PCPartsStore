using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCPartsStore.Entities;

public class Address
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Short Name")]
    public string ShortName { get; set; }

    [Required]
    [MaxLength(10)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    public string Recipient { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Street { get; set; }

    [ValidateNever]
    public string UserId { get; set; }

    [ValidateNever]
    public IdentityUser User { get; set; }
}