// See https://aka.ms/new-console-template for more information
using Module1Ep1;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");



Task.Run(() => {
    byte[] bytes = Helpers.ConvertStringToBytes("Welcome To The Server");

    TcpListener listener = new TcpListener(IPAddress.Any, 25565);
    listener.Start();
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    Console.WriteLine($"Server Sent Data:{Helpers.ConvertBytesToDisplayString(bytes, 0, bytes.Length)}");

    stream.Write(bytes, 0, bytes.Length);

});


Task.Run(() => {
    byte[] bytes = new byte[255];

    var client = new TcpClient();
    client.Connect("127.0.0.1", 25565);
    var stream = client.GetStream();
    var readBytes = stream.Read(bytes, 0, bytes.Length);
    Console.WriteLine($"Client Received Data:{Helpers.ConvertBytesToDisplayString(bytes, 0, readBytes)}");


});

Console.Read();