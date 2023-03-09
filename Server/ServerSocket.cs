using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
     public class ServerSocket
     {
          private readonly Socket     _serverSocket;
          private readonly IPEndPoint _serverEndPoint;
          private          int        _clientsCount;
          private readonly Socket[]   _clients;

          public ServerSocket(String ip, int port)
          {
               IPAddress ipAddress = IPAddress.Parse(ip);

               _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

               _serverEndPoint = new IPEndPoint(ipAddress, port);

               _clients = new Socket[99];
               _clientsCount = 0;
          }

          public void BindAndListen(int queueLimit)
          {
               try
               {
                    _serverSocket.Bind(_serverEndPoint);
                    _serverSocket.Listen(queueLimit);
                    Console.WriteLine($"Listening on {_serverEndPoint.Address}:{_serverEndPoint.Port}");
               }
               catch(Exception ex)
               {
                    Console.WriteLine($"Error binding and listening: {ex.Message}");
               }
          }

          public void AcceptAndReceive()
          {
               while (true)
               {
                    Socket? client;

                    client = acceptClient();

                    if (client != null)
                    {
                         _clients[_clientsCount] = client;
                         _clientsCount++;

                         Thread clientThread = new Thread(() => receiveLoop(client));
                         clientThread.Start();
                    }
               }
               
          }

          private Socket? acceptClient()
          {
               Socket? client = null;
               try
               {
                    client = _serverSocket.Accept();
                    Console.WriteLine($"Client {client.RemoteEndPoint} accepted!");
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
               }
               return client;
          }

          private void receiveLoop(Socket client)
          {
               while (true)
               {
                    try
                    {
                         byte[] buffer = new byte[1024];
                         int bytesCount = client.Receive(buffer);
                         string response = "Message received !";

                         if (bytesCount == 0)
                         {
                              Console.WriteLine($"Client {client.RemoteEndPoint} disconnected :(");
                              return;
                         }

                         string text = Encoding.UTF8.GetString(buffer, 0, bytesCount);
                         Console.WriteLine($"From {client.RemoteEndPoint}: {text}");

                         buffer = Encoding.UTF8.GetBytes(response);
                         client.Send(buffer);
                    }
                    catch (Exception ex)
                    {
                         Console.WriteLine($"Error receiving data from {client.RemoteEndPoint}: {ex.Message}");
                    }
               }
          }
     }
}
