using System.ComponentModel.DataAnnotations;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UserStorageElement 
{
    [Key]
    public string Login { get; set; }
}