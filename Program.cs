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


// TODO: Put BME280 sensor readings into a library, see C# book
// TODO: Work on getting the potentiometer working, and extrapolate to anometer (direction)
// TODO: Work on getting wind speed working (anometer)
// TODO: Work on rain sensor, might need to do first using python
// TODO: Upload to cloud DB

namespace led_blink
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // bus id on the raspberry pi 3
            const int busId = 1;

            // Setup i2C device (BME280)
            var i2cSettings = new I2cConnectionSettings(busId, Bme280.DefaultI2cAddress);
            var i2cDevice = I2cDevice.Create(i2cSettings);
            var i2CBmpe80 = new Bme280(i2cDevice);
            

            using(i2CBmpe80)
            {
                while (true)
                {
                    // set mode forced so device sleeps after read
                    i2CBmpe80.SetPowerMode(Bmx280PowerMode.Forced);
                    
                    // Get sampling accuracy
                    i2CBmpe80.SetHumiditySampling(Sampling.Standard);
                    i2CBmpe80.SetTemperatureSampling(Sampling.Standard);
                    i2CBmpe80.SetPressureSampling(Sampling.Standard);

                    // Get variables
                    Iot.Units.Temperature tempValue = await i2CBmpe80.ReadTemperatureAsync();
                    double humValue = await i2CBmpe80.ReadHumidityAsync();
                    double preValue = await i2CBmpe80.ReadPressureAsync();

                    // Print to screen
                    Console.WriteLine($"Weather at time: {DateTime.Now}");
                    Console.WriteLine($"Temperature: {tempValue.Celsius:0.#}\u00B0C");
                    Console.WriteLine($"Pressure: {preValue/100:0.##}hPa");
                    Console.WriteLine($"Relative humidity: {humValue:0.#}%\n");

                    Thread.Sleep(2000);
                    
                }
            }
        }
    }
}