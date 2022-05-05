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
         
        
        Console.WriteLine("Response headers: " + response.Headers);


        IEnumerable<string> value; 
 
        if(response.Headers.TryGetValues("Set-Cookie", out value)){
            if(value.Count() <= 0) return false;

            foreach( var val in value)
            {
                Console.WriteLine("cookievalue:" + val);

                if(val.Contains("EC2_L=deleted")) return false;
            }
        }

        var cookies = cookieContainer.GetCookies(baseUri);

        return cookies.Any((c) => c.Name == "HW_SID");

    }

     public async Task<bool> AddPost(ProductRow product)
    {
        MultipartFormDataContent data = new MultipartFormDataContent("----MyAppBoundary" + DateTime.Now.Ticks.ToString("x"));
        
        data.Add(new StringContent("2057677023"), "hw_rid");
        data.Add(new StringContent(product.cid), "cid");        
        data.Add(new StringContent(product.hw_reg_1), "hw_reg_1");
        data.Add(new StringContent(product.hw_reg_2), "hw_reg_2");
        data.Add(new StringContent(product.f_title), "f_title");
        data.Add(new StringContent(product.f_desc), "f_descr");
        data.Add(new StringContent(product.f_capacity), "f_capacity");
        data.Add(new StringContent(product.f_condition), "f_condition");
        data.Add(new StringContent(product.f_video), "f_video");
        data.Add(new StringContent(product.hw_auct_enabled), "hw_auct_enabled");
        data.Add(new StringContent(product.hw_exp_days), "hw_exp_days");
        data.Add(new StringContent(product.f_price), "f_price");
        data.Add(new StringContent(product.f_quantity), "f_quantity");
        data.Add(new StringContent("0"), "is_upl_adv_images");
        
        data.Add(new StringContent("submit"), "go");




        if(product.hi_images_upload1 != "" && product.hi_images_upload1 != null)
        {
            
            var fileContent = await GetByteArrayContent(filePath: product.hi_images_upload1);
            data.Add(fileContent, "hi_images_upload1", Path.GetFileName(product.hi_images_upload1));
        }
        if(product.hi_images_upload2 != "" && product.hi_images_upload2 != null)
        {
            
            var fileContent = await GetByteArrayContent(filePath: product.hi_images_upload2);
            data.Add(fileContent, "hi_images_upload2", Path.GetFileName(product.hi_images_upload2));
        }
        if(product.hi_images_upload3 != "" && product.hi_images_upload3 != null)
        {
            
            var fileContent = await GetByteArrayContent(filePath: product.hi_images_upload3);
            data.Add(fileContent, "hi_images_upload3", Path.GetFileName(product.hi_images_upload3));
        }
        if(product.hi_images_upload4 != "" && product.hi_images_upload4 != null)
        {
            
            var fileContent = await GetByteArrayContent(filePath: product.hi_images_upload4);
            data.Add(fileContent, "hi_images_upload4", Path.GetFileName(product.hi_images_upload4));
        }


        HttpResponseMessage response = await client.PostAsync(addPostUri, data);

        

        var responseStr = await response.Content.ReadAsStringAsync();

        if(responseStr.Contains(postAddedStr)){
            Console.WriteLine(postAddedStr);
            return true;
        }else{
            Console.WriteLine(postFailedStr);
            return false;
        }



    }

    private async Task<ByteArrayContent> GetByteArrayContent(string filePath)
    {
        var file = File.OpenRead(filePath);
        var streamContent = new StreamContent(file);


        return new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
    }
}