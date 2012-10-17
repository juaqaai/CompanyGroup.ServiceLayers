
namespace CompanyGroup.Data
{
    /// <summary>
    /// Exposes access to NHibernate DAO classes.  Motivation for this DAO
    /// framework can be found at http://www.hibernate.org/328.html.
    /// </summary>
    public class NHibernateDaoFactory : IDaoFactory
    {
        public ICustomerDao GetCustomerDao() { return new CustomerDao(); }

        public IStructureDao GetStructureDao(string serverHostName, int port, string dataBaseName, string collectionName) { return new StructureDao( serverHostName, port, dataBaseName, collectionName); }

        #region Inline DAO implementations

        /// <summary>
        /// Concrete DAO for accessing instances of <see cref="Customer" /> from DB.
        /// This should be extracted into its own class-file if it needs to extend the
        /// inherited DAO functionality.
        /// </summary>
        //public class CustomerDao : AbstractNHibernateDao<Customer, string>, ICustomerDao { }

        #endregion
    }
}
