using System;
using BizTrak.Domain.Entities;
using MediatR;

namespace BizTrak.Domain.Events;

public class QuoteAcceptedEvent : INotification
{
    public Quote Quote { get; }

    public QuoteAcceptedEvent(Quote quote)
    {
        Quote = quote ?? throw new ArgumentNullException(nameof(quote));
    }
}
