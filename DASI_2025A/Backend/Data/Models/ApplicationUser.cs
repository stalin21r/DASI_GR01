﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;
public class ApplicationUser : IdentityUser
{
  [Required]
  public required string FirstName { get; set; }

  [Required]
  public required string LastName { get; set; }

  [Required]
  public DateTime DateOfBirth { get; set; }

  [Required]
  public required string ScoutUniqueId { get; set; }

    [Required]
    [Precision(18, 4)]
    public required decimal AccountBalance { get; set; }

  [Required]
  [EmailAddress]
  [PersonalData]
  public override string? Email
  {
    get => base.Email;
    set
    {
      base.Email = value;
      base.UserName = value; // Establecer el email como UserName
    }
  }

  [Required]
  public bool Active { get; set; }

  // FK a Occupation
  [Required]
  public int OccupationFk { get; set; }

  [ForeignKey("OccupationFk")]
  public OccupationEntity? Occupation { get; set; }
}
