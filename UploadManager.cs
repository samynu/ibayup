using System.Text.Json;

public class UploadManager
{
    private AppArguments appArguments;

    public UploadManager(AppArguments appArguments)
    {
        this.appArguments = appArguments;

        //TODO: ensure required arguments.
    }


    public async Task ProcessProductRecords()
    {
        
        var ibayCom = new IbayCom(userName:appArguments.UserName, password:  appArguments.Password);
        var result = await ibayCom.Login();
        // var result = true;

        if(result){

            Console.WriteLine("Logon Success!");
        }else{
            Console.WriteLine("Logon failed!");
            return;
        }


        //process file records

        string line;

        try{

            FileStream stream = new FileStream(appArguments.File, FileMode.Open);
            StreamReader streamReader = new StreamReader(stream);
            Console.WriteLine("Product file: " + appArguments.File + " read success");

            


            while((line = streamReader.ReadLine()) != null){
                Console.WriteLine("Reading line: " + line);

                //POST to ibay
                try{
                    var productRow = new ProductRow(line.Split(','), appArguments.ImagePath);
                    var jsonOptions = new JsonSerializerOptions {WriteIndented = true};
                    var jsonProduct = JsonSerializer.Serialize(productRow, jsonOptions);
                    Console.WriteLine("Posting item: " + jsonProduct);
                    await ibayCom.AddPost(productRow);

                }catch(Exception ex){
                    Console.WriteLine("Error: " + ex.Message);
                }

            }
            streamReader.Close();
        }catch(FileNotFoundException ex){
            Console.WriteLine("File " + appArguments.File + " not found!");
            return;
        }
    }


}