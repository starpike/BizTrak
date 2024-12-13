namespace BizTrak.Application.Services;

using System;
using System.Linq.Expressions;
using BizTrak.Domain.Entities;

public static class QuoteFilterBuilder
{
    public static Expression<Func<Quote, bool>> BuildFilter(QuoteState? quoteState, string search)
    {
        Expression<Func<Quote, bool>> filter = q => true;

        if (quoteState.HasValue)
        {
            filter = filter.AndAlso(q => q.State == quoteState);
        }

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.AndAlso(q => q.Customer.Name.Contains(search) || q.Title.Contains(search));
        }

        return filter;
    }
}

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
