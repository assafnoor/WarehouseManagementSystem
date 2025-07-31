namespace WMS.Application.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}