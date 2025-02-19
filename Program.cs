using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

const int width = 800, height = 480;

using var image = new Image<Bgr565>(width, height);
image.Mutate(ctx =>
{
    ctx.Fill(Color.Aqua);
    var circle = new EllipsePolygon(width / 2, height / 2, 200);
    ctx.Draw(new SolidPen(Color.Red), circle);
});

string file = args[0];
var outFile = File.Open(file, FileMode.OpenOrCreate);
var size = width * height * 2;
Span<byte> buffer = new byte[size];

image.CopyPixelDataTo(buffer);
outFile.Write(buffer);

Console.WriteLine("Framebuffer updated.");
