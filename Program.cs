// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;

namespace led_blink
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pin = 17;
            var lightTimeInMilliseconds = 1000;
            var dimTimeInMilliseconds = 200;

            // bus id on the raspberry pi 3
            const int busId = 1;

            var i2cSettings = new I2cConnectionSettings(busId, Bme280.DefaultI2cAddress);
            var i2cDevice = I2cDevice.Create(i2cSettings);
            var i2CBmpe80 = new Bme280(i2cDevice);
            

            using(i2CBmpe80)
            {
                while (true)
                {
                    // set mode forced so device sleeps after read
                    i2CBmpe80.SetPowerMode(Bmx280PowerMode.Forced);
                    i2CBmpe80.SetHumiditySampling(Sampling.HighResolution);
                    double humValue = await i2CBmpe80.ReadHumidityAsync();
                    Console.WriteLine($"Humidity: {humValue} %");

                    if(humValue > 38)
                    {
                        // Console.WriteLine($"Let's blink an LED!");
                        using (GpioController controller = new GpioController())
                        {
                            controller.OpenPin(pin, PinMode.Output);
                            // Console.WriteLine($"GPIO pin enabled for use: {pin}");

                            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) =>
                            {
                                controller.Dispose();
                            };

                            
                            Console.WriteLine($"Light for {lightTimeInMilliseconds}ms");
                            controller.Write(pin, PinValue.High);
                            Thread.Sleep(lightTimeInMilliseconds);
                            Console.WriteLine($"Dim for {dimTimeInMilliseconds}ms");
                            controller.Write(pin, PinValue.Low);
                            Thread.Sleep(dimTimeInMilliseconds);
                        
                        }
                    }
                }
            }
        }
    }
}