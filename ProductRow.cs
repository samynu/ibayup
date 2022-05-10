
public class ProductRow
{

    public static class Constants{
        public const string hi_images_upload1 = "hi_images_upload1";
        public const string hi_images_upload2 = "hi_images_upload2";
        public const string hi_images_upload3 = "hi_images_upload3";
        public const string hi_images_upload4 = "hi_images_upload4";
    }

    public string cid {get; set;}  
    public string hw_reg_1 {get; set;}
    public string hw_reg_2 {get; set;}
    public string f_title {get; set;}
    public     string f_desc {get; set;}
    public string f_capacity {get; set;}
    public string f_brand {get; set;}
    public string f_condition {get; set;}
    public string f_video {get; set;}
    public string hw_auct_enabled {get; set;}
    public string hw_exp_days {get; set;}
    public string f_price {get; set;}
    public string f_quantity {get; set;}
    public string hi_images_upload1 {get; set;}
    public string hi_images_upload2 {get; set;}
    public string hi_images_upload3 {get; set;}
    public string hi_images_upload4 {get; set;}
    
    
    public ProductRow(string[] row, string imagePath)
    {
        if(row.Count() < 16) throw new Exception("Product row has incorrect column count");

        this.cid = row[0];
        this.hw_reg_1 = row[1];
        this.hw_reg_2 = row[2];
        this.f_title = row[3];
        this.f_desc = row[4];
        this.f_capacity = row[5];
        this.f_condition = row[6];
        this.f_video = row[7];
        this.hw_auct_enabled = row[8];
        this.hw_exp_days = row[9];
        this.f_price = row[10];
        this.f_quantity = row[11];
        this.hi_images_upload1 = row[12] == "" ? "" : Path.Combine(imagePath,row[12]);
        this.hi_images_upload2 = row[13] == "" ? "" : Path.Combine(imagePath,row[13]);
        this.hi_images_upload3 = row[14] == "" ? "" : Path.Combine(imagePath,row[14]);
        this.hi_images_upload4 = row[15] == "" ? "" : Path.Combine(imagePath,row[15]);
        

    }
}