using CompanyGroup.Domain.Utils;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// domain object õsosztály 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    public abstract class DomainObject<T>
    {
        private T id = default(T);

        /// <summary>
        /// típusparaméter a típusa (string, int)
        /// A setter védett, csak a leszármazottakból állítható be.
        /// </summary>
        public T Id 
        {
            get { return id; }
            protected set { id = value; }
        }

        /// <summary>
        /// két domain entitás közötti egyezõség vizsgálat 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override sealed bool Equals(object obj) 
        {
            DomainObject<T> compareTo = obj as DomainObject<T>;

            return (compareTo != null) && (HasSameNonDefaultIdAs(compareTo) || (((IsTransient()) || compareTo.IsTransient()) && HasSameBusinessSignatureAs(compareTo)));
        }

        /// <summary>
        /// Transiens objektum nem hivatkozhat egy elemre az adatbázisban
        /// </summary>
        public bool IsTransient()
        {
            return Id == null || Id.Equals(default(T));
        }

        /// <summary>
        /// a leszármazottakban kötelezõen felül kell írni ezt a metódust
        /// </summary>
        public abstract override int GetHashCode();

        /// <summary>
        /// a hash kódoknak egyezniük kell ugyanazon domain objektumok vonatkozásában
        /// </summary>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        private bool HasSameBusinessSignatureAs(DomainObject<T> compareTo)
        {
            Check.Require(compareTo != null, "compareTo may not be null");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(DomainObject<T> compareTo) 
        {
            Check.Require(compareTo != null, "compareTo may not be null");

            return (Id != null && ! Id.Equals(default(T))) && (compareTo.Id != null && !compareTo.Id.Equals(default(T))) && Id.Equals(compareTo.Id);
        }
    }
}
