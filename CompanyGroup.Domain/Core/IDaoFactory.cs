using CompanyGroup.Core.Domain;

namespace CompanyGroup.Core.DataInterfaces
{
    /// <summary>
    /// Provides an interface for retrieving DAO objects
    /// </summary>
    public interface IDaoFactory 
    {
        ICustomerDao GetCustomerDao();

        IStructureDao GetStructureDao(string serverHostName, int port, string dataBaseName, string collectionName);
    }

    // There's no need to declare each of the DAO interfaces in its own file, so just add them inline here.
    // But you're certainly welcome to put each declaration into its own file.
    #region Inline interface declarations

    //public interface ICustomerDao : IDao<Customer, string> { }

    #endregion
}
