using System;

namespace ServerCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var server = HTSServer.getInstance(@"http://127.0.0.1:5000");
            var response = server.login("Neptune2", "12345");
            response = server.login("Neptune2", "12345");
            Console.WriteLine(response.text);
        }
    }
}
