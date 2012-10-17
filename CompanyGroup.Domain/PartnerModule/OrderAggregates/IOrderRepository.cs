using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// Since this extends the <see cref="IDao{TypeOfListItem, IdT}" /> behavior, it's a good idea to 
    /// place it in its own file for manageability.  In this way, it can grow further without
    /// cluttering up <see cref="IDaoFactory" />.
    /// </summary>
    public interface IOrderRepository
    {
        List<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate);
    }
}
