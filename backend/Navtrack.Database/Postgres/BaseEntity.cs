using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Database.Postgres;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}