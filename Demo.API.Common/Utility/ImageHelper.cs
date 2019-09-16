namespace Demo.API.Common.Utility
{
    using System;
    using System.IO;

    public class ImageHelper
    {
        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(filePath);
                }
            }
            catch
            {
            }
        }

        public static string GetUniqueFileName(string slug, string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return
                $"{slug.Trim().Replace(" ", "-")}_{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(fileName)}";
        }
    }
}