using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace RPI_4Bit_LCD
{
  public class LcdDriver
  {
    private readonly byte lcdRs;

    private readonly byte lcdE;

    private readonly byte lcdData4;

    private readonly byte lcdData5;

    private readonly byte lcdData6;

    private readonly byte lcdData7;

    private readonly byte lcdWidth;        // Zeichen je Zeile

    private readonly byte lcdLine1 = 0x80;     // Adresse der ersten Display Zeile

    private readonly byte lcdLine2 = 0xC0;     // Adresse der zweiten Display Zeile

    private readonly int ePulse = 5;

    private readonly int eDelay = 5;

    private readonly bool lcdChr = true;

    private readonly bool lcdCmd = false;

    public LcdDriver(byte lcdRs, byte lcdE, byte lcdData4, byte lcdData5, byte lcdData6, byte lcdData7, byte lcdWidth = 16)
    {
      this.lcdRs = lcdRs;
      this.lcdE = lcdE;
      this.lcdData4 = lcdData4;
      this.lcdData5 = lcdData5;
      this.lcdData6 = lcdData6;
      this.lcdData7 = lcdData7;
      this.lcdWidth = lcdWidth;
    }

    public void Init()
    {
      Pi.Gpio[lcdRs].PinMode = GpioPinDriveMode.Output;
      Pi.Gpio[lcdE].PinMode = GpioPinDriveMode.Output;
      Pi.Gpio[lcdData4].PinMode = GpioPinDriveMode.Output;
      Pi.Gpio[lcdData5].PinMode = GpioPinDriveMode.Output;
      Pi.Gpio[lcdData6].PinMode = GpioPinDriveMode.Output;
      Pi.Gpio[lcdData7].PinMode = GpioPinDriveMode.Output;

      lcd_send_byte(0x33, lcdCmd);
      lcd_send_byte(0x32, lcdCmd);
      lcd_send_byte(0x28, lcdCmd);
      lcd_send_byte(0x0C, lcdCmd);
      lcd_send_byte(0x06, lcdCmd);
      Clear();
    }

    public void Clear()
    {
      lcd_send_byte(0x01, lcdCmd);
    }

    public void WriteScreen(string l1, string l2)
    {
      Clear();
      WriteLine1(l1);
      WriteLine2(l2);
    }

    public void WriteLine1(string text)
    {
      WriteLine(text, lcdLine1);
    }

    public void WriteLine2(string text)
    {
      WriteLine(text, lcdLine2);
    }

    private void WriteLine(string text, byte line)
    {
      lcd_send_byte(line, lcdCmd);
      lcd_message(text);
    }

    private void lcd_message(string text)
    {
      text = text.Substring(0, lcdWidth);
      foreach (char c in text)
      {
        lcd_send_byte((byte)c, lcdChr);
      }
    }

    private void lcd_send_byte(byte bits, bool mode)
    {
      Pi.Gpio[lcdRs].Write(mode);

      Pi.Gpio[lcdData4].Write((bits & 0x10) == 0x10);
      Pi.Gpio[lcdData5].Write((bits & 0x20) == 0x20);
      Pi.Gpio[lcdData6].Write((bits & 0x40) == 0x40);
      Pi.Gpio[lcdData7].Write((bits & 0x80) == 0x80);

      Thread.Sleep(eDelay);
      Pi.Gpio[lcdE].Write(true);
      Thread.Sleep(ePulse);
      Pi.Gpio[lcdE].Write(false);
      Thread.Sleep(eDelay);

      Pi.Gpio[lcdData4].Write((bits & 0x01) == 0x01);
      Pi.Gpio[lcdData5].Write((bits & 0x02) == 0x02);
      Pi.Gpio[lcdData6].Write((bits & 0x04) == 0x04);
      Pi.Gpio[lcdData7].Write((bits & 0x08) == 0x08);

      Thread.Sleep(eDelay);
      Pi.Gpio[lcdE].Write(true);
      Thread.Sleep(ePulse);
      Pi.Gpio[lcdE].Write(false);
      Thread.Sleep(eDelay);
    }
  }
}
