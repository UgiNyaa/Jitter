using Jitter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JitterUser
{
    class Program
    {
        static void Main(string[] args)
        {
            var mem = new MemoryStream(Encoding.UTF8.GetBytes("2;hey;1;hello;0;ugii;"));

            var parser = new StringParser();
            var jitter = new JitterBuffer<string>(mem, parser);

            jitter.Begin((word) => Console.WriteLine(word));

            Console.ReadKey();
        }
    }
}
