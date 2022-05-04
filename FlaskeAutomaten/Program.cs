using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomaten
{
    internal class Program
    {
        public static bool terminator = false;

        static void Main(string[] args)
        {
            Queue<Bottles> bpQueue = new Queue<Bottles>();
            Queue<Bottles> bcQueue = new Queue<Bottles>();
            Queue<Bottles> scQueue = new Queue<Bottles>();

            BottleProduction bp = new BottleProduction(bpQueue);
            Splitter split = new Splitter(bpQueue, bcQueue, scQueue);
            BeerConsumer bc = new BeerConsumer(bcQueue);
            SodaConsumer sc = new SodaConsumer(scQueue);

            Thread producer = new Thread(bp.Producer);
            Thread splitter = new Thread(split.SplitBottles);
            Thread beerConsumer = new Thread(bc.ConsumeBeer);
            Thread sodaConsumer = new Thread(sc.ConsumeSoda);

            producer.Start();
            splitter.Start();
            sodaConsumer.Start();
            beerConsumer.Start();

            while (terminator == false)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    terminator = true;
                }
            }

            try
            {
                producer.Join();
                splitter.Join();
                sodaConsumer.Join();
                beerConsumer.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}

//public static int maxAmount = 0;

//public static void Producer()
//{
//    Random rand = new Random();

//    while (maxAmount < 50)
//    {
//        Monitor.Enter(bottles);

//        if (rand.Next(0, 99) < 50)
//        {
//            bottles.Enqueue($"Beer {maxAmount}");
//            Console.WriteLine($"Bottle {maxAmount} was a beer");
//        }
//        else
//        {
//            bottles.Enqueue($"Soda {maxAmount}");
//            Console.WriteLine($"Bottle {maxAmount} was a soda");
//        }

//        maxAmount++;

//        Monitor.Pulse(bottles);
//        Monitor.Exit(bottles);

//        Thread.Sleep(500);
//    }
//}

//public static void Splitter()
//{
//    string dequeueBottle = "";

//    while (true)
//    {
//        while (true)
//        {
//            if (Monitor.TryEnter(bottles))
//            {
//                if (bottles.Count == 0)
//                {
//                    Monitor.Wait(bottles);
//                }
//                dequeueBottle = bottles.Dequeue();
//                Monitor.PulseAll(bottles);
//                Monitor.Exit(bottles);
//                break;
//            }
//        }


//        if (dequeueBottle.Contains("B"))
//        {
//            while (true)
//            {
//                if (Monitor.TryEnter(beers))
//                {
//                    if (beers.Count == 50)
//                    {
//                        Monitor.Wait(beers);
//                    }

//                    beers.Enqueue(dequeueBottle);
//                    Console.WriteLine($"Beer number {beers.Count} was relocated");
//                    Monitor.Pulse(beers);
//                    Monitor.Exit(beers);
//                    break;
//                }
//            }
//        }
//        else
//        {
//            while (true)
//            {
//                if (Monitor.TryEnter(sodas))
//                {
//                    if (sodas.Count == 50)
//                    {
//                        Monitor.Wait(sodas);
//                    }

//                    sodas.Enqueue(dequeueBottle);
//                    Console.WriteLine($"Soda number {sodas.Count} was relocated");
//                    Monitor.Pulse(sodas);
//                    Monitor.Exit(sodas);
//                    break;
//                }
//            }
//        }
//        Thread.Sleep(100 / 15);
//    }
//}

//public static void BeerConsumer()
//{
//    while (true)
//    {
//        if (Monitor.TryEnter(beers))
//        {
//            if (beers.Count == 0)
//            {
//                Monitor.Wait(beers);
//            }
//            Console.WriteLine($"{beers.Dequeue()} has been consumed");
//            Monitor.Pulse(beers);
//            Monitor.Exit(beers);
//            break;
//        }
//    }
//}

//public static void SodaConsumer()
//{
//    while (true)
//    {
//        if (Monitor.TryEnter(sodas))
//        {
//            if (sodas.Count == 0)
//            {
//                Monitor.Wait(sodas);
//            }
//            Console.WriteLine($"{sodas.Dequeue()} has been consumed");
//            Monitor.Pulse(sodas);
//            Monitor.Exit(sodas);
//            break;
//        }
//    }
//}
