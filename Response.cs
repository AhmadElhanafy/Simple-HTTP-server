using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }

        List<string> headerLines = new List<string>();

        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add("Content-Type: " + contentType + "\r\n");
            headerLines.Add("Content-Length: " + content.Length + "\r\n");
            headerLines.Add("Date: " + DateTime.Now.ToString() + "\r\n");
            headerLines.Add("Location: " + redirectoinPath + "\r\n");
            // TODO: Create the request string
            if (redirectoinPath.Length == 0)
            {
                responseString = GetStatusLine(code)
                    + headerLines[0]
                    + headerLines[1]
                    + headerLines[2]
                    + "\r\n"
                    + content;
            }
            else
            {
                responseString = GetStatusLine(code)
                    + headerLines[0]
                    + headerLines[1]
                    + headerLines[2]
                    + headerLines[3]
                    + "\r\n"
                    + content;
            }
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            
            switch (code) 
            { 
                case (StatusCode.OK):
                    {
                        statusLine = Configuration.ServerHTTPVersion + " " + ((int)StatusCode.OK).ToString() + " " + StatusCode.OK.ToString() + "\r\n";
                        break;
                    }

                case (StatusCode.BadRequest):
                    {
                        statusLine = Configuration.ServerHTTPVersion + " " + ((int)StatusCode.BadRequest).ToString() + " " + StatusCode.BadRequest.ToString() + "\r\n";
                        break;
                    }

                case (StatusCode.InternalServerError):
                    {
                        statusLine = Configuration.ServerHTTPVersion + " " + ((int)StatusCode.InternalServerError).ToString() + " " + StatusCode.InternalServerError.ToString() + "\r\n";
                        break;
                    }

                case (StatusCode.NotFound):
                    {
                        statusLine = Configuration.ServerHTTPVersion + " " + ((int)StatusCode.NotFound).ToString() + " " + StatusCode.NotFound.ToString() + "\r\n";
                        break;
                    }

                case (StatusCode.Redirect):
                    {
                        statusLine = Configuration.ServerHTTPVersion + " " + ((int)StatusCode.NotFound).ToString() + " " + StatusCode.Redirect.ToString() + "\r\n";
                        break;
                    }
            }
            return statusLine;
        }
    }
}
