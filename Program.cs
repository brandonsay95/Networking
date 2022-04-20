// See https://aka.ms/new-console-template for more information
using NetworkLib;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");
var debugMode = true;
Helpers.LoadLogFile();
Packet.SignLength = SignDigit.Short;
Task.Run(() => {

    TcpListener listener = new TcpListener(IPAddress.Any, 25565);
    listener.Start();
    int connectionId = 0;
    while (true) {
        var client = listener.AcceptTcpClient();
        var connection = new TcpConnection<PacketIdentifier>(connectionId, client);
        connection.OnConnected += (s, e) => {
            Console.WriteLine($@"Client connected");
            e.WritePacket(
                new WritePacket<PacketIdentifier>(PacketIdentifier.Welcome)
                    .WriteUShort((ushort)e.ConnectionId)
                );
        };
        Task.Run(async () => 
        await connection.ConnectAsync());
    }
});


Task.Run(() => {
    byte[] bytes = new byte[255];

    var connection = new TcpConnection<PacketIdentifier>(0,"127.0.0.1",25565);
    Player player = new Player();
    connection.PacketReceived += (s, e) => {
        //Console.Clear();
        Console.Write($@"Packet Received({e.PacketId})");
        ConsoleColor baseColor = Console.ForegroundColor;
        if (e.PacketId == PacketIdentifier.Position)
        {
            int x, y, z;
            x = e.Packet.ReadInt();
            y = e.Packet.ReadInt();
            z = e.Packet.ReadInt();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"({x},{y},{z})\n");
        }
        else if (e.PacketId == PacketIdentifier.Welcome)
        {
            int connectionId = e.Packet.ReadUShort();
            player.PlayerId = connectionId;
            Console.WriteLine($"Connection Established with PlayerId {connectionId}\n");
        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"RAW({Helpers.ConvertPacketEventToTextData(e, true)})\n");
        }
        if (debugMode)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"    ({Helpers.ConvertPacketEventToTextData(e, true)})\n");

        }
        Console.ForegroundColor = baseColor;
    };
    Task.Run(async () =>
       await connection.ConnectAsync());


});
Task.Run(() => {
    while (true)
    {
        Helpers.SaveLogTick();
        Thread.Sleep(500);
    }

});

Console.Read();


public enum PacketIdentifier
{
    None=0,
    Welcome=1,
    Position=2
}

public class Player{
    public int PlayerId { get; set; }
}