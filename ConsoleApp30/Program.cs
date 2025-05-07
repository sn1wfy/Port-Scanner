using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Port scanner | Sn1wfy";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ██████╗  ██████╗ ███╗   ██╗███╗   ███╗ █████╗ ██████╗ 
 ██╔═══██╗██╔═══██╗████╗  ██║████╗ ████║██╔══██╗██╔══██╗
 ██║   ██║██║   ██║██╔██╗ ██║██╔████╔██║███████║██║  ██║
 ██║   ██║██║   ██║██║╚██╗██║██║╚██╔╝██║██╔══██║██║  ██║
 ╚██████╔╝╚██████╔╝██║ ╚████║██║ ╚═╝ ██║██║  ██║██████╔╝
  ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═╝     ╚═╝╚═╝  ╚═╝╚═════╝ 
       Multi-Threaded Port Scanner - Sn1wfy
");
        Console.ResetColor();

        string targetIP;
        do
        {
            Console.Write("Enter target IP: ");
            targetIP = Console.ReadLine();
        } while (!IsValidIPAddress(targetIP));

        int startPort = GetValidPort("Enter start port (default 1): ", 1);
        int endPort = GetValidPort("Enter end port (default 1024): ", 1024);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nScanning {targetIP} from port {startPort} to {endPort}...\n");
        Console.ResetColor();

        
        int maxThreads = 50; 
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThreads);

        var tasks = new Task[endPort - startPort + 1];
        int taskIndex = 0;

       
        List<int> openPorts = new List<int>();

        for (int port = startPort; port <= endPort; port++)
        {
            int p = port;
            tasks[taskIndex++] = Task.Run(async () =>
            {
                await semaphore.WaitAsync(); 
                await ScanPort(targetIP, p, openPorts);
                semaphore.Release();
            });
        }

        await Task.WhenAll(tasks); 

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nScan Complete!");
        Console.Beep();

        Console.ForegroundColor = ConsoleColor.Green;
        if (openPorts.Count > 0)
        {
            Console.WriteLine("\nOpen Ports:");
            foreach (var openPort in openPorts)
            {
                Console.WriteLine($"Port {openPort} is OPEN");
            }
        }
        else
        {
            Console.WriteLine("\nNo open ports found.");
        }

        Console.ResetColor();
    }

    static async Task ScanPort(string ip, int port, List<int> openPorts)
    {
        try
        {
            using (TcpClient client = new TcpClient())
            {
                client.ReceiveTimeout = 500;  
                client.SendTimeout = 500;    
                var connectionTask = client.ConnectAsync(ip, port);  

                var timeoutTask = Task.WhenAny(connectionTask, Task.Delay(2000)); 

                if (await timeoutTask == connectionTask) 
                {
                    lock (openPorts) 
                    {
                        openPorts.Add(port);  
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[✔] Port {port} is OPEN");
                    Console.ResetColor();
                }
                else 
                {
                    // Check if the task was canceled (timeout occurred)
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[⚠] Port {port} is LIKELY CLOSED (Connection Timeout or Blocked)");
                    Console.ResetColor();
                }
            }
        }
        catch (SocketException ex)
        {
            
            if (ex.SocketErrorCode == SocketError.TimedOut)
            {
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[⚠] Port {port} is FILTERED (Connection Timeout)");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[✘] Port {port} is CLOSED");
                Console.ResetColor();
            }
        }
    }

    static bool IsValidIPAddress(string ip)
    {
        // Check if the IP is valid (IPv4 or IPv6)
        if (IPAddress.TryParse(ip, out _))
        {
            return true;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("⚠ Invalid IP address. Please enter a valid IPv4 or IPv6 address.");
        Console.ResetColor();
        return false;
    }

    static int GetValidPort(string prompt, int defaultPort)
    {
        int port;
        string input;

        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return defaultPort; 
            }

           
            if (int.TryParse(input, out port) && port >= 1 && port <= 65535)
            {
                return port;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠ Invalid port. Please enter a number between 1 and 65535.");
            Console.ResetColor();
        }
    }
}
