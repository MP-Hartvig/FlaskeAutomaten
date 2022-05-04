using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomaten
{
    class SodaConsumer
    {
        Queue<Bottles> sodas;

        int maxAmount = 0;

        public SodaConsumer(Queue<Bottles> sodaQueue)
        {
            sodas = sodaQueue;
        }

        public void ConsumeSoda()
        {
            Random random = new Random();

            int count = 1;

            while (true)
            {
                if (sodas.Count != 0)
                {
                    while (maxAmount < 50)
                    {
                        Monitor.TryEnter(sodas);

                        if (sodas.Count == 0)
                        {
                            Monitor.Wait(sodas);
                        }

                        Bottles bots = sodas.Dequeue();
                        Console.WriteLine($"{bots.Name + " " + bots.Id} has been consumed");

                        Monitor.Pulse(sodas);
                        Monitor.Exit(sodas);

                        maxAmount++;
                        count++;

                        Thread.Sleep(random.Next(100, 500));
                    }
                }
            }
        }
    }
}
