using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Common
{
    public class StringComparer : IEqualityComparer<String>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(String x, String y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.ToUpper() == y.ToUpper();
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(String str)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(str, null)) return 0;
            //Calculate the hash code for the product.
            return str.GetHashCode();
        }

    }

}
