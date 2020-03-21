using System;
using System.Text.RegularExpressions;


namespace CourseSaod
{
    class Program
    {
        static void Main()
        {
            Random rnd = new Random();
            DateTime time1, time2;
            int total = 1000000;

            rnd.Next(1, 2);
            BinomialHeap b = new BinomialHeap();
            time1 = DateTime.Now;
            for (int i = 0; i < total/2; i++)
                b.Insert(rnd.Next(1, 100000));
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

            //time1 = DateTime.Now;
            //for (int i = 0; i < total; i++)
            //    d.ExtractMin();
            //time2 = DateTime.Now;
            //Console.WriteLine($"Извлечение всех элементов из пирамиды с {total} элем за " + (time2 - time1));

            var bm = new BinomialHeapMin();
            time1 = DateTime.Now;
            Regex regex = new Regex(@"(0|1|01*2)*(1|01*2)");
            for (int i = 0; i < total; i++)
            {
                bm.Insert(rnd.Next(1, 100000));
                //bm.Insert(i);
                //Console.WriteLine(bm.ToString());
                //if(bm.stackTwoNodes.Count > 2) Console.WriteLine("@@SLOMALOS @@");
                //Console.WriteLine(regex.Matches(bm.ToString())[0]);
                //if (regex.IsMatch(bm.ToString()) == false) Console.WriteLine("SLOMALOS");
            }
            Console.WriteLine(bm.stackTwoNodes.Count);
            time2 = DateTime.Now;
            Console.WriteLine($"Вставка {total} элем в пирамиду за " + (time2 - time1));

            var cm = new BinomialHeapMin();
            time1 = DateTime.Now;
            for (int i = 0; i < total / 2; i++)
            {
                int da = rnd.Next(1, 100000);
                cm.Insert(da);

            }
            time2 = DateTime.Now;
            Console.WriteLine($"Вставка {total / 2} элем в пирамиду за " + (time2 - time1));

            time1 = DateTime.Now;
            Console.WriteLine(bm.ToString());
            Console.WriteLine(cm.ToString());
            BinomialHeapMin dm = bm.Merge(cm);
            Console.WriteLine(dm.ToString());
            time2 = DateTime.Now;
            Console.WriteLine($"Слияние пирамид по {total / 2} элем кажд за " + (time2 - time1));

            time1 = DateTime.Now;
            for (int i = 0; i < total; i++)
            {
                BinomialHeapMin.HeapNode temp = dm.head;
                //Console.Write(dm._minNode.Key + " | ");
                //do
                //{
                //    Console.Write(temp.Key + $"({temp.Degree})" + " ");
                //    temp = temp.rightBrother;
                //} while (temp != dm.head);
                //temp = dm.GetMin().child;
                //var child = temp;
                //Console.Write("Дети извлеченного ");
                //do
                //{
                //    Console.Write(temp?.Key + $"({temp?.Degree})" + " ");
                //    temp = temp?.rightBrother;
                //} while (temp != child);
                //Console.WriteLine();
                //temp = dm.ExtractMin();
                //Console.WriteLine("[EXTRACT] | Извлек: " + i + " " + temp.Key);
            }
            time2 = DateTime.Now;
            Console.WriteLine($"Извлечение всех элементов из пирамиды с {total} элем за " + (time2 - time1));

            Console.WriteLine(b);
            Console.WriteLine(bm);


        }
    }
}
