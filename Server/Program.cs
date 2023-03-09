using Server;

ServerSocket server = new ServerSocket("127.0.0.1", 9000);

server.BindAndListen(15);
server.AcceptAndReceive();
