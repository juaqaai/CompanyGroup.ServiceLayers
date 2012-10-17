using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.DataAccess
{
    class CustomerRepository : BaseRepository, ICustomerRepository  
    {
        public CustomerRepository(DataAccess.SqlServer.Entities entities) : base(entities) { }

        public void GetSalesOrders()
        {
            
        }
    }
}
