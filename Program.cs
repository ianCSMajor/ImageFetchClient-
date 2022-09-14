// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using System.Net;
//using ConsoleApp6;

internal class Program
{
    static void Main(string[] args)
    {
        string catCaption = "yo";
        string saveFileName = "file.jpg";

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(@"https://cataas.com/cat"), @"C:\Users\IOh\Documents\IOh\image1.jfif");
 
        }

        APIHelper.InitializeClient();
        ProcessImage processImage;

        string source = @"https://cataas.com/cat";
       // FileInfo fileInfo = new FileInfo(source);

        //File.Create(source);

        //program execution starts from here
        Console.WriteLine("Total Arguments: {0}", args.Length);

        Console.Write("Arguments: ");

        for(int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-o")    
            {
                saveFileName = args[i + 1];
            }
            if (args[i] == "-t")
            {
                catCaption = args[i + 1];
            }
        }

        LoadImage(saveFileName, catCaption);

        Console.WriteLine("Save Name: " + saveFileName);
        Console.WriteLine("Caption: " + catCaption);


    }
    private static async Task LoadImage(string saveFileName, string pictureCaption)
    {
        ProcessImage processImage = new ProcessImage(saveFileName, pictureCaption);
        var picture = await processImage.LoadPicture();

        Console.WriteLine("Save Name: " + saveFileName);
        Console.WriteLine("Caption: " + pictureCaption);
        string wow = "";

      //  var uri = new Uri(picture.Img, UriKind.Absolute);

    }

}
public static class APIHelper
{
    //Client that will interact with API 
    public static HttpClient HttpClient { get; set; }

    public static void InitializeClient()
    {
        //Instantiating http client
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Accept.Clear();

        //Adding a header to get json. 
        HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }
}
public class ProcessImage
{
    public string SaveFileName { get; set; }
    public string PictureCaption { get; set; }
    public ProcessImage(string saveFileName, string pictureCaption)
    {
        SaveFileName = saveFileName;
        PictureCaption = pictureCaption;
    }
    public async Task<PictureModel> LoadPicture()
    {
        string fetchPicUri = @"/cat/:tag/says/" + PictureCaption;
        if (SaveFileName.Length <= 0)
        {
            throw new Exception();
        }

        string url = "";

        //IF- 
        if(PictureCaption.Length <= 0)
        {
            url = @"https://cataas.com/cat/";
        }
        else
        {
            url = fetchPicUri;
        }

        //Open new request and await response 
        using (HttpResponseMessage response = await APIHelper.HttpClient.GetAsync(url))
        {
            if(response.IsSuccessStatusCode)
            { 
                //Takes json data and convert to type given. 
                PictureModel picture = await response.Content.ReadAsAsync<PictureModel>();

                return picture;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        //Close the stream 
    }
}
public class PictureModel
{
    public string Picture { get; set; }

}