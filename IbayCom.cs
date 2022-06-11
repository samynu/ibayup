// See https://aka.ms/new-console-template for more information
using System.Net;

internal class IbayCom
{

    Uri baseUri = new Uri("https://ibay.com.mv");
    String loginUri = "https://ibay.com.mv/index.php";
    String addPostUri = "https://ibay.com.mv/index.php?page=add&cid=324";
    String postAddedStr = "Your Listing has been Submited Successfully";
    String postFailedStr = "Post failed";

    private string userName;
    private string password;

    private  readonly HttpClient client;
    private  HttpClientHandler handler = new HttpClientHandler();
    private CookieContainer cookieContainer = new CookieContainer();

    public IbayCom(string userName, string password)
    {
        this.userName = userName;
        this.password = password;

        handler.CookieContainer = cookieContainer;
        
        client = new HttpClient(handler);

        client.BaseAddress = baseUri;

    }


    public async Task<bool> Login()
    {
        Console.WriteLine("Logging in...");
        //reset cookies
        cookieContainer.SetCookies(baseUri, "");

        var values = new Dictionary<string, string>
        {
            {"page", "login"},
            {"act", "login"},
            {"pwd_md5", ""},
            {"ref_url", "login"},
            {"login", this.userName},
            {"pwd", this.password},

        };

        var content = new FormUrlEncodedContent(values);
        
        var response = await client.PostAsync(loginUri, content);
        

        //something else might be wrong!
        if(response.StatusCode != HttpStatusCode.OK) return false;  
         
        
        // Console.WriteLine("Response headers: " + response.Headers);


        IEnumerable<string> value; 
 
        if(response.Headers.TryGetValues("Set-Cookie", out value)){
            if(value.Count() <= 0) return false;

            foreach( var val in value)
            {
                // Console.WriteLine("cookievalue:" + val);

                if(val.Contains("EC2_L=deleted")) return false;
            }
        }

        var cookies = cookieContainer.GetCookies(baseUri);

        return cookies.Any((c) => c.Name == "HW_SID");

    }



    public async Task<bool> AddPost(Dictionary<string,string> paramList, string imagePath, bool logVerbose)
    {
        MultipartFormDataContent data = new MultipartFormDataContent("----MyAppBoundary" + DateTime.Now.Ticks.ToString("x"));
        
        
        await AddImageData(ProductRow.Constants.hi_images_upload1, paramList,imagePath, data);
        await AddImageData(ProductRow.Constants.hi_images_upload2, paramList,imagePath, data);
        await AddImageData(ProductRow.Constants.hi_images_upload3, paramList,imagePath, data);
        await AddImageData(ProductRow.Constants.hi_images_upload4, paramList,imagePath, data);

        
        
        paramList.ToList().ForEach(p => {
            data.Add(new StringContent(p.Value), p.Key);
            if(logVerbose) PrettyLog.LogSilent($"add- {p.Key} : {p.Value}");
            });



        data.Add(new StringContent("2057677023"), "hw_rid");        
        data.Add(new StringContent("0"), "is_upl_adv_images");        
        data.Add(new StringContent("submit"), "go");


        

        HttpResponseMessage response = await client.PostAsync(addPostUri, data);

        

        var responseStr = await response.Content.ReadAsStringAsync();

        
        if(responseStr.Contains(postAddedStr)){
            
            return true;
        }else{
            
            return false;
        }



    }

    private async Task AddImageData(string key, Dictionary<string, string> paramList, string imagePath, MultipartFormDataContent data)
    {
        var fileName = paramList.GetValueOrDefault(key);
        if(fileName != null && fileName != ""){

            var file = Path.Combine(imagePath,fileName);
            var imageFile = new FileInfo(file);

            if(imageFile.Exists)
            {
                
                var fileContent = await GetByteArrayContent(filePath: imageFile.FullName);
                data.Add(fileContent, key, Path.GetFileName(imageFile.FullName));
            }
        }

        paramList.Remove(key);
    }


    private async Task<ByteArrayContent> GetByteArrayContent(string filePath)
    {
        var file = File.OpenRead(filePath);
        var streamContent = new StreamContent(file);


        return new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
    }
}