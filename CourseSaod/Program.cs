using System;


namespace CourseSaod
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            DateTime time1, time2;

            rnd.Next(1, 2);
            BinomialHeap b = new BinomialHeap();
            time1 = DateTime.Now;
            for (int i = 0; i < 50000; i++)
                b.Insert(rnd.Next(1, 10000));
            time2 = DateTime.Now;
            Console.WriteLine("Вставка 50000 элем в пирамиду за " + (time2-time1));

            BinomialHeap c = new BinomialHeap();
            time1 = DateTime.Now;
            for (int i = 0; i < 50000; i++)
                c.Insert(rnd.Next(1, 100000));
            time2 = DateTime.Now;
            Console.WriteLine("Вставка 50000 элем в пирамиду за " + (time2 - time1));

            time1 = DateTime.Now;
            BinomialHeap d = b.Merge(c);
            time2 = DateTime.Now;
            Console.WriteLine("Слияние пирамид по 50000 элем кажд за " + (time2 - time1));

            time1 = DateTime.Now;
            for (int i = 0; i < 100000; i++)
                d.ExtractMin();
            time2 = DateTime.Now;
            Console.WriteLine("Извлечение всех элементов из пирамиды с 100000 элем за " + (time2 - time1));
            
            Console.WriteLine(b);
        }
    }
}
