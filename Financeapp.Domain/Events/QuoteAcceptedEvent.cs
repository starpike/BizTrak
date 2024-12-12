using System;
using FinanceApp.Domain.Entities;
using MediatR;

namespace FinanceApp.Domain.Events;

public class QuoteAcceptedEvent : INotification
{
    public Quote Quote { get; }

    public QuoteAcceptedEvent(Quote quote)
    {
        Quote = quote ?? throw new ArgumentNullException(nameof(quote));
    }
}
