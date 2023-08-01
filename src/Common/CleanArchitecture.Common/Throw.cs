using CleanArchitecture.Common.Exceptions;

namespace CleanArchitecture.Common;

public static class Throw
{
    public static void UserFriendlyExceptionIf(bool condition, int code, string message)
    {
        if (condition)
        {
            throw new UserFriendlyException(code, message);
        }
    }
    
    public static void UserFriendlyExceptionIfNull(object? obj, int code, string message)
    {
        UserFriendlyExceptionIf(obj is null, code, message);
    }
}
