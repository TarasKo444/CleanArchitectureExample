namespace CleanArchitecture.Common.Exceptions;

public class UserFriendlyException : Exception
{
    public int Status { get; set; }
    
    public UserFriendlyException(int status, string message)
        : base(message)
    {
        Status = status;
    }
}
