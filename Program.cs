using System.IO;
using System;

namespace HTTPServer
{
    class Program
    {
        static readonly string path = "redirectionRules.txt";
        static void Main()
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            Server HTTPServer = new Server(1000, path);
            // 2) Start Server
            HTTPServer.StartServer(); 
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html, it redirects me to aboutus2
            if (!File.Exists(path)) 
            {
                _ = new StreamWriter(path);
            }
        }
         
    }
}
