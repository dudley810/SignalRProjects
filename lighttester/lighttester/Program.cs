using RaspberryPiDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace lighttester
{
    class Program
    {
        static void Main(string[] args)
        {
            GPIOMem gpio01 = new GPIOMem(GPIOPins.GPIO_01);

            for (int i = 0; i < 100; i++)
            {
                PinState ps = gpio01.Read();
                Console.WriteLine(ps.ToString());
                Thread.Sleep(2000);
            }
        }
    }
}
