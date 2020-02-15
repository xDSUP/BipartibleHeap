using System;


namespace CourseSaod
{
    class Program
    {
        static void Main(string[] args)
        {
            BinomialHeap b = new BinomialHeap();
            b.Insert(8);
            b.Insert(3);
            b.Insert(7);
            b.Insert(1);
            b.Insert(2);
            b.Insert(23);
            b.Insert(31);
            b.Insert(71);
            b.Insert(12);
            b.Insert(22);
            BinomialHeap c = new BinomialHeap();
            c.Insert(21);
            c.Insert(212);
            c.Insert(5);
            c.Insert(32);
            c.Insert(2112);
            c.Insert(21);

            BinomialHeap d = b.Merge(c);

            for(int i = 0; i < 10; i++)
                Console.WriteLine(d.ExtractMin());

            Console.WriteLine(b);
        }
    }
}
