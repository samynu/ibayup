using System.Text.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class UploadManager
{
    private AppArguments appArguments;
    private IbayCom ibayCom;

    public UploadManager(AppArguments appArguments)
    {
        this.appArguments = appArguments;

        //TODO: ensure required arguments.

        this.ibayCom = new IbayCom(userName:appArguments.UserName, password:  appArguments.Password);
    }


    public async Task ProcessProductRecords()
    {
        

        var result = await ibayCom.Login();
        // var result = true;

        if(result){

            Console.WriteLine("Logon Success!");
        }else{
            Console.WriteLine("Logon failed!");
            return;
        }


        await ReadExcelFile(appArguments.File);

        //process file records

        // await ReadCsvFile(appArguments.File);

        
    }

    private async Task ReadCsvFile(string file)
    {
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

    private async Task ReadExcelFile(string file)
    {
        FileInfo excelFile = new FileInfo(appArguments.File);
        if (!excelFile.Exists || excelFile.Extension != ".xlsx")
        {
            throw new FileNotFoundException("File does not exist:" + excelFile.FullName + $"ext:{excelFile.Extension}");
            
        }
        
            Dictionary<string,string> paramList = new Dictionary<string, string>();
            
            List<string> headerList = new List<string>();

            ISheet sheet;
            using (var stream = new FileStream(excelFile.FullName, FileMode.Open))
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) throw new Exception($"Cant have empty header cell: {j}");
                    {
                        headerList.Add(cell.ToString());
                    } 
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                paramList.Add(headerList[j], row.GetCell(j).ToString());
                            }
                        }
                    }
                    if(paramList.Count>0){

                        // paramList.ToList().ForEach(r => Console.WriteLine(r.ToString()));
                        await ibayCom.AddPost(paramList);
                    }
                    paramList.Clear(); 
                }
            }
            


    }
}