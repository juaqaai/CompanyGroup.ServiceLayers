using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Helpers
{

    /*
        IList<MyObject> list = new List<MyObject>();
        list.Add(new MyObject { Name = "Name3", Address = "Address3" });
        list.Add(new MyObject { Name = "Name1", Address = "Address1" });
        list.Add(new MyObject { Name = "Name2", Address = "Address2" });

        var prm = Expression.Parameter(typeof(MyObject), "root");
        IList<MyObject> sortedList = new Sorter<MyObject>().Sort(list, prm, "Name", SortDirection.Ascending);

        Console.WriteLine("Sorting simple list:");
        foreach (MyObject obj in sortedList)
            Console.WriteLine(String.Format("{0} : {1}", obj.Name, obj.Address));
        Console.WriteLine(String.Empty);

        IList<MyComplexObject> complexList =
            new List<MyComplexObject>();
        complexList.Add(new MyComplexObject { Detail = new MyObject { Name = "Name3", Address = "Address3" } });
        complexList.Add(new MyComplexObject { Detail = new MyObject { Name = "Name1", Address = "Address1" } });
        complexList.Add(new MyComplexObject { Detail = new MyObject { Name = "Name2", Address = "Address2" } });

        var prmComplex = Expression.Parameter(typeof(MyComplexObject), "root");
        MemberExpression keySelectExpr =
            Expression.Property(Expression.Property(prmComplex, "Detail"), "Name");
        IList<MyComplexObject> sortedComplexList = new Sorter<MyComplexObject>().Sort(complexList, prmComplex, keySelectExpr, SortDirection.Ascending);

        Console.WriteLine("Sorting complex list:");
        foreach (MyComplexObject obj in sortedComplexList)
            Console.WriteLine(String.Format("{0} : {1}", obj.Detail.Name, obj.Detail.Address));

        Console.WriteLine(String.Empty);
        Console.WriteLine("Press any key to exit...");
        Console.ReadLine(); 

        public class MyObject
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class MyComplexObject
        {
            public MyObject Detail { get; set; }
        }      
      
     */

    /// <summary>
    /// A generic sorter class used to wrap the LINQ OrderBy functions.
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> of object being sorted.</typeparam>
    public class Sorter<T>
    {
        /// <summary>
        /// Sorts a list of type T.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="prmExpression">The expression defining the parameter to supply to the lambda expression.</param>
        /// <param name="sortExpression">The name of the property on the parameter object to sort by.
        /// <param name="sortDirection">The direction in which to sort the objects.</param>
        /// <returns>The sorted list.</returns>
        public List<T> Sort(
            IEnumerable<T> list,
            System.Linq.Expressions.ParameterExpression prmExpression,
            string sortExpression,
            System.Web.UI.WebControls.SortDirection sortDirection)
        {
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(prmExpression, sortExpression), typeof(object)), prmExpression);
            return sortDirection == System.Web.UI.WebControls.SortDirection.Ascending ?
                list.AsQueryable<T>().OrderBy<T, object>(lambda).ToList() :
                list.AsQueryable<T>().OrderByDescending<T, object>(lambda).ToList();
        }

        /// <summary>
        /// Sorts a list of type T.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="prmExpression">The expression defining the parameter to supply to the lambda expression.</param>
        /// <param name="keySelectionExpression">
        /// A <see cref="MemberExpression"/> that identifies a property on an object in the supplied <param name="list"/> that is used to
        /// perform the sort.
        /// </param>
        /// <param name="sortDirection">The direction in which to sort the objects.</param>
        /// <returns>The sorted list.</returns>
        public List<T> Sort(
            IEnumerable<T> list,
            System.Linq.Expressions.ParameterExpression prmExpression,
            System.Linq.Expressions.MemberExpression sortExpression,
            System.Web.UI.WebControls.SortDirection sortDirection)
        {
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(System.Linq.Expressions.Expression.Convert(sortExpression, typeof(object)), prmExpression);
            return sortDirection == System.Web.UI.WebControls.SortDirection.Ascending ?
                list.AsQueryable<T>().OrderBy<T, object>(lambda).ToList() :
                list.AsQueryable<T>().OrderByDescending<T, object>(lambda).ToList();
        }
    }
}
