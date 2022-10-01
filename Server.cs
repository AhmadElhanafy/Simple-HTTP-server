using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        readonly Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ip);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(500);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);
                ThreadPool.QueueUserWorkItem(HandleConnection, clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket ClientSocket = (Socket)(obj);
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            ClientSocket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] data = new byte[1024];
                    Request Req;
                    int receivedLength = ClientSocket.Receive(data);
                    string ReqString = Encoding.ASCII.GetString(data, 0, receivedLength);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        Console.WriteLine("Client: {0} ended the connection", ClientSocket.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    else 
                    {
                        Req = new Request(ReqString);
                    }
                    // TODO: Call HandleRequest Method that returns the response
                    Response Rep = HandleRequest(Req);
                    // TODO: Send Response back to client
                    data = Encoding.ASCII.GetBytes(Rep.ResponseString);
                    ClientSocket.Send(data);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }
            // TODO: close client socket
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            Response Rep;
            string Content;
            string RedierctPage;
            string PageName;
            try
            {
                //throw new NotImplementedException();
                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    Content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    Rep = new Response(StatusCode.BadRequest, "text/html", Content, string.Empty);
                    return Rep;
                    throw new FormatException();
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                PageName = Configuration.RootPath + request.relativeURI;
                //TODO: check for redirect
                RedierctPage = GetRedirectionPagePathIFExist(request.relativeURI);
                if (RedierctPage.Length != 0) 
                {
                    Content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                    Rep = new Response(StatusCode.Redirect, "text/html", Content, RedierctPage);
                    return Rep;
                }
                //TODO: check file exists
                if (!File.Exists(PageName))
                {
                    Content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    Rep = new Response(StatusCode.NotFound, "text/html", Content, string.Empty);
                    return Rep;
                    throw new FileNotFoundException();
                }
                //TODO: read the physical file
                Content = LoadDefaultPage(request.relativeURI);
                // Create OK response
                Rep = new Response(StatusCode.OK, "text/html", Content, RedierctPage);
                return Rep;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error.
                Content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                Rep = new Response(StatusCode.InternalServerError, "text/html", Content, string.Empty);
                return Rep;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            foreach (KeyValuePair<string, string> entry in Configuration.RedirectionRules)
            {
                if (entry.Key == relativePath)
                {
                    return entry.Value;
                }
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Configuration.RootPath + defaultPageName;
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            // else read file and return its content
            try 
            {
                StreamReader sr = new StreamReader(filePath);
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary
                StreamReader sr = new StreamReader(filePath);
                string line;
                string[] arr;

                while ((line = sr.ReadLine()) != null)
                {
                    arr = line.Split(',');
                    Configuration.RedirectionRules.Add(arr[0], arr[1]);
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
            }
        }
    }
}
