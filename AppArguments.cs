

using Mono.Options;

public class AppArguments
{
    public string? UserName {get;set;} = null;
    public string? Password { get; set; } = null;
    public string File { get; set; } = @"products.csv";
    public string ImagePath { get; set; } = @".\";



    public void ParseCommand(string[] args)
    {


        bool show_help = false;
        
        var p = new OptionSet () {
            { "u|user=", "ibay {Username}",
            v => {
                    if (v == null || v == "")
                            throw new OptionException ("username is required", 
                                    "-u");
                        UserName = v;
            } },
            { "p|pass=", "ibay {Password}.",
            v => {
                    if (v == null)
                            throw new OptionException ("password is required", 
                                    "-p");
                        Password = v;
                
            }},            
            { "f|fileName=", "The CSV {File} to process (default: products.csv).",
            v => File = v },
            { "i|imageDir=", "Set the Path to image directory. (default: current directory).",
            v => ImagePath = v },
            { "h|help",  "show help", 
            v => show_help = v != null },
        };

        List<string> extra;
        try {
            extra = p.Parse (args);
            if (UserName == null || UserName == "")
                throw new OptionException ("username and password is required", "-u");
            if (Password == null || Password == "")
                throw new OptionException ("username and password is required", "-p");

        }
        catch (OptionException e) {
            Console.Write ("ibayup: ");
            Console.WriteLine (e.Message);
            ShowHelp(p);
            return;
        }


        if (show_help) {
            ShowHelp (p);
            return;
        }

    }


    private void ShowHelp (OptionSet p)
    {
        Console.WriteLine ("Usage: ibay -u {username} -p {password} \n");
        
        Console.WriteLine ("Options:");
        p.WriteOptionDescriptions (Console.Out);
    }
}