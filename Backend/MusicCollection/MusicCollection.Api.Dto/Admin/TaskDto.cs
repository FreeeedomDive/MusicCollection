namespace MusicCollection.Api.Dto.Admin;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string State { get; set; }
    public int Progress { get; set; }
}