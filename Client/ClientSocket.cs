using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
     internal class ClientSocket
     {
          readonly Socket _clientSocket;

          public ClientSocket()
          {
               _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          }

          public void Connect(string remoteIP, int remotePort)
          {
               IPAddress ipAddress = IPAddress.Parse(remoteIP);
               IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, remotePort);
               try
               {
                    _clientSocket.Connect(remoteEndPoint);
                    Console.WriteLine($"Connected to {remoteEndPoint.Address}");
                    Console.WriteLine("Say \"q\" to disconnect\n");
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error connecting: {ex.Message}");
               }
          }

          public void SendLoop()
          {
               while (true)
               {
                    try
                    {
                         Console.Write("\nYour message: ");
                         string text = Console.ReadLine() ?? "";

                         if(text == "q")
                         {
                              CloseClientSocket();
                              return;
                         }

                         byte[] bytesData = Encoding.UTF8.GetBytes(text);
                         _clientSocket.Send(bytesData);

                         byte[] bytesResponse = new byte[1024];
                         int bytesReceived = _clientSocket.Receive(bytesResponse);
                         string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesReceived);
                         Console.WriteLine(response);
                    }
                    catch(Exception ex)
                    {
                         Console.WriteLine($"Error sending: {ex.Message}");
                    }
               }
          }

          public void CloseClientSocket()
          {
               _clientSocket.Shutdown(SocketShutdown.Both);
               _clientSocket.Close();
               Console.Write("\nDisconnected!");
          }
     }
}
