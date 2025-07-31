using MediatR;

namespace WMS.Domain.Common.Models;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}