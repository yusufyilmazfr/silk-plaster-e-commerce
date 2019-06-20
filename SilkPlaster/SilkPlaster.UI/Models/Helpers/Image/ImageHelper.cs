using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models.Helpers.Image
{
    public static class ImageHelper
    {

        public static bool IsImage(string mimeType)
        {
            string[] mimeTypes = { "image/jpeg", "image/png", "image/gif" };

            return mimeTypes.Contains(mimeType);
        }

        public static string CreateUniqueString()
        {
            return String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        }

        public static ImageUploadResultMessage Save(HttpPostedFileBase file, string absolutePath)
        {
            ImageUploadResultMessage message = new ImageUploadResultMessage();

            if (file != null && file.ContentLength > 0)
            {
                string mimeType = file.ContentType;

                if (ImageHelper.IsImage(mimeType))
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);

                    string newFileNameWithExtension = fileName + ImageHelper.CreateUniqueString() + extension;

                    string path = absolutePath + newFileNameWithExtension;

                    file.SaveAs(path);

                    if (File.Exists(path))
                    {
                        message.Result = true;
                        message.FileName = newFileNameWithExtension;
                    }

                }
            }

            return message;
        }

        public static void Remove(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}