// See https://aka.ms/new-console-template for more information
using Mono.Options;

Console.WriteLine("Ibay Updload~");

var appArguments = new AppArguments();


appArguments.ParseCommand(args);

var uploadManager = new UploadManager(appArguments);













