using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data
{
    [System.Configuration.ConfigurationCollection(typeof(SessionFactoryElement))]
    public sealed class SessionFactoriesCollection : System.Configuration.ConfigurationElementCollection
    {
        public SessionFactoriesCollection()
        {
            SessionFactoryElement sessionFactory = (SessionFactoryElement) CreateNewElement();

            Add(sessionFactory);
        }

        public override System.Configuration.ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new SessionFactoryElement();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((SessionFactoryElement)element).Name;
        }

        public SessionFactoryElement this[int index]
        {
            get
            {
                return (SessionFactoryElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        new public SessionFactoryElement this[string name]
        {
            get
            {
                return (SessionFactoryElement)BaseGet(name);
            }
        }

        public int IndexOf(SessionFactoryElement sessionFactory)
        {
            return BaseIndexOf(sessionFactory);
        }

        public void Add(SessionFactoryElement sessionFactory)
        {
            BaseAdd(sessionFactory);
        }

        protected override void BaseAdd(System.Configuration.ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SessionFactoryElement sessionFactory)
        {
            if (BaseIndexOf(sessionFactory) >= 0)
            {
                BaseRemove(sessionFactory.Name);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
