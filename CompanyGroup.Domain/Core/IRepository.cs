using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// Base interface for implement a "Repository Pattern", http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    public interface IRepository<T> where T : class
    {

        /// <summary>
        /// kollekció elemeinek kiolvasása
        /// </summary>
        /// <returns>List of selected elements</returns>
        MongoDB.Driver.MongoCollection<T> GetCollection();

        /// <summary>
        /// kollekció elemeinek kiolvasása kollekció név alapján 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        MongoDB.Driver.MongoCollection<T> GetCollection(string collection);

        /// <summary>
        /// kapcsolat bontása
        /// </summary>
        void Disconnect();

        /// <summary>
        /// mongo kollekció törlése
        /// </summary>
        void DropCollection();

        /// <summary>
        /// törli a kollekcióhoz tartozó összes indexet 
        /// </summary>
        void DeleteIndexes();

        /// <summary>
        /// kiüríti az elemeket a megadott kollekcióból
        /// </summary>
        /// <param name="dataAreaId"></param>
        void RemoveItemsFromCollection(string dataAreaId);

        /// <summary>
        /// eltávolítja az összes elemet a kollekcióból 
        /// </summary>
        void RemoveAllItemsFromCollection();

    }
}
