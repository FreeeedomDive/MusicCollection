namespace MusicCollection.Api.Dto.Users;

public class UserSettings
{
    public bool Shuffle { get; set; }

    public static UserSettings CreateDefault()
    {
        return new UserSettings
        {
            Shuffle = false
        };
    }
}