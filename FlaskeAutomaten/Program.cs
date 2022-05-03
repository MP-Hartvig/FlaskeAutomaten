using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    internal class Program
    {
        public static Queue<string> bottles = new Queue<string>();
        public static Queue<string> sodas = new Queue<string>();
        public static Queue<string> beers = new Queue<string>();

        public static bool terminator = false;

        public static int maxAmount = 0;

        public static void Producer()
        {
            Random rand = new Random();

            while (maxAmount < 50)
            {
                Monitor.Enter(bottles);

                if (rand.Next(0, 99) < 50)
                {
                    bottles.Enqueue($"Beer {bottles.Count}");
                    Console.WriteLine($"Bottle {bottles.Count} was a beer");
                }
                else
                {
                    bottles.Enqueue($"Soda {bottles.Count}");
                    Console.WriteLine($"Bottle {bottles.Count} was a soda");
                }

                maxAmount++;

                Monitor.PulseAll(bottles);
                Monitor.Exit(bottles);

                Thread.Sleep(500);
            }
        }

        public static void Splitter()
        {
            string dequeueBottle = "";

            while (true)
            {
                while (true)
                {
                    if (Monitor.TryEnter(bottles))
                    {
                        if (bottles.Count == 0)
                        {
                            Monitor.Wait(bottles);
                        }
                        dequeueBottle = bottles.Dequeue();
                        Monitor.Pulse(bottles);
                        Monitor.Exit(bottles);
                        break;
                    }
                }


                if (dequeueBottle.Contains("B"))
                {
                    while (true)
                    {
                        if (Monitor.TryEnter(beers))
                        {
                            if (beers.Count == 50)
                            {
                                Monitor.Wait(beers);
                            }

                            beers.Enqueue(dequeueBottle);
                            Console.WriteLine($"Beer number {beers.Count} was relocated");
                            Monitor.Pulse(beers);
                            Monitor.Exit(beers);
                            break;
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        if (Monitor.TryEnter(sodas))
                        {
                            if (sodas.Count == 50)
                            {
                                Monitor.Wait(sodas);
                            }

                            sodas.Enqueue(dequeueBottle);
                            Console.WriteLine($"Soda number {sodas.Count} was relocated");
                            Monitor.Pulse(sodas);
                            Monitor.Exit(sodas);
                            break;
                        }
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }

        public static void BeerConsumer()
        {
            while (true)
            {
                if (Monitor.TryEnter(beers))
                {
                    if (beers.Count == 0)
                    {
                        Monitor.Wait(beers);
                    }
                    Console.WriteLine($"{beers.Dequeue()} has been consumed");
                    Monitor.Pulse(beers);
                    Monitor.Exit(beers);
                    break;
                }
            }
        }

        public static void SodaConsumer()
        {
            while (true)
            {
                if (Monitor.TryEnter(sodas))
                {
                    if (sodas.Count == 0)
                    {
                        Monitor.Wait(sodas);
                    }
                    Console.WriteLine($"{sodas.Dequeue()} has been consumed");
                    Monitor.Pulse(sodas);
                    Monitor.Exit(sodas);
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Thread producer = new Thread(Producer);
            Thread splitter = new Thread(Splitter);
            Thread sodaConsumer = new Thread(BeerConsumer);
            Thread beerConsumer = new Thread(SodaConsumer);

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
