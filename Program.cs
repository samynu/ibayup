// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var desc = "<p><span style=\"background-color:rgb(255, 255, 255); color:rgb(0, 0, 0); font-family:roboto,robotodraft,helvetica,arial,sans-serif; font-size:13px\">Western Digital 1TB WD Blue SN550 NVMe Internal SSD - Gen3 x4 PCIe 8Gb/s, M.2 2280, 3D NAND, Up to 2,400 MB/s</span></p><p>&nbsp;</p><p><span style=\"background-color:rgb(255, 255, 255); color:rgb(0, 0, 0); font-family:roboto,robotodraft,helvetica,arial,sans-serif; font-size:13px\">Call 7835405</span></p>";


string image1Data = "";
string image2Data = "";
string image3Data = "";
string image4Data = "";


var ibayCom = new IbayCom(userName:"samynu", password: "saspatis113");

var result = await ibayCom.Login();


Console.WriteLine("Logon Success: " + result);


var postResult = await ibayCom.AddPost
    (
        hw_rid: "2057677023",
        cid: "324", 
        hw_region_id: "",
        hw_region_upd: "",
        hw_reg_1: "11",
        hw_reg_2: "100",
        f_title: "Western Digital 1TB WD Blue SN550 NVMe Internal SSD - Gen3 x4 PCIe 8Gb/s, M.2 22",
        f_desc: desc,
        f_capacity: "1 TB and More",
        f_condition: "New",
        f_video: "",
        hw_auct_enabled: "0",
        hw_exp_days: "5",
        f_price: "2900",
        f_quantity: "1",
        is_upl_adv_images: "0",
        hi_images_upload1: @".\wd.jpg",
        hi_images_upload2: image2Data,
        hi_images_upload3: image3Data,
        hi_images_upload4: image4Data,
        term: "",
        go: "submit"
    );
    