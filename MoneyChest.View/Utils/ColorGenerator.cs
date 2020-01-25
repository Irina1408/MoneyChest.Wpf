using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoneyChest.View.Utils
{
    public static class ColorGenerator
    {
        public static List<Color> GenerateColors(int number)
        {
            var colors = new List<Color>();
            var rand = new Random();
            var contrast = number > 1000 ? 5 : 8;
            var maxContrastColorsNumber = (int)Math.Pow(255 / contrast, 3);

            for (int i = 0; i < number; i++)
            {
                // generate new values
                byte a = 255;
                byte r = (byte)rand.Next(0, 255);
                byte g = (byte)rand.Next(0, 255);
                byte b = (byte)rand.Next(0, 255);

                // check on existing new value
                while (colors.Count < maxContrastColorsNumber
                     && colors.Any(item => item.A > a - contrast && item.A < a + contrast
                     && item.R > r - contrast && item.R < r + contrast
                     && item.G > g - contrast && item.G < g + contrast
                     && item.B > b - contrast && item.B < b + contrast))
                {
                    r = (byte)rand.Next(0, 255);
                    g = (byte)rand.Next(0, 255);
                    b = (byte)rand.Next(0, 255);
                }

                // add new brush
                colors.Add(new Color() { A = a, R = r, G = g, B = b });
            }

            return colors;
        }

        public static List<Brush> GenerateBrushes(int number) => GenerateColors(number).Select(x => new SolidColorBrush(x) as Brush).ToList();

        private class ColorData
        {
            public ColorData(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public byte A { get; private set; }
            public byte R { get; private set; }
            public byte G { get; private set; }
            public byte B { get; private set; }
        }
    }
}
