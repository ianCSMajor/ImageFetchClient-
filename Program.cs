// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using System.Net;
//using ConsoleApp6;

internal class Program
{
    static void Main(string[] args)
    {
        string imageCaption = ""; //Text to be displayed on picture
        string saveFileName = ""; //Name of file specified by user 

        //program execution starts from here

        //FOR: saving args to variables
        for(int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-o")    
            {
                saveFileName = args[i + 1];
            }
            if (args[i] == "-t")
            {
                imageCaption = args[i + 1];
            }
        }
        LoadPicture(saveFileName, imageCaption);
        Console.ReadKey();

    }
    /// <summary>
    /// Async helper function to load in image. 
    /// </summary>
    /// <param name="path"></param>
 
    public static void LoadPicture(string SaveFileName, string PictureCaption)
    {
        string sendToPath = @"C:\Users\IOh\Documents\ConsoleApp\" + SaveFileName + ".jfif";

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
