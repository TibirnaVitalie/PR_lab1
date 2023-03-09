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
          private readonly Socket _clientSocket;

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
                         Console.Write("Your message: ");
                         string text = Console.ReadLine() ?? "";

                         if(text == "q")
                         {
                              _clientSocket.Shutdown(SocketShutdown.Both);
                              _clientSocket.Close();
                              Console.Write("Disconnected!");
                              return;
                         }

                         byte[] bytesData = Encoding.UTF8.GetBytes(text);
                         _clientSocket.Send(bytesData);

                         byte[] bytesResponse = new byte[1024];
                         int bytesReceived = _clientSocket.Receive(bytesResponse);
                         string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesReceived);
                         Console.WriteLine("Response from Server: " + response);
                    }
                    catch(Exception ex)
                    {
                         Console.WriteLine($"Error sending: {ex.Message}");
                    }
               }
          }
     }
}
