// See https://aka.ms/new-console-template for more information
using System.Device.Gpio.Drivers;
using System.Device.Gpio;

using RPI_4Bit_LCD;

Console.WriteLine("Hello, World!");

const byte LcdRs = 3;
const byte LcdE = 5;
const byte LcdD4 = 7;
const byte LcdD5 = 8;
const byte LcdD6 = 10;
const byte LcdD7 = 11;


GpioController gpioController = new GpioController(PinNumberingScheme.Board, new RaspberryPi3Driver());

LcdDriver lcd = new LcdDriver(gpioController, LcdRs, LcdE, LcdD4, LcdD5, LcdD6, LcdD7, 20);
lcd.Init();
lcd.WriteLine1("ABCDEFGHIJKLMNOPQRST");
lcd.WriteLine2("uvwxyz1234567890.,-!");