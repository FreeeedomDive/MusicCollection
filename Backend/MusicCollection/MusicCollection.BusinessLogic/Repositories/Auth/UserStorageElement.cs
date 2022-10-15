using System.ComponentModel.DataAnnotations;
using DatabaseCore.Models;
using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UserStorageElement : SqlStorageElement
{
    public string Login { get; set; }
    public string Password { get; set; }
}