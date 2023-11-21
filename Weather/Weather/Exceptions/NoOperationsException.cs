namespace Weather.Exceptions
{
    public class NoOperationsException : Exception
    { public NoOperationsException(string message = "") : base($"Не добавлено новых элементов. {message}") { } 
    }
}
