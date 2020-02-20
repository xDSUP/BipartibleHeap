using System;


namespace CourseSaod
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            DateTime time1, time2;
            int total = 1000000;

            rnd.Next(1, 2);
            BinomialHeap b = new BinomialHeap();
            time1 = DateTime.Now;
            for (int i = 0; i < total/2; i++)
                b.Insert(rnd.Next(1, 10000));
            time2 = DateTime.Now;
            Console.WriteLine($"Вставка {total/2} элем в пирамиду за " + (time2-time1));

            BinomialHeap c = new BinomialHeap();
            time1 = DateTime.Now;
            for (int i = 0; i < total/2; i++)
                c.Insert(rnd.Next(1, 100000));
            time2 = DateTime.Now;
            Console.WriteLine($"Вставка {total/2} элем в пирамиду за " + (time2 - time1));

            time1 = DateTime.Now;
            BinomialHeap d = b.Merge(c);
            time2 = DateTime.Now;
            Console.WriteLine($"Слияние пирамид по {total/2} элем кажд за " + (time2 - time1));

            time1 = DateTime.Now;
            for (int i = 0; i < total; i++)
                d.ExtractMin();
            time2 = DateTime.Now;
            Console.WriteLine($"Извлечение всех элементов из пирамиды с {total} элем за " + (time2 - time1));

            //var bm = new BinomialHeapMin();
            //time1 = DateTime.Now;
            //for (int i = 0; i < total/2; i++)
            //    bm.Insert(rnd.Next(1, 100000));
            //time2 = DateTime.Now;
            //Console.WriteLine($"Вставка {total/2} элем в пирамиду за " + (time2 - time1));

            //var cm = new BinomialHeapMin();
            //time1 = DateTime.Now;
            //for (int i = 0; i < total / 2; i++)
            //    cm.Insert(rnd.Next(1, 100000));
            //time2 = DateTime.Now;
            //Console.WriteLine($"Вставка {total/2} элем в пирамиду за " + (time2 - time1));

            //time1 = DateTime.Now;
            //BinomialHeapMin dm = bm.Merge(cm);
            //time2 = DateTime.Now;
            //Console.WriteLine($"Слияние пирамид по {total/2} элем кажд за " + (time2 - time1));

            //time1 = DateTime.Now;
            //for (int i = 0; i < total; i++)
            //    dm.ExtractMin();
            //time2 = DateTime.Now;
            //Console.WriteLine($"Извлечение всех элементов из пирамиды с {total} элем за " + (time2 - time1));

            //Console.WriteLine(b);
        }
    }
}
