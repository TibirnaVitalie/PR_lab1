using Client;

ClientSocket client = new ClientSocket();
Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCancelHandler);

client.Connect("127.0.0.1", 9000);

client.SendLoop();


void ConsoleCancelHandler(object sender, ConsoleCancelEventArgs e)
{
     // Set the Cancel property of the ConsoleCancelEventArgs object to true to prevent the application from immediately terminating
     e.Cancel = true;

     client.CloseClientSocket();

     // Call Environment.Exit() to terminate the application
     Environment.Exit(0);
}