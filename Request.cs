using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace HTTPServer
{
    class Request
    {
        string[] requestLines;
        string[] requestLine;
        readonly string[] str = {"\r\n"};
        readonly string requestString;
        public string relativeURI;
        
        public Request(string requestString)
        {
            this.requestString = requestString;
        }
      
        public bool ParseRequest()
        {
            //TODO: parse the receivedRequest using the \r\n delimeter   
            // Parse Request line
            requestLines = requestString.Split(str, StringSplitOptions.None);
            requestLine = requestLines[0].Split(' ');

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length >= 3)
            {
                //Validate request line
                bool isparserequestline = ParseRequestLine();
                //Validate URI
                bool validUri = ValidateIsURI(requestLine[1]);
                // Validate blank line exists
                bool isBlanckLine = ValidateBlankLine();
                //load headerlines
                //LoadHeaderLines();
                if (validUri && isparserequestline && isBlanckLine)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else
                return false;
        }

        private bool ParseRequestLine()
        {
            try
            {
                relativeURI = requestLine[1].Trim().Remove(0,1);
                if (requestLine[0].Trim() != "GET")
                {
                    return false;
                }

                if (requestLine[2].Trim() != "HTTP/1.1" && requestLine[2].Trim() != "HTTP/1.0" && requestLine[2].Trim() != "HTTP/0.9")
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        private bool ValidateIsURI(string uri)
        {
            try
            {
                if (Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
                {
                    string[] curi = uri.Split('.');
                    if (curi[1] != "html")
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        private bool ValidateBlankLine()
        {
            if (requestLines[requestLines.Length - 2] == "")
                return true;
            else
                return false;
        }
    }
}