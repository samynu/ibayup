// See https://aka.ms/new-console-template for more information
using System.Net;

internal class IbayCom
{

    Uri baseUri = new Uri("https://ibay.com.mv");
    String loginUri = "https://ibay.com.mv/index.php";
    String addPostUri = "https://ibay.com.mv/index.php?page=add&cid=324";
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



        if(response.StatusCode != HttpStatusCode.OK) return false;  //uknown error


        // Console.WriteLine(await response.Content.ReadAsStringAsync());
        
        
        Console.WriteLine("Response headers: " + response.Headers);


        IEnumerable<string> value; 
 
        if(response.Headers.TryGetValues("Set-Cookie", out value)){
            foreach( var val in value)
            {
                Console.WriteLine("cookievalue:" + val);

                if(val.Contains("EC2_L=deleted")) return false;
            }
        }

        var cookies = cookieContainer.GetCookies(baseUri);

        return cookies.Any((c) => c.Name == "HW_SID");

    }

     public async Task<bool> AddPost(
         string hw_rid, 
         string cid, 
         string hw_region_id, 
         string hw_region_upd, 
         string hw_reg_1, 
         string hw_reg_2, 
         string f_title, 
         string f_desc, 
         string f_capacity, 
         string f_condition, 
         string f_video, 
         string hw_auct_enabled, 
         string hw_exp_days, 
         string f_price, 
         string f_quantity, 
         string is_upl_adv_images, 
         string hi_images_upload1, 
         string hi_images_upload2, 
         string hi_images_upload3, 
         string hi_images_upload4, 
         string term, 
         string go)
    {
        MultipartFormDataContent data = new MultipartFormDataContent("----MyAppBoundary" + DateTime.Now.Ticks.ToString("x"));

        data.Add(new StringContent(hw_rid), "hw_rid");
        data.Add(new StringContent(cid), "cid");
        // data.Add(new StringContent(hw_region_id), "hw_region_id");
        // data.Add(new StringContent(hw_region_upd), "hw_region_upd");
        data.Add(new StringContent(hw_reg_1), "hw_reg_1");
        data.Add(new StringContent(hw_reg_2), "hw_reg_2");
        data.Add(new StringContent(f_title), "f_title");
        data.Add(new StringContent(f_desc), "f_descr");
        data.Add(new StringContent(f_capacity), "f_capacity");
        data.Add(new StringContent(f_condition), "f_condition");
        // data.Add(new StringContent(f_video), "f_video");
        data.Add(new StringContent(hw_auct_enabled), "hw_auct_enabled");
        data.Add(new StringContent(hw_exp_days), "hw_exp_days");
        data.Add(new StringContent(f_price), "f_price");
        data.Add(new StringContent(f_quantity), "f_quantity");
        data.Add(new StringContent(is_upl_adv_images), "is_upl_adv_images");
        // data.Add(new StringContent(hi_images_upload1), "hi_images_upload1");
        // data.Add(new StringContent(hi_images_upload2), "hi_images_upload2");
        // data.Add(new StringContent(hi_images_upload3), "hi_images_upload3");
        // data.Add(new StringContent(hi_images_upload4), "hi_images_upload4");
        // data.Add(new StringContent(term), "term[]");
        data.Add(new StringContent(go), "go");

        var file = File.OpenRead(hi_images_upload1);
        var streamContent = new StreamContent(file);

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

        data.Add(fileContent, "hi_images_upload1", Path.GetFileName(hi_images_upload1));


        HttpResponseMessage response = await client.PostAsync(addPostUri, data);

        

        Console.WriteLine(await response.Content.ReadAsStringAsync());


        return true;

    }
}