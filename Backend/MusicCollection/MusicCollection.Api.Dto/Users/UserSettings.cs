namespace MusicCollection.Api.Dto.Users;

public class UserSettings
{
    public static UserSettings CreateDefault()
    {
        return new UserSettings
        {
            Shuffle = false,
        };
    }

    public bool Shuffle { get; set; }
}