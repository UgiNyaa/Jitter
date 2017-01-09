using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jitter
{
    public class IndexedQueue<T>
    {
        private Node begin;
        private Node last;

        public void Enqueue(T value, int index)
        {
            if (begin == null)
            {
                begin = new Node(null, null, value, index);
                last = begin;
                return;
            }
            
            if (last.Index < index)
            {
                // best case
                // everything is fine
                last.Next = new Node(last, null, value, index);
                if (last == begin)
                    begin.Next = last.Next;
                last = last.Next;
            }
            else if (last.Index > index)
            {
                // fixable case
                // not perfect, but we can fix this
                var curr = last;
                while (curr.Index > index)
                    if (curr.Prev == null)
                    {
                        curr.Prev = new Node(null, curr, value, index);
                        begin = curr.Prev;
                        return;
                    }
                    else
                        curr = curr.Prev;

                if (curr.Index == index)
                    // worst case
                    return;

                curr.Next = new Node(curr.Prev, curr.Next, value, index); ;
                curr.Next.Next.Prev = curr.Next;
            }
            else
            {
                // worst case
                // very weird problem
                // just gonna assume, that they are the same object
                // ignoring
            }
        }
        public T Dequeue()
        {
            if (begin == null)
                throw new NothingThereException("No Node present.");

            if (last == begin)
            {
                var v = begin.Value;
                begin = null;
                last = null;
                return v;
            }

            var value = begin.Value;
            begin = begin.Next;
            begin.Prev = null;
            return value;
        }
        public T Peek()
        {
            return last.Value;
        }

        private class Node
        {
            public Node Next { get; set; }
            public Node Prev { get; set; }
            public T Value { get; set; }
            public int Index { get; set; }

            public Node(Node prev, Node next, T value, int index)
            {
                this.Prev = prev;
                this.Next = next;
                this.Value = value;
                this.Index = index;
            }
        }
    }

    public class NothingThereException : Exception
    {
        public NothingThereException() { }
        public NothingThereException(string message) : base(message) { }
        public NothingThereException(string message, Exception inner) : base(message, inner) { }
    }
}
