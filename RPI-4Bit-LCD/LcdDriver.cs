namespace RPI_4Bit_LCD
{
  using System.Device.Gpio;

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

    private readonly int ePulse = 1;

    private readonly int eDelay = 1;

    private readonly bool lcdChr = true;

    private readonly bool lcdCmd = false;

    private GpioController gpioController;

    public LcdDriver(GpioController gpioController, byte lcdRs, byte lcdE, byte lcdData4, byte lcdData5, byte lcdData6, byte lcdData7, byte lcdWidth = 16)
    {
      this.gpioController = gpioController;
      this.lcdRs = lcdRs;
      this.lcdE = lcdE;
      this.lcdData4 = lcdData4;
      this.lcdData5 = lcdData5;
      this.lcdData6 = lcdData6;
      this.lcdData7 = lcdData7;
      this.lcdWidth = lcdWidth;
    }

    /// <summary>
    /// Initializes the display
    /// </summary>
    public void Init()
    {
      InitPin();
      lcd_send_byte(0x33, lcdCmd);
      lcd_send_byte(0x32, lcdCmd);
      lcd_send_byte(0x28, lcdCmd);
      DisplayOptions(true,false,false);
      DisplayMode(true,false);
      Clear();
    }

    private void InitPin()
    {
      gpioController.OpenPin(lcdRs, PinMode.Output);
      gpioController.OpenPin(lcdE, PinMode.Output);
      gpioController.OpenPin(lcdData4, PinMode.Output);
      gpioController.OpenPin(lcdData5, PinMode.Output);
      gpioController.OpenPin(lcdData6, PinMode.Output);
      gpioController.OpenPin(lcdData7, PinMode.Output);
    }

    /// <summary>
    /// Sets the display options
    /// </summary>
    /// <param name="d">true for display on, false for off</param>
    /// <param name="c">true to show the curser, false for hide</param>
    /// <param name="b">true for flash the curser, false for static</param>
    public void DisplayOptions(bool d, bool c, bool b)
    {
      byte data = 0x08;

      if (d)
      {
        data += 0x04;
      }

      if (c)
      {
        data += 0x02;
      }

      if (b)
      {
        data += 0x01;
      }

      lcd_send_byte(data, lcdCmd);
    }

    /// <summary>
    /// Sets the display mode
    /// </summary>
    /// <param name="id">true for curser increment, false for decrement</param>
    /// <param name="s">true for shifting the text, false for fix text position</param>
    public void DisplayMode(bool id, bool s)
    {
      byte data = 0x04;

      if (id)
      {
        data += 0x02;
      }

      if (s)
      {
        data += 0x01;
      }

      lcd_send_byte(data, lcdCmd);
    }

    /// <summary>
    /// Clears the screen
    /// </summary>
    public void Clear()
    {
      lcd_send_byte(0x01, lcdCmd);
    }

    /// <summary>
    /// Clears the screen and write both lines
    /// </summary>
    /// <param name="l1">Text for line 1</param>
    /// <param name="l2">Text fpr line 2</param>
    public void WriteScreen(string l1, string l2)
    {
      Clear();
      WriteLine1(l1);
      WriteLine2(l2);
    }

    /// <summary>
    /// Writes line 1 without clear
    /// </summary>
    /// <param name="text">text to write</param>
    public void WriteLine1(string text)
    {
      WriteLine(text, lcdLine1);
    }

    /// <summary>
    /// Writes line 2 without clear
    /// </summary>
    /// <param name="text">text to write</param>
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

    private void writeBoolean(int pin, bool value)
    {
      gpioController.Write(pin, value ? PinValue.High : PinValue.Low);
    }

    private void lcd_send_byte(byte bits, bool mode)
    {
      writeBoolean(lcdRs, mode);

      writeBoolean(lcdData4, (bits & 0x10) == 0x10);
      writeBoolean(lcdData5, (bits & 0x20) == 0x20);
      writeBoolean(lcdData6, (bits & 0x40) == 0x40);
      writeBoolean(lcdData7, (bits & 0x80) == 0x80);

      Thread.Sleep(eDelay);
      writeBoolean(lcdE, true);
      Thread.Sleep(ePulse);
      writeBoolean(lcdE, false);
      Thread.Sleep(eDelay);

      writeBoolean(lcdData4, (bits & 0x01) == 0x01);
      writeBoolean(lcdData5, (bits & 0x02) == 0x02);
      writeBoolean(lcdData6, (bits & 0x04) == 0x04);
      writeBoolean(lcdData7, (bits & 0x08) == 0x08);

      Thread.Sleep(eDelay);
      writeBoolean(lcdE, true);
      Thread.Sleep(ePulse);
      writeBoolean(lcdE, false);
      Thread.Sleep(eDelay);
    }
  }
}
