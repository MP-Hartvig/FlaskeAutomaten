using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomaten
{
    class BeerConsumer
    {
        Queue<Bottles> beers;

        int maxAmount = 0;

        public BeerConsumer(Queue<Bottles> beerQueue)
        {
            beers = beerQueue;
        }

        public void ConsumeBeer()
        {
            Random random = new Random();

            int count = 1;

            while (true)
            {
                if (beers.Count != 0)
                {
                    while (maxAmount < 50)
                    {
                        Monitor.TryEnter(beers);

                        if (beers.Count == 0)
                        {
                            Monitor.Wait(beers);
                        }

                        Bottles bots = beers.Dequeue();
                        Console.WriteLine($"{bots.Name + " " + bots.Id} has been consumed");

                        Monitor.Pulse(beers);
                        Monitor.Exit(beers);

                        maxAmount++;
                        count++;

                        Thread.Sleep(random.Next(100, 500));
                    }
                }
            }
        }
    }
}
