using System.ComponentModel.DataAnnotations;

namespace MusicCollection.Api.Dto.Auth;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Login { get; set; }
}