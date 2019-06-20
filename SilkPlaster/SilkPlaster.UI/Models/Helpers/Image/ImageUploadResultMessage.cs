using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models.Helpers.Image
{
    public class ImageUploadResultMessage
    {
        public bool Result { get; set; }
        public string FileName { get; set; }

        public ImageUploadResultMessage()
        {
            Result = false;
        }
    }
}