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
            
            PrettyLog.LogSuccess("Logon Success!");
        }else{
            PrettyLog.LogError("Logon failed!");
            return;
        }


        await ReadExcelFile(appArguments.File);

        //process file records

        // await ReadCsvFile(appArguments.File);

        
    }
    


    private async Task ReadExcelFile(string file)
    {
        FileInfo excelFile = new FileInfo(appArguments.File);
        if (!excelFile.Exists || excelFile.Extension != ".xlsx")
        {
            PrettyLog.LogError("File does not exist:" + excelFile.FullName + $"ext:{excelFile.Extension}");
            return;
            
        }
        
            Dictionary<string,string> paramList = new Dictionary<string, string>();
            
            List<string> headerList = new List<string>();

            ISheet sheet;
            using (var stream = new FileStream(excelFile.FullName, FileMode.Open))
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(1);   //Row Zero(0) contains Readable titles
                int cellCount = headerRow.LastCellNum;

                if (cellCount <= 5 )
                {
                    PrettyLog.LogError("Error no data columns found");
                    return;
                }
                
                for (int j = 5; j < cellCount; j++)     //First 5 cols for other use
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) 
                    {
                        PrettyLog.LogError($"Cant have empty header cell: {j+1}");
                        return;
                    }
                    headerList.Add(cell.ToString());
                     
                }

                
                for (int i = (sheet.FirstRowNum + 2); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) 
                    {PrettyLog.LogWarning($"Skipping empty row {i+1}"); continue;}

                    ICell cell = row.GetCell(4);      //Designate column #5 for publish-yes/no
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) 
                    {
                        PrettyLog.LogWarning($"Empty publish(Y/N) cell reached, Ending at row #: {i+1}");
                        return;
                    }

                    if(cell.ToString().ToLower() != "yes") 
                    {
                        PrettyLog.LogWarning($"Skipping row #: {i+1}");
                        continue;  //Skip record
                    }


                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = 5; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                paramList.Add(headerList[j-5], row.GetCell(j).ToString());
                            }
                        }
                    }
                    if(paramList.Count>0){

                        // paramList.ToList().ForEach(r => Console.WriteLine(r.ToString()));
                        var success = await ibayCom.AddPost(paramList, appArguments.ImagePath, appArguments.Verbose);

                        if(success) PrettyLog.LogSuccess($"Post Added row # {i+1} Successfully!");
                        else PrettyLog.LogError($"Post row # {i+1} Failed");
                        if(appArguments.TimeOut > 0) 
                        {
                            PrettyLog.LogWarning($"Sleep for {appArguments.TimeOut} seconds");
                            Thread.Sleep(appArguments.TimeOut * 1000);
                        }
                    }
                    paramList.Clear(); 
                }
            }
            


    }
}