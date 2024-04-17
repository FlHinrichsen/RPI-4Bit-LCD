# RPI-4Bit-LCD

This is a .NET Library to use a LCD Display on a Raspberry Pi

## Examples

```c#
var example = new LcdDriver(lcdRs, lcdE, lcdData4, lcdData5, lcdData6, lcdData7, 16);
example.Init();
example.WriteLine1("Text");
example.WriteLine2("Line2 Text");
```
