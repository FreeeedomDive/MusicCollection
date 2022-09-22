using System.ComponentModel.DataAnnotations;
using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UserStorageElement 
{
    [Key]
    public Guid Id { get; set; }
    public string Login { get; set; }
}