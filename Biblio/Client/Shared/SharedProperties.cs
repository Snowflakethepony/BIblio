﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Client.Shared
{
    public static class SharedProperties
    {
        public static string BaseBookCoverUrl = "/Media/Images/BaseBookImg.png";

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
