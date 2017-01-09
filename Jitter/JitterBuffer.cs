using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Jitter
{
    public class JitterBuffer<T>
    {
        private IndexedQueue<T> queue;
        private Stream stream;
        private IParser<T> parser;
        private bool running;

        private Timer timer;
        private Thread thread;
        private Action<T> callback;

        public JitterBuffer(Stream stream, IParser<T> parser)
        {
            queue = new IndexedQueue<T>();
            this.stream = stream;
            this.parser = parser;
            this.running = false;
        }

        public void Begin(Action<T> callback)
        {
            running = true;
            this.callback = callback;
            
            timer = new Timer(1000)
            {
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;

            thread = new Thread(read_from_buffer)
            {
                IsBackground = true
            };

            thread.Start();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            running = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                T value;
                lock (queue)
                    value = queue.Dequeue();
                callback(value);
            }
            catch (NothingThereException) { }
        }

        private void read_from_buffer()
        {
            var chunk = new byte[4096];
            var buffer = new List<byte>();

            while (running)
            {
                stream.Read(chunk, 0, 4096);
                buffer.AddRange(chunk);
                var length = 0;
                do
                {
                    length = parser.Check(buffer.ToArray());
                    if (length > 0)
                    {
                        var objdata = buffer.GetRange(0, length).ToArray();
                        int index;
                        var obj = parser.Parse(objdata, out index);
                        lock (queue)
                            queue.Enqueue(obj, index);
                        buffer.RemoveRange(0, length);
                    }
                } while (length > 0);
            }
        }
    }
}
