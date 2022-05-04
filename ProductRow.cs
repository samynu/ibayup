
public class ProductRow
{    
    public string cid;    
    public string hw_reg_1;
    public string hw_reg_2;
    public string f_title;
    public     string f_desc;
    public string f_capacity;
    public string f_condition;
    public string f_video;
    public string hw_auct_enabled;
    public string hw_exp_days;
    public string f_price;
    public string f_quantity;
    public string hi_images_upload1;
    public string hi_images_upload2;
    public string hi_images_upload3;
    public string hi_images_upload4;
    
    
    public ProductRow(string[] row, string imagePath)
    {
        if(row.Count() < 16) throw new Exception("CSV file error");

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
        this.hi_images_upload1 = imagePath + row[12];
        this.hi_images_upload2 = imagePath + row[13];
        this.hi_images_upload3 = imagePath + row[14];
        this.hi_images_upload4 = imagePath + row[15];

    }
}