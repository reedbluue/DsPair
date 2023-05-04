using DsPair.src.Enums;

namespace DsPair.src.Exceptions;
class StatusException : Exception
{
    public ErrorStatus status;
    public StatusException(ErrorStatus status = ErrorStatus.FatalError)
    {
        this.status = status;
    }
}