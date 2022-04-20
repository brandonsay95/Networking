// See https://aka.ms/new-console-template for more information
using NetworkLib;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");


Helpers.LoadLogFile();
Task.Run(() => {

    TcpListener listener = new TcpListener(IPAddress.Any, 25565);
    listener.Start();
    int connectionId = 0;
    while (true) {
        var client = listener.AcceptTcpClient();
        var connection = new TcpConnection<PacketIdentifier>(connectionId, client);
        connection.OnConnected += (s, e) => {
            Console.WriteLine($@"Client connected");

            Task.Run(() =>
            {
                int x = 0; int y = 0; int z = 0;
                while (true)
                {
                    y++;

                    var movePacket = new WritePacket<PacketIdentifier>(PacketIdentifier.Position);
                    movePacket.WriteInt(x);
                    movePacket.WriteInt(y);
                    movePacket.WriteInt(z);
                    e.WritePacket(movePacket);
                    Thread.Sleep(150);
                }

            });


            //var packet = new WritePacket<PacketIdentifier>(PacketIdentifier.Welcome);
            //packet.WriteString($@"Welcome to the server");
            //e.WritePacket(packet);
        };
        Task.Run(async () => 
        await connection.ConnectAsync());
    }
});


Task.Run(() => {
    byte[] bytes = new byte[255];

    var connection = new TcpConnection<PacketIdentifier>(0,"127.0.0.1",25565);
    connection.PacketReceived += (s, e) => {
        Console.Write($@"Packet Received({e.PacketId})");
        if(e.PacketId == PacketIdentifier.Position)
        {
            int x, y, z;
            x = e.Packet.ReadInt();
            y = e.Packet.ReadInt();
            z = e.Packet.ReadInt();
            Console.Write($"({x},{y},{z})\n");
        }
        //else if ( e.PacketId == PacketIdentifier.Welcome)
        //{
        //    string message = e.Packet.ReadString();
        //    Console.Write($"\"{message}\"\n");
        //}

        else
        {
            Console.Write($"RAW({Helpers.ConvertPacketEventToTextData(e)})\n");
        }
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

