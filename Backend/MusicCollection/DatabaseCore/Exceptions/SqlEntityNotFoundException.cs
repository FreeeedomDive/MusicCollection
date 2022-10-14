namespace DatabaseCore.Exceptions;

public class SqlEntityNotFoundException : Exception
{
    public SqlEntityNotFoundException(Guid id): base($"Entity {id} not found")
    {
        
    }
}