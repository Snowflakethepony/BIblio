using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Client.Shared
{
    public static class SharedProperties
    {
        public enum BookTypes
        {
            Book,
            Copy
        }

        public enum DisplayTypes
        {
            Card,
            List,
            Table
        }
    }
}
