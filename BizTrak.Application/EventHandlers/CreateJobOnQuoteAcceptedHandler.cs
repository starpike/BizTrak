using System;
using BizTrak.Application.Exceptions;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.Domain.Events;
using MediatR;

namespace BizTrak.Application.EventHandlers;

public class CreateJobOnQuoteAcceptedHandler : INotificationHandler<QuoteAcceptedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobOnQuoteAcceptedHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(QuoteAcceptedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Quote == null)
            throw new NotFoundException("Quote not found in the notification.");

        var quote = notification.Quote;

        if (quote.Customer == null)
            throw new NotFoundException($"Customer not found for the quote with ID {quote.Id}.");

        //Make sure handles idempotentency
        var existingJob = await _unitOfWork.Jobs.GetJobByQuoteIdAsync(quote.Id);
        if (existingJob != null)
            return;

        await _unitOfWork.Jobs.AddAsync(new Job
        {
            Title = quote.Title,
            CustomerName = quote.Customer.Name ?? "",
            Start = DateTime.Now.ToUniversalTime(),
            End = DateTime.Now.AddHours(1).ToUniversalTime(),
            QuoteId = quote.Id,
            QuoteRef = quote.QuoteRef
        });
    }
}
