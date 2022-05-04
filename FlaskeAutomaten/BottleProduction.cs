using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomaten
{
    class BottleProduction
    {
        int maxAmount = 1;

        Queue<Bottles> bottles;

        public BottleProduction(Queue<Bottles> bottleQueue)
        {
            bottles = bottleQueue;
        }

        public void Producer()
        {
            Random rand = new Random();

            int beerCount = 1;
            int sodaCount = 1;

            while (maxAmount <= 50)
            {
                Monitor.TryEnter(bottles);

                if (rand.Next(0, 100) < 50)
                {
                    bottles.Enqueue(new Bottles("Beer", beerCount));
                    Console.WriteLine($"Bottle {maxAmount} was a beer");
                    beerCount++;
                }
                else
                {
                    bottles.Enqueue(new Bottles("Soda", sodaCount));
                    Console.WriteLine($"Bottle {maxAmount} was a soda");
                    sodaCount++;
                }

                maxAmount++;

                Monitor.PulseAll(bottles);
                Monitor.Exit(bottles);

                Thread.Sleep(rand.Next(50, 200));
            }
        }
    }
}
