﻿using System.Net.Sockets;
using System.Text;

namespace ConsoleChat.Client;

public class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Введите ваше имя: ");
        var name = Console.ReadLine();

        var client = new TcpClient("127.0.0.1", 8888);
        var stream = client.GetStream();

        var nameBytes = Encoding.UTF8.GetBytes(name);
        await stream.WriteAsync(nameBytes, 0, nameBytes.Length);

        _ = Task.Run(() => ReceiveMessageAsync(stream));

        while (true)
        {
            var message = Console.ReadLine();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }

    private static async Task ReceiveMessageAsync(NetworkStream stream)
    {
        var buffer = new byte[1024];
        while (true)
        {
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            Console.WriteLine(message);
        }
    }
}