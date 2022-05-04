using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomaten
{
    class Splitter
    {
        Queue<Bottles> beers;
        Queue<Bottles> sodas;
        Queue<Bottles> bottles;

        private int maxAmount = 0;

        public Splitter(Queue<Bottles> bottleQueue, Queue<Bottles> beerQueue, Queue<Bottles> sodaQueue)
        {
            beers = beerQueue;
            sodas = sodaQueue;
            bottles = bottleQueue;
        }

        public void SplitBottles()
        {
            Random random = new Random();

            int beerCounter = 1;
            int sodaCounter = 1;

            while (maxAmount < 50)
            {
                if (Monitor.TryEnter(bottles))
                {
                    if (bottles.Count < 3)
                    {
                        Monitor.Wait(bottles);
                    }

                    Bottles dequeueBottle = bottles.Dequeue();

                    Monitor.PulseAll(bottles);
                    Monitor.Exit(bottles);

                    if (dequeueBottle.Name.Contains("Beer"))
                    {
                        Monitor.TryEnter(beers);

                        if (beers.Count == 50)
                        {
                            Monitor.Wait(beers);
                        }

                        beers.Enqueue(dequeueBottle);
                        Console.WriteLine($"Beer number {beerCounter} was relocated");
                        beerCounter++;

                        Monitor.Pulse(beers);
                        Monitor.Exit(beers);
                    }
                    else
                    {
                        Monitor.TryEnter(sodas);

                        if (sodas.Count == 50)
                        {
                            Monitor.Wait(sodas);
                        }

                        sodas.Enqueue(dequeueBottle);
                        Console.WriteLine($"Soda number {sodaCounter} was relocated");
                        sodaCounter++;

                        Monitor.Pulse(sodas);
                        Monitor.Exit(sodas);
                    }

                    maxAmount++;

                    Thread.Sleep(random.Next(50, 200));
                }
            }

        }
    }
}
