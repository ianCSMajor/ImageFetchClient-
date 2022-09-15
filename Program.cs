// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using System.Net;
//using ConsoleApp6;

internal class Program
{
    static void Main(string[] args)
    {
        string imageCaption = "hi"; //Text to be displayed on picture
        string saveFileName = "image.jpg"; //Name of file specified by user 
        string fileExt = "";
        bool isValidExtension = false;

        fileExt = System.IO.Path.GetExtension(saveFileName);
        if (fileExt.Length != 0)
        {
            CheckExtension(fileExt);
        }

        //program execution starts from here

        //FOR: saving args to variables
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-o")    
            {
                saveFileName = args[i + 1];
                fileExt = System.IO.Path.GetExtension(saveFileName);
                if (fileExt.Length != 0)
                {
                    isValidExtension = CheckExtension(fileExt);
                }
            }
            if (args[i] == "-t")
            {
                imageCaption = args[i + 1];
            }
        }
        if (isValidExtension)
        {
            LoadPicture(saveFileName, imageCaption);
        }
        else
        {
            throw new ArgumentException(String.Format("Saving image with {0} extension is prohibited.", fileExt),
                                      "num");
        }
        Console.ReadKey();

    }
    /// <summary>
    /// Validator to test if recommended extension is used 
    /// </summary>
    /// <param name="extension"></param>
    public static bool CheckExtension(string extension)
    {
        List<string> fileTypesList = new List<string> { "*.tif", "*.png", "*.gif", "*.jpg", "*.bmp" };

        if(fileTypesList.Contains(extension))
        {
            return true;
        }
        return false;      
    }
    /// <summary>
    /// Validator to test for illegal character input IE: '#', '=', '|', etc
    /// </summary>
    /// <param name="fileName"></param>
    public static bool CheckIllegalCharacter(string fileName)
    {
        string regex = @"^[\w\-. ]+$";
        if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Async helper function to load in image. 
    /// </summary>
    /// <param name="SaveFileName"></param>
    /// <param name="PictureCaption"></param>
    public static void LoadPicture(string SaveFileName, string PictureCaption)
    {
        //Ian, check to see if name has an extension 
        string sendToPath = @"C:\Users\IOh\Documents\ConsoleApp\" + SaveFileName + ".png";

        //string sendToPath = @"C:\Users\IOh\Downloads\imageOfCat.jfif";
        string fetchPicWithoutCaptionUri = @"https://cataas.com/cat";
        string url = "";

        //IF- url will grab a picture of a cat without captions, else captions input by user will be added. 
        if (PictureCaption.Length <= 0)
        {
            url = fetchPicWithoutCaptionUri;
        }
        else
        {
            fetchPicWithoutCaptionUri += @"/says/" + PictureCaption;
            url = fetchPicWithoutCaptionUri;
        }


        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(url), sendToPath);
        }
    }

}


