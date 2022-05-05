// See https://aka.ms/new-console-template for more information
using Mono.Options;

Console.WriteLine("*Ibay Updload*");
Console.WriteLine("--------------");

var appArguments = new AppArguments();


var success = appArguments.ParseCommand(args);

if(success){
    var uploadManager = new UploadManager(appArguments);
    await uploadManager.ProcessProductRecords();

}










