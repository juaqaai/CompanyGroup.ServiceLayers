using System;
using System.Collections.Generic;
using CompanyGroup.Domain.Utils;

namespace CompanyGroup.Domain.PartnerModule
{
    public class Customer : CompanyGroup.Domain.Core.DomainObject<string>, CompanyGroup.Domain.Core.IHasAssignedId<string>
    {
        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Customer() {}

        public Customer(string companyName) 
        {
            CompanyName = companyName;
        }

        /// <summary>
        /// Provides an accessor for injecting an IOrderDao so that this class does 
        /// not have to create one itself.  Can be set from a controller, using 
        /// IoC, or from another business object.  As a rule-of-thumb, I do not like
        /// domain objects to use DAOs directly; but there are exceptional cases; 
        /// therefore, this shows a way to do it without having a concrete dependency on the DAO.
        /// </summary>
        public IOrderRepository OrderRepository 
        {
            get
            {
                if (orderRepository == null) 
                {
                    throw new MemberAccessException("OrderDao has not yet been initialized");
                }

                return orderRepository;
            }
            set {
                orderRepository = value;
            }
        }

        public string CompanyName {
            get { return companyName; }
            set 
            {
                Check.Require(!string.IsNullOrEmpty(value), "A valid company name must be provided");
                companyName = value;
            }
        }

        public string ContactName 
        {
            get { return contactName; }
            set { contactName = value; }
        }

        public IList<Order> Orders {
            get { return new List<Order>(orders).AsReadOnly(); }
            protected set { orders = value; }
        }

        public void AddOrder(Order order) {
            if (order != null && !orders.Contains(order)) {
                orders.Add(order);
            }
        }

        public void RemoveOrder(Order order) {
            if (order != null && orders.Contains(order)) {
                orders.Remove(order);
            }
        }

        /// <summary>
        /// To get all the orders ordered on a particular date, we could loop through 
        /// each item in the Orders collection.  But if a customer has thousands of 
        /// orders, we don't want all the orders to have to be loaded from the database.  
        /// Instead, we can let the data layer do the filtering for us.
        /// </summary>
        public List<Order> GetOrdersOrderedOn(DateTime orderedDate) {

            List<Order> matchingOrdersForThisCustomer = new List<Order>();

            return matchingOrdersForThisCustomer;
        }

        public void SetAssignedIdTo(string assignedId) {
            Check.Require(! string.IsNullOrEmpty(assignedId), "assignedId may not be null or empty");
            // As an alternative to Check.Require, the Validation Application Block could be used for the following
            Check.Require(assignedId.Trim().Length == 5, "assignedId must be exactly 5 characters");

            Id = assignedId.Trim().ToUpper();
        }

        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    CompanyName + "|" +
                    ContactName).GetHashCode();
        }

        private IOrderRepository orderRepository;
        private string companyName = "";
        private string contactName = "";
        private IList<Order> orders = new List<Order>();
    }
}
