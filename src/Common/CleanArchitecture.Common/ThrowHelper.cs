using CleanArchitecture.Common.Exceptions;

namespace CleanArchitecture.Common;

public static class ThrowHelper
{
    public static void ThrowUserFriendlyExceptionIf(bool condition, int code, string message)
    {
        if (condition)
        {
            throw new UserFriendlyException(code, message);
        }
    }
    
    public static void ThrowUserFriendlyExceptionIfNull(object? obj, int code, string message)
    {
        ThrowUserFriendlyExceptionIf(obj is null, code, message);
    }
}
