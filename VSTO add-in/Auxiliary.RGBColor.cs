using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace CodeEvaluation
{    
    partial class Auxiliary
    {        
        private static List<string> colors = new List<string>();
        private const int MINIMUM_COLOR_DIFFERENCE = 100;
        
        /// <summary>
        /// Generate a color that can be easily distinguished
        /// </summary>
        /// <returns></returns>
        public static string GenerateColor()
        {
            RGBColor color = new RGBColor(0, 0, 0);
            while (!GenerateColor(ref color)) ;

            return color.ToHexadecimal();
        }

        private static bool GenerateColor(ref RGBColor color)
        {
            Random random = new Random();
            color.Red = random.Next(255);
            color.Green = random.Next(255); 
            color.Blue = random.Next(255); 

            foreach (var str in colors)
            {
                if(color.ColorDifference(str) < MINIMUM_COLOR_DIFFERENCE)
                {
                    return false;
                }
            }
            return true;
        }        
    }

    /// <summary>
    /// Represent color in RGB format
    /// </summary>
    class RGBColor
    {
        private int red;
        private int green;
        private int blue;

        /// <summary>
        /// Red component, the value should between [0, 255], 
        /// otherwise the component is not modified
        /// </summary>
        public int Red
        {
            get => red;
            set
            {
                if (value <= 255 && value >= 0)
                {
                    red = value;
                }
            }
        }

        /// <summary>
        /// Green component, the value should between [0, 255]
        /// </summary>
        public int Green
        {
            get => green;
            set
            {
                if (value <= 255 && value >= 0)
                {
                    green = value;
                }
            }
        }

        /// <summary>
        /// Blue component, the value should between [0, 255]
        /// </summary>
        public int Blue
        {
            get => blue;
            set
            {
                if (value <= 255 && value >= 0)
                {
                    blue = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        public RGBColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RGBColor() : this(0, 0, 0) {}

        /// <summary>
        /// Convert the color in RGB fotmat to its hexadecimal representation
        /// </summary>
        /// <returns></returns>
        public string ToHexadecimal()
        {
            return String.Format($"{Red:X02}{Green:X02}{Blue:X02}");
        }

        /// <summary>
        /// Convert hexadecimal representation into RGB format
        /// </summary>
        /// <param name="color">String representation of the color(hexadecimal)</param>
        /// <returns></returns>
        public static RGBColor ToRGBColor(string color)
        {
            if (color.Length != 6)
            {
                throw new ArgumentException($"Invalid color format in hexadecimal {color}");
            }

            int red = Convert.ToInt32(color.Substring(0, 2), 16);
            int green = Convert.ToInt32(color.Substring(2, 2), 16);
            int blue = Convert.ToInt32(color.Substring(4, 2), 16);

            return new RGBColor(red, green, blue);
        }

        /// <summary>
        /// Determine the difference of two colors, avoid randomly generated colors 
        /// are too colose to each other so that they cannot be distinguished
        /// </summary>
        /// <param name="other">Another color in RGB format</param>
        /// <returns></returns>
        public int ColorDifference(RGBColor other)
        {
            return (int)(Math.Pow(Red - other.Red, 2) + Math.Pow(Green - other.Green, 2) + Math.Pow(Blue - other.Blue, 2));
        }

        /// <summary>
        /// Determine the difference of two colors
        /// </summary>
        /// <param name="color">Another color in string representation(Hexadecimal)</param>
        /// <returns></returns>
        public int ColorDifference(string color)
        {
            RGBColor temp = ToRGBColor(color);
            return ColorDifference(temp);
        }

        public override string ToString()
        {
            return String.Format($"RGBColor({Red}, {Green}, {Blue})");
        }
    }
}
