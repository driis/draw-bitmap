using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

const int width = 800, height = 480;

string file = args[0];
var outFile = File.Open(file, FileMode.OpenOrCreate);
var size = width * height * 2;


using var image = new Image<Bgr565>(width, height);
var pos = new PointF(width/2, height/2);
var vec = new PointF(1, -1);
const int r = 16;
while (true)
{
    image.Mutate(ctx =>
    {
        ctx.Fill(Color.WhiteSmoke);
        var circle = new EllipsePolygon(pos, r);
        ctx.Fill(Color.Red, circle);
    });    
    Draw(image);
    await Task.Delay(5);
    pos = pos + vec;
    if (pos.X <= 0 || pos.X >= width)
    {
        vec.X *= -1;
    }

    if (pos.Y <= 0 || pos.Y >= height)
    {
        vec.Y *= -1;
    }
}
void Draw(Image<Bgr565> img)
{
    Span<byte> buffer = new byte[size];
    outFile.Seek(0, SeekOrigin.Begin);
    img.CopyPixelDataTo(buffer);
    outFile.Write(buffer);    
}
