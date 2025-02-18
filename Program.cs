
using System.IO.MemoryMappedFiles;
using SkiaSharp;

int width = 800, height = 600;

// Create a Skia canvas
using (var bmp = new SKBitmap(width, height, SKColorType.Rgb565, SKAlphaType.Opaque))
using (var canvas = new SKCanvas(bmp))
{
    canvas.Clear(SKColors.Black);
    using (var paint = new SKPaint { Color = SKColors.Red, StrokeWidth = 5 })
    {
        canvas.DrawLine(50, 50, 750, 550, paint); // Draw red line
        canvas.DrawLine(100,100,400,600, paint);
    }

    // Save raw pixel data to framebuffer
    CopyToFramebuffer("/dev/fb0", bmp);
}

Console.WriteLine("Framebuffer updated.");


static void CopyToFramebuffer(string fbDevice, SKBitmap bmp)
{
int width = bmp.Width, height = bmp.Height;
int bytesPerPixel = 2; // RGB565
int screenSize = width * height * bytesPerPixel;

using (var mmf = MemoryMappedFile.CreateFromFile(fbDevice, FileMode.Open, "fb", screenSize, MemoryMappedFileAccess.ReadWrite))
using (var accessor = mmf.CreateViewAccessor(0, screenSize))
{
    int offset = 0;
    for (int y = 0; y < height; y++)  // Top-down order
    {
        for (int x = 0; x < width; x++)
        {
            SKColor color = bmp.GetPixel(x, y);
            ushort pixel = RGB565(color.Red, color.Green, color.Blue);
            accessor.Write(offset, pixel);
            offset += bytesPerPixel;
        }
    }
}
}

static ushort RGB565(int r, int g, int b)
{
    return (ushort)(((r >> 3) << 11) | ((g >> 2) << 5) | (b >> 3));
}