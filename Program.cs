using System;
using System.Threading.Tasks;
using System.Net;
//using ConsoleApp6;
/// <summary>
/// Class: Retrieve image from cataas api 
/// </summary>
internal class Program
{
    static void Main(string[] args)
    {
        string saveFileName = "haha"; //Name of file specified by user 
        string imageCaption = "hi"; //Text to be displayed on picture

        //FOR: saving args to variables
        for (int i = 0; i < args.Length; i++)
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

        fetchImageFromRestfulApi fetchImage = new fetchImageFromRestfulApi(saveFileName, imageCaption);
        fetchImage.LoadPictureHelper();

        Console.ReadKey();

    }
}
/// <summary>
/// Class: purpose is to retrieve image from cataas api.
/// </summary>
public class fetchImageFromRestfulApi
{
    private string _saveFileName;
    public string SaveFileName
    {
        get { return _saveFileName; }
        set
        {
            if(CheckExtension(value) && CheckIllegalCharacter(value))
            {
                _saveFileName = value;
            }
            else
            {
                throw new ArgumentException("{0} file name not valid", value);
            }
        }
    }

    public string ImageCaption { get; set; }
    /// <summary>
    /// Constructor for this application that sets the file name and image caption and then loading the picture.
    /// </summary>
    public fetchImageFromRestfulApi(string saveFileName, string imageCaption)
    {
        string newFileName = extensionExists(saveFileName);

        SaveFileName = newFileName;
        ImageCaption = imageCaption;

    }
    /// <summary>
    /// Helper function for loading pictures. 
    /// </summary>
    public void LoadPictureHelper()
    {
        LoadPicture(SaveFileName, ImageCaption);
    }
    /// <summary>
    /// Async helper function to load in image. 
    /// </summary>
    /// <param name="SaveFileName"></param>
    /// <param name="PictureCaption"></param>
    private void LoadPicture(string SaveFileName, string PictureCaption)
    {
        string sendToPath = @"C:\Users\IOh\Documents\ConsoleApp\" + SaveFileName;
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
    /// <summary>
    /// Validator to test if extension is present in user input 
    /// </summary>
    /// <param name="newFile"></param>
    private string extensionExists(string newFile)
    {
        string fileExt = System.IO.Path.GetExtension(newFile); //Retrieves extension

        //IF- no file extension is present, add a default extension. Else, return as is. 
        if (fileExt.Length == 0)
        {
            newFile += ".jfif"; //Default image extension
            return newFile;
        }
        return newFile;
    }
    /// <summary>
    /// Validator to test if recommended extension is used 
    /// </summary>
    /// <param name="extension"></param>
    private bool CheckExtension(string newFile)
    {
        List<string> fileTypesList = new List<string> { ".tif", ".png", ".gif", ".jpg", ".bmp", ".jfif"};
        string fileExt = System.IO.Path.GetExtension(newFile); //Retrieves extension

        //IF- extension is found in list of accepted file types. 
        if (fileTypesList.Contains(fileExt))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Validator to test for illegal character input IE: '#', '=', '|', etc
    /// </summary>
    /// <param name="fileName"></param>
    private bool CheckIllegalCharacter(string fileName)
    {
        //IF- any character in the string is an illegal character, return false. Else, true.
        if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            return false;
        }
        return true;
    }
}

