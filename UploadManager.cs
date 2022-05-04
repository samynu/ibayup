

public class UploadManager
{
    private AppArguments appArguments;

    public UploadManager(AppArguments appArguments)
    {
        this.appArguments = appArguments;

        //TODO: ensure required arguments.
    }


    async void ProcessProductRecords()
    {
        
        var ibayCom = new IbayCom(userName:appArguments.UserName, password:  appArguments.Password);
        var result = await ibayCom.Login();

        if(result){

            Console.WriteLine("Logon Success!");
        }else{
            Console.WriteLine("Logon failed!");
            return;
        }


        //process file records

        string line;

        FileStream stream = new FileStream(appArguments.File, FileMode.Open);
        StreamReader streamReader = new StreamReader(stream);

        while((line = streamReader.ReadLine()) != null){
            Console.WriteLine("Read line: " + line);

            //POST to ibay
            var productRow = new ProductRow(line.Split(','), appArguments.ImagePath);
            ibayCom.AddPost(productRow);

        }

        streamReader.Close();



        





        // var postResult = await ibayCom.AddPost
        //     (
        //         hw_rid: "2057677023",
        //         cid: "324", 
        //         hw_region_id: "",
        //         hw_region_upd: "",
        //         hw_reg_1: "11",
        //         hw_reg_2: "100",
        //         f_title: "Western Digital 1TB WD Blue SN550 NVMe Internal SSD - Gen3 x4 PCIe 8Gb/s, M.2 22",
        //         f_desc: desc,
        //         f_capacity: "1 TB and More",
        //         f_condition: "New",
        //         f_video: "",
        //         hw_auct_enabled: "0",
        //         hw_exp_days: "5",
        //         f_price: "2900",
        //         f_quantity: "1",

        //         hi_images_upload1: @".\wd.jpg",
        //         hi_images_upload2: image2Data,
        //         hi_images_upload3: image3Data,
        //         hi_images_upload4: image4Data,
        //         term: "",
        //         go: "submit"
        //     );
    }


}