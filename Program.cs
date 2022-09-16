using System;
using System.Threading.Tasks;
using System.Net;
//using ConsoleApp6;
/// <summary>
/// Program: Retrieve image from cataas api 
/// </summary>
internal class Program
{
    static async void Main(string[] args)// -o "file.img" -t 
    {
        string saveFileName = "newImage1.jpg"; //Name of file specified by user 
        string imageCaption = "ya"; //Text to be displayed on picture

        try
        {
            //FOR: saving args to variables
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-o")
                {
                    saveFileName = args[i + 1]; //Add null checking 
                }
                if (args[i] == "-t")
                {
                    imageCaption = args[i + 1];
                }
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine("No argument specified: {0}",e.Message);
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
    /// <summary>
    /// Property that will contain the name of the save file the user has specified.
    /// </summary>
    private string _saveFileName;
    public string SaveFileName
    {
        get { return _saveFileName; }
        set
        {
            //IF- the value passes the error checking, save to property. Else, invalid arguments. 
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
    /// <summary>
    /// Property that will contain a caption to be overlayed onto the image specified by the user. 
    /// </summary>
    public string ImageCaption { get; set; }
    /// <summary>
    /// Constructor for this application that sets the file name and image caption and then loading the picture.
    /// </summary>
    public fetchImageFromRestfulApi(string saveFileName, string imageCaption)
    {
        string newFileName = IsExtensionExists(saveFileName);

        SaveFileName = newFileName;
        ImageCaption = imageCaption;
    }
    /// <summary>
    /// Helper function for loading pictures. 
    /// </summary>
    public async Task LoadPictureHelper()
    {
        await LoadPictureAsync(SaveFileName, ImageCaption);
    }
    /// <summary>
    /// Async helper function to load in image. 
    /// </summary>
    /// <param name="SaveFileName"></param>
    /// <param name="PictureCaption"></param>
    private async Task LoadPictureAsync(string SaveFileName, string PictureCaption)
    {
        string dowloadFoldersPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        string sendToPath = dowloadFoldersPath + "\\" + SaveFileName;
        string fetchPicWithoutCaption = @"https://cataas.com/cat";
        string fetchPicWithCaption = "";
        string url = "";

        //IF- url will grab a picture of a cat without captions, else captions input by user will be added. 
        if (PictureCaption.Length <= 0)
        {
            url = fetchPicWithoutCaption;
        }
        else
        {
            fetchPicWithCaption = fetchPicWithoutCaption + @"/says/" + PictureCaption;
            url = fetchPicWithCaption;
        }
        await DownloadPictureAsync(url, sendToPath);
        
    }
    /// <summary>
    /// Download picture from rest api 
    /// </summary>
    /// <param name="newFile"></param>
    private async Task DownloadPictureAsync(string url, string sendToPath)
    {
        using (WebClient client = new WebClient())
        {
            await Task.Run(() => client.DownloadFile(new Uri(url), sendToPath));
        }
    }
    /// <summary>
    /// Validator to test if extension is present in user input 
    /// </summary>
    /// <param name="newFile"></param>
    private string IsExtensionExists(string newFile)
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
        List<string> fileTypesList = PrepValidExtData();
        string fileExt = System.IO.Path.GetExtension(newFile); //Retrieves extension

        //IF- extension is found in list of accepted file types. 
        if (fileTypesList.Contains(fileExt))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// helper function to populate list of valid extensions. 
    /// </summary>
    /// <param name="extension"></param>
    private List<string>PrepValidExtData()
    {
        List<string> fileTypesList = new List<string> { ".tif", ".png", ".gif", ".jpg", ".bmp", ".jfif" };

        return fileTypesList;
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

