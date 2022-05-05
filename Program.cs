// See https://aka.ms/new-console-template for more information
using Mono.Options;

Console.WriteLine("*Ibay Updload*");
Console.WriteLine("--------------");

var appArguments = new AppArguments();


appArguments.ParseCommand(args);

var uploadManager = new UploadManager(appArguments);


await uploadManager.ProcessProductRecords();










