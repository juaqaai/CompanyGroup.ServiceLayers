using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// domain value object ősosztály
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {

        /// <summary>
        /// egyenlőség vizsgálat
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(T other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            //összes publikus jellemző összehasonlítása
            System.Reflection.PropertyInfo[] publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null && publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);

                    if (typeof(T).IsAssignableFrom(left.GetType()))
                    {
                        //check not self-references...
                        return Object.ReferenceEquals(left, right);
                    }
                    else
                    {
                        return left.Equals(right);
                    }
                });
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((object)obj == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            ValueObject<T> item = obj as ValueObject<T>;

            if ((object)item != null)
            {
                return Equals((T)item);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// hash kód előállítása
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            System.Reflection.PropertyInfo[] publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null && publicProperties.Any())
            {
                foreach (var item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if ((object)value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        hashCode = hashCode ^ (index * 13);//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                    }
                }
            }
            return hashCode;
        }

        /// <summary>
        /// egyezőség operátor
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (Object.Equals(left, null))
            {
                return (Object.Equals(right, null)) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// nem egyenlő operátor
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }

    }
}
