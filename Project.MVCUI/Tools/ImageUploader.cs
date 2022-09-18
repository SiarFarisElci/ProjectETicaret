using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Tools
{
    public static class ImageUploader
    {
        public static string UploaderImage(string serverPath , HttpPostedFileBase file , string name)
        {
            if (file != null)
            {
                Guid uniquName = Guid.NewGuid();

                string[] fileArray = file.FileName.Split('.');
                
                string extension = fileArray[fileArray.Length - 1].ToLower();

                string fileName = $"{uniquName}.{name}.{extension}";

                
                if (extension == "jpg" || extension == "gif" || extension== "png")
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(serverPath + fileName)))
                    {
                        return "1";
                    }
                    else
                    {
                        string filePath = HttpContext.Current.Server.MapPath(serverPath + fileName);
                        file.SaveAs(filePath);
                        return serverPath + fileName;
                    }
                }
                else
                {
                    return "2";
                }
            }
            else
            {
                return "3";
            }
        }
    }
}