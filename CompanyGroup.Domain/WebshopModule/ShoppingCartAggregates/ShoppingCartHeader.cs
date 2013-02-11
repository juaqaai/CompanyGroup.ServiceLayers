using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// kosár fejléc, az aktív kosarak fejlécadatait tartalmazó listához
    /// </summary>
    public class ShoppingCartHeader
    {
        public ShoppingCartHeader(int id, string name, bool active, int status)
        {
            this.Id = id;

            this.Name = name;

            this.Active = active;

            this.Status = status;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public int Status { get; set; }
    }

    /// <summary>
    /// aktív kosarak fejlécadatait tartalmazó lista
    /// </summary>
    public class ShoppingCartHeaderCollection : List<ShoppingCartHeader>
    {

        public ShoppingCartHeaderCollection(List<ShoppingCartHeader> list)
        {
            this.AddRange(list);
        }

        public List<ShoppingCartHeader> OpenedShoppingCartHeaders
        { 
            get 
            { 
                return this.Where(x => x.Status == (int) CartStatus.Created).ToList(); 
            }
        }

        public List<ShoppingCartHeader> StoredShoppingCartHeaders
        {
            get 
            { 
                return this.Where(x => x.Status == (int) CartStatus.Stored).ToList(); 
            }
        }

        public int ActiveCartId
        {
            get 
            { 
                ShoppingCartHeader item = this.FirstOrDefault( x => x.Active );

                if (item == null)
                {
                    return 0;
                }

                return item.Id;
            }
        }
    }
}
