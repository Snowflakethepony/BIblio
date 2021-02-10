using Biblio.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Client
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

        public enum ViewState
        {
            ShowDetails,
            ShowList,
            Editing
        }

        public static string GetImageUrl(BookDTO bookDTO)
        {
            if (bookDTO.Image == null || bookDTO.Image.Length == 0)
            {
                return SharedProperties.BaseBookCoverUrl;
            }
            else
            {
                return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bookDTO.Image));
            }
        }

        public static string BoolToHumanTongue(bool eva)
        {
            return eva ? "Yes" : "No";
        }
    }
}
