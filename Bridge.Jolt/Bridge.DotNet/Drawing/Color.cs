using System.Collections.Generic;

namespace System.Drawing
{
    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color.
    /// </summary>
    public struct Color
    {
        #region Constructors

        Color(byte Alpha, byte Red, byte Green, byte Blue)
        {
            this.KnownColor = "";
            this.A = Alpha;
            this.R = Red;
            this.G = Green;
            this.B = Blue;
        }


        Color(string KnownColor, byte Alpha, byte Red, byte Green, byte Blue)
        {
            this.KnownColor = KnownColor;
            this.A = Alpha;
            this.R = Red;
            this.G = Green;
            this.B = Blue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the red component value of this Color structure.
        /// </summary>
        public byte R { get; private set; }

        /// <summary>
        /// Gets the green component value of this Color structure.
        /// </summary>
        public byte G { get; private set; }

        /// <summary>
        /// Gets the blue component value of this Color structure.
        /// </summary>
        public byte B { get; private set; }

        /// <summary>
        /// Gets the alpha component value of this Color structure.
        /// </summary>
        public byte A { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this Color structure is a predefined color. Predefined colors are represented by the elements of the KnownColor enumeration.
        /// </summary>
        public bool IsKnownColor => !String.IsNullOrEmpty(this.KnownColor);

        /// <summary>
        /// Gets the name of this Color.
        /// This method returns either the user-defined name of the color, if the color was created from a name, or the name of the known color. For custom colors, the RGB value is returned.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.IsKnownColor)
                    return this.KnownColor;
                else
                    return ArgbToName(this.A, this.R, this.G, this.B);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the 32-bit ARGB value of this Color structure.
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB. The most significant byte (MSB), represented by AA, is the alpha component value.
        /// The second, third, and fourth bytes, represented by RR, GG, and BB, respectively, are the color components red, green, and blue, respectively
        /// </summary>
        /// <returns>The 32-bit ARGB value of this Color.</returns>
        public int ToArgb()
        {
            int A = this.A;
            int R = this.R;
            int G = this.G;
            int B = this.B;
            return (A << 24) | (R << 16) | (G << 8) | B;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Color lhs, Color rhs) => lhs.A == rhs.A && lhs.R == rhs.R && lhs.G == rhs.G && lhs.B == rhs.B;

        /// <summary>
        /// Unequal operator
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Color lhs, Color rhs) => !(lhs == rhs);

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (o is Color C)
                return this == C;
            else
                return false;
        }

        /// <summary>
        /// Gets a hashcode for this color instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => this.ToArgb();

        /// <summary>
        /// Provides a string-converter for this color instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.Name;

        #endregion

        #region Static methods

        /// <summary>
        /// Creates a Color structure from the specified name of a predefined color.
        /// A predefined color is also called a known color and is represented by an element of the KnownColor enumeration.
        /// If the name parameter is not the valid name of a predefined color, the FromName method creates a Color structure that has an ARGB value of 0 (that is, all ARGB components are 0).
        /// </summary>
        /// <param name="name">A string that is the name of a predefined color.</param>
        /// <returns>The Color that this method creates.</returns>
        static public Color FromName(string name)
        {
            if (colorsFromNameDict == null)
            {
                Dictionary<string, Color> dict = new Dictionary<string, Color>(160);    // Prevent excessive resizing.
                GenerateColorsFromNameDict(dict);
                colorsFromNameDict = dict;
            }

            // Try to find the requested name in our dictionary.
            string temp = name.ToLower();
            if (colorsFromNameDict.TryGetValue(temp, out Color result))
            {
                return result;
            }
            else
            {
                return new Color(name, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Creates a Color structure from the four ARGB component (alpha, red, green, and blue) values. Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha component. Valid values are 0 through 255.</param>
        /// <param name="red">The red component. Valid values are 0 through 255.</param>
        /// <param name="green">The green component. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component. Valid values are 0 through 255.</param>
        /// <returns>The Color that this method creates.</returns>
        static public Color FromArgb(int alpha, int red, int green, int blue)
        {
            // Check arguments
            if (alpha < 0 || alpha > 255) throw new ArgumentException("Value is less than 0 or greater than 255", nameof(alpha));
            if (red < 0 || red > 255) throw new ArgumentException("Value is less than 0 or greater than 255", nameof(red));
            if (green < 0 || green > 255) throw new ArgumentException("Value is less than 0 or greater than 255", nameof(green));
            if (blue < 0 || blue > 255) throw new ArgumentException("Value is less than 0 or greater than 255", nameof(blue));

            // Create it
            return new Color(Convert.ToByte(alpha), Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
        }

        /// <summary>
        /// Creates a Color structure from the specified 8-bit color values (red, green, and blue). The alpha value is implicitly 255 (fully opaque). Although this method allows a 32-bit value to be passed for each color component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="red">The red component value for the new Color. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the new Color. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new Color. Valid values are 0 through 255.</param>
        /// <returns>The Color that this method creates.</returns>
        static public Color FromArgb(int red, int green, int blue) => FromArgb(255, red, green, blue);

        /// <summary>
        /// Creates a Color structure from the specified Color structure, but with the new specified alpha value. Although this method allows a 32-bit value to be passed for the alpha value, the value is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha value for the new Color. Valid values are 0 through 255.</param>
        /// <param name="baseColor">The Color from which to create the new Color.</param>
        /// <returns>The Color that this method creates.</returns>
        static public Color FromArgb(int alpha, Color baseColor) => FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);

        /// <summary>
        /// Creates a Color structure from a 32-bit ARGB value.
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB. The most significant byte (MSB), represented by AA, is the alpha component value.
        /// The second, third, and fourth bytes, represented by RR, GG, and BB, respectively, are the color components red, green, and blue, respectively.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <returns>The Color structure that this method creates.</returns>
        public static Color FromArgb(int argb)
        {
            return FromArgb((argb >> 24) & 0xFF, (argb >> 16) & 0xFF, (argb >> 8) & 0xFF, argb & 0xFF);
        }

        #endregion

        #region Privates

        static string ArgbToName(byte Alpha, byte Red, byte Green, byte Blue) => $"#{Alpha.ToString("X2", Globalization.CultureInfo.InvariantCulture)}{Red.ToString("X2", System.Globalization.CultureInfo.InvariantCulture)}{Green.ToString("X2", System.Globalization.CultureInfo.InvariantCulture)}{Blue.ToString("X2", System.Globalization.CultureInfo.InvariantCulture)}";

        readonly string KnownColor;

        #endregion

        #region System defined colors

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #00FFFFFF.
        /// </summary>
        public static Color Transparent { get; } = new Color("Transparent", 0, 255, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF0F8FF.
        /// </summary>
        public static Color AliceBlue { get; } = new Color("AliceBlue", 255, 240, 248, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFAEBD7.
        /// </summary>
        public static Color AntiqueWhite { get; } = new Color("AntiqueWhite", 255, 250, 235, 215);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00FFFF.
        /// </summary>
        public static Color Aqua { get; } = new Color("Aqua", 255, 0, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF7FFFD4.
        /// </summary>
        public static Color Aquamarine { get; } = new Color("Aquamarine", 255, 127, 255, 212);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF0FFFF.
        /// </summary>
        public static Color Azure { get; } = new Color("Azure", 255, 240, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF5F5DC.
        /// </summary>
        public static Color Beige { get; } = new Color("Beige", 255, 245, 245, 220);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFE4C4.
        /// </summary>
        public static Color Bisque { get; } = new Color("Bisque", 255, 255, 228, 196);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF000000.
        /// </summary>
        public static Color Black { get; } = new Color("Black", 255, 0, 0, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFEBCD.
        /// </summary>
        public static Color BlanchedAlmond { get; } = new Color("BlanchedAlmond", 255, 255, 235, 205);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF0000FF.
        /// </summary>
        public static Color Blue { get; } = new Color("Blue", 255, 0, 0, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF8A2BE2.
        /// </summary>
        public static Color BlueViolet { get; } = new Color("BlueViolet", 255, 138, 43, 226);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFA52A2A.
        /// </summary>
        public static Color Brown { get; } = new Color("Brown", 255, 165, 42, 42);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDEB887.
        /// </summary>
        public static Color BurlyWood { get; } = new Color("BurlyWood", 255, 222, 184, 135);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF5F9EA0.
        /// </summary>
        public static Color CadetBlue { get; } = new Color("CadetBlue", 255, 95, 158, 160);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF7FFF00.
        /// </summary>
        public static Color Chartreuse { get; } = new Color("Chartreuse", 255, 127, 255, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFD2691E.
        /// </summary>
        public static Color Chocolate { get; } = new Color("Chocolate", 255, 210, 105, 30);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF7F50.
        /// </summary>
        public static Color Coral { get; } = new Color("Coral", 255, 255, 127, 80);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF6495ED.
        /// </summary>
        public static Color CornflowerBlue { get; } = new Color("CornflowerBlue", 255, 100, 149, 237);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFF8DC.
        /// </summary>
        public static Color Cornsilk { get; } = new Color("Cornsilk", 255, 255, 248, 220);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDC143C.
        /// </summary>
        public static Color Crimson { get; } = new Color("Crimson", 255, 220, 20, 60);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00FFFF.
        /// </summary>
        public static Color Cyan { get; } = new Color("Cyan", 255, 0, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00008B.
        /// </summary>
        public static Color DarkBlue { get; } = new Color("DarkBlue", 255, 0, 0, 139);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF008B8B.
        /// </summary>
        public static Color DarkCyan { get; } = new Color("DarkCyan", 255, 0, 139, 139);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFB8860B.
        /// </summary>
        public static Color DarkGoldenrod { get; } = new Color("DarkGoldenrod", 255, 184, 134, 11);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFA9A9A9.
        /// </summary>
        public static Color DarkGray { get; } = new Color("DarkGray", 255, 169, 169, 169);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF006400.
        /// </summary>
        public static Color DarkGreen { get; } = new Color("DarkGreen", 255, 0, 100, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFBDB76B.
        /// </summary>
        public static Color DarkKhaki { get; } = new Color("DarkKhaki", 255, 189, 183, 107);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF8B008B.
        /// </summary>
        public static Color DarkMagenta { get; } = new Color("DarkMagenta", 255, 139, 0, 139);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF556B2F.
        /// </summary>
        public static Color DarkOliveGreen { get; } = new Color("DarkOliveGreen", 255, 85, 107, 47);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF8C00.
        /// </summary>
        public static Color DarkOrange { get; } = new Color("DarkOrange", 255, 255, 140, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF9932CC.
        /// </summary>
        public static Color DarkOrchid { get; } = new Color("DarkOrchid", 255, 153, 50, 204);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF8B0000.
        /// </summary>
        public static Color DarkRed { get; } = new Color("DarkRed", 255, 139, 0, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFE9967A.
        /// </summary>
        public static Color DarkSalmon { get; } = new Color("DarkSalmon", 255, 233, 150, 122);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF8FBC8B.
        /// </summary>
        public static Color DarkSeaGreen { get; } = new Color("DarkSeaGreen", 255, 143, 188, 139);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF483D8B.
        /// </summary>
        public static Color DarkSlateBlue { get; } = new Color("DarkSlateBlue", 255, 72, 61, 139);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF2F4F4F.
        /// </summary>
        public static Color DarkSlateGray { get; } = new Color("DarkSlateGray", 255, 47, 79, 79);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00CED1.
        /// </summary>
        public static Color DarkTurquoise { get; } = new Color("DarkTurquoise", 255, 0, 206, 209);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF9400D3.
        /// </summary>
        public static Color DarkViolet { get; } = new Color("DarkViolet", 255, 148, 0, 211);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF1493.
        /// </summary>
        public static Color DeepPink { get; } = new Color("DeepPink", 255, 255, 20, 147);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00BFFF.
        /// </summary>
        public static Color DeepSkyBlue { get; } = new Color("DeepSkyBlue", 255, 0, 191, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF696969.
        /// </summary>
        public static Color DimGray { get; } = new Color("DimGray", 255, 105, 105, 105);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF1E90FF.
        /// </summary>
        public static Color DodgerBlue { get; } = new Color("DodgerBlue", 255, 30, 144, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFB22222.
        /// </summary>
        public static Color Firebrick { get; } = new Color("Firebrick", 255, 178, 34, 34);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFAF0.
        /// </summary>
        public static Color FloralWhite { get; } = new Color("FloralWhite", 255, 255, 250, 240);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF228B22.
        /// </summary>
        public static Color ForestGreen { get; } = new Color("ForestGreen", 255, 34, 139, 34);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF00FF.
        /// </summary>
        public static Color Fuchsia { get; } = new Color("Fuchsia", 255, 255, 0, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDCDCDC.
        /// </summary>
        public static Color Gainsboro { get; } = new Color("Gainsboro", 255, 220, 220, 220);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF8F8FF.
        /// </summary>
        public static Color GhostWhite { get; } = new Color("GhostWhite", 255, 248, 248, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFD700.
        /// </summary>
        public static Color Gold { get; } = new Color("Gold", 255, 255, 215, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDAA520.
        /// </summary>
        public static Color Goldenrod { get; } = new Color("Goldenrod", 255, 218, 165, 32);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF808080.
        /// </summary>
        public static Color Gray { get; } = new Color("Gray", 255, 128, 128, 128);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF008000.
        /// </summary>
        public static Color Green { get; } = new Color("Green", 255, 0, 128, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFADFF2F.
        /// </summary>
        public static Color GreenYellow { get; } = new Color("GreenYellow", 255, 173, 255, 47);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF0FFF0.
        /// </summary>
        public static Color Honeydew { get; } = new Color("Honeydew", 255, 240, 255, 240);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF69B4.
        /// </summary>
        public static Color HotPink { get; } = new Color("HotPink", 255, 255, 105, 180);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFCD5C5C.
        /// </summary>
        public static Color IndianRed { get; } = new Color("IndianRed", 255, 205, 92, 92);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF4B0082.
        /// </summary>
        public static Color Indigo { get; } = new Color("Indigo", 255, 75, 0, 130);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFFF0.
        /// </summary>
        public static Color Ivory { get; } = new Color("Ivory", 255, 255, 255, 240);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF0E68C.
        /// </summary>
        public static Color Khaki { get; } = new Color("Khaki", 255, 240, 230, 140);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFE6E6FA.
        /// </summary>
        public static Color Lavender { get; } = new Color("Lavender", 255, 230, 230, 250);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFF0F5.
        /// </summary>
        public static Color LavenderBlush { get; } = new Color("LavenderBlush", 255, 255, 240, 245);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF7CFC00.
        /// </summary>
        public static Color LawnGreen { get; } = new Color("LawnGreen", 255, 124, 252, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFACD.
        /// </summary>
        public static Color LemonChiffon { get; } = new Color("LemonChiffon", 255, 255, 250, 205);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFADD8E6.
        /// </summary>
        public static Color LightBlue { get; } = new Color("LightBlue", 255, 173, 216, 230);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF08080.
        /// </summary>
        public static Color LightCoral { get; } = new Color("LightCoral", 255, 240, 128, 128);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFE0FFFF.
        /// </summary>
        public static Color LightCyan { get; } = new Color("LightCyan", 255, 224, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFAFAD2.
        /// </summary>
        public static Color LightGoldenrodYellow { get; } = new Color("LightGoldenrodYellow", 255, 250, 250, 210);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF90EE90.
        /// </summary>
        public static Color LightGreen { get; } = new Color("LightGreen", 255, 144, 238, 144);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFD3D3D3.
        /// </summary>
        public static Color LightGray { get; } = new Color("LightGray", 255, 211, 211, 211);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFB6C1.
        /// </summary>
        public static Color LightPink { get; } = new Color("LightPink", 255, 255, 182, 193);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFA07A.
        /// </summary>
        public static Color LightSalmon { get; } = new Color("LightSalmon", 255, 255, 160, 122);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF20B2AA.
        /// </summary>
        public static Color LightSeaGreen { get; } = new Color("LightSeaGreen", 255, 32, 178, 170);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF87CEFA.
        /// </summary>
        public static Color LightSkyBlue { get; } = new Color("LightSkyBlue", 255, 135, 206, 250);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF778899.
        /// </summary>
        public static Color LightSlateGray { get; } = new Color("LightSlateGray", 255, 119, 136, 153);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFB0C4DE.
        /// </summary>
        public static Color LightSteelBlue { get; } = new Color("LightSteelBlue", 255, 176, 196, 222);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFFE0.
        /// </summary>
        public static Color LightYellow { get; } = new Color("LightYellow", 255, 255, 255, 224);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00FF00.
        /// </summary>
        public static Color Lime { get; } = new Color("Lime", 255, 0, 255, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF32CD32.
        /// </summary>
        public static Color LimeGreen { get; } = new Color("LimeGreen", 255, 50, 205, 50);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFAF0E6.
        /// </summary>
        public static Color Linen { get; } = new Color("Linen", 255, 250, 240, 230);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF00FF.
        /// </summary>
        public static Color Magenta { get; } = new Color("Magenta", 255, 255, 0, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF800000.
        /// </summary>
        public static Color Maroon { get; } = new Color("Maroon", 255, 128, 0, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF66CDAA.
        /// </summary>
        public static Color MediumAquamarine { get; } = new Color("MediumAquamarine", 255, 102, 205, 170);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF0000CD.
        /// </summary>
        public static Color MediumBlue { get; } = new Color("MediumBlue", 255, 0, 0, 205);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFBA55D3.
        /// </summary>
        public static Color MediumOrchid { get; } = new Color("MediumOrchid", 255, 186, 85, 211);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF9370DB.
        /// </summary>
        public static Color MediumPurple { get; } = new Color("MediumPurple", 255, 147, 112, 219);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF3CB371.
        /// </summary>
        public static Color MediumSeaGreen { get; } = new Color("MediumSeaGreen", 255, 60, 179, 113);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF7B68EE.
        /// </summary>
        public static Color MediumSlateBlue { get; } = new Color("MediumSlateBlue", 255, 123, 104, 238);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00FA9A.
        /// </summary>
        public static Color MediumSpringGreen { get; } = new Color("MediumSpringGreen", 255, 0, 250, 154);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF48D1CC.
        /// </summary>
        public static Color MediumTurquoise { get; } = new Color("MediumTurquoise", 255, 72, 209, 204);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFC71585.
        /// </summary>
        public static Color MediumVioletRed { get; } = new Color("MediumVioletRed", 255, 199, 21, 133);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF191970.
        /// </summary>
        public static Color MidnightBlue { get; } = new Color("MidnightBlue", 255, 25, 25, 112);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF5FFFA.
        /// </summary>
        public static Color MintCream { get; } = new Color("MintCream", 255, 245, 255, 250);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFE4E1.
        /// </summary>
        public static Color MistyRose { get; } = new Color("MistyRose", 255, 255, 228, 225);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFE4B5.
        /// </summary>
        public static Color Moccasin { get; } = new Color("Moccasin", 255, 255, 228, 181);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFDEAD.
        /// </summary>
        public static Color NavajoWhite { get; } = new Color("NavajoWhite", 255, 255, 222, 173);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF000080.
        /// </summary>
        public static Color Navy { get; } = new Color("Navy", 255, 0, 0, 128);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFDF5E6.
        /// </summary>
        public static Color OldLace { get; } = new Color("OldLace", 255, 253, 245, 230);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF808000.
        /// </summary>
        public static Color Olive { get; } = new Color("Olive", 255, 128, 128, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF6B8E23.
        /// </summary>
        public static Color OliveDrab { get; } = new Color("OliveDrab", 255, 107, 142, 35);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFA500.
        /// </summary>
        public static Color Orange { get; } = new Color("Orange", 255, 255, 165, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF4500.
        /// </summary>
        public static Color OrangeRed { get; } = new Color("OrangeRed", 255, 255, 69, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDA70D6.
        /// </summary>
        public static Color Orchid { get; } = new Color("Orchid", 255, 218, 112, 214);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFEEE8AA.
        /// </summary>
        public static Color PaleGoldenrod { get; } = new Color("PaleGoldenrod", 255, 238, 232, 170);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF98FB98.
        /// </summary>
        public static Color PaleGreen { get; } = new Color("PaleGreen", 255, 152, 251, 152);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFAFEEEE.
        /// </summary>
        public static Color PaleTurquoise { get; } = new Color("PaleTurquoise", 255, 175, 238, 238);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDB7093.
        /// </summary>
        public static Color PaleVioletRed { get; } = new Color("PaleVioletRed", 255, 219, 112, 147);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFEFD5.
        /// </summary>
        public static Color PapayaWhip { get; } = new Color("PapayaWhip", 255, 255, 239, 213);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFDAB9.
        /// </summary>
        public static Color PeachPuff { get; } = new Color("PeachPuff", 255, 255, 218, 185);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFCD853F.
        /// </summary>
        public static Color Peru { get; } = new Color("Peru", 255, 205, 133, 63);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFC0CB.
        /// </summary>
        public static Color Pink { get; } = new Color("Pink", 255, 255, 192, 203);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFDDA0DD.
        /// </summary>
        public static Color Plum { get; } = new Color("Plum", 255, 221, 160, 221);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFB0E0E6.
        /// </summary>
        public static Color PowderBlue { get; } = new Color("PowderBlue", 255, 176, 224, 230);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF800080.
        /// </summary>
        public static Color Purple { get; } = new Color("Purple", 255, 128, 0, 128);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF0000.
        /// </summary>
        public static Color Red { get; } = new Color("Red", 255, 255, 0, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFBC8F8F.
        /// </summary>
        public static Color RosyBrown { get; } = new Color("RosyBrown", 255, 188, 143, 143);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF4169E1.
        /// </summary>
        public static Color RoyalBlue { get; } = new Color("RoyalBlue", 255, 65, 105, 225);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF8B4513.
        /// </summary>
        public static Color SaddleBrown { get; } = new Color("SaddleBrown", 255, 139, 69, 19);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFA8072.
        /// </summary>
        public static Color Salmon { get; } = new Color("Salmon", 255, 250, 128, 114);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF4A460.
        /// </summary>
        public static Color SandyBrown { get; } = new Color("SandyBrown", 255, 244, 164, 96);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF2E8B57.
        /// </summary>
        public static Color SeaGreen { get; } = new Color("SeaGreen", 255, 46, 139, 87);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFF5EE.
        /// </summary>
        public static Color SeaShell { get; } = new Color("SeaShell", 255, 255, 245, 238);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFA0522D.
        /// </summary>
        public static Color Sienna { get; } = new Color("Sienna", 255, 160, 82, 45);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFC0C0C0.
        /// </summary>
        public static Color Silver { get; } = new Color("Silver", 255, 192, 192, 192);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF87CEEB.
        /// </summary>
        public static Color SkyBlue { get; } = new Color("SkyBlue", 255, 135, 206, 235);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF6A5ACD.
        /// </summary>
        public static Color SlateBlue { get; } = new Color("SlateBlue", 255, 106, 90, 205);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF708090.
        /// </summary>
        public static Color SlateGray { get; } = new Color("SlateGray", 255, 112, 128, 144);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFAFA.
        /// </summary>
        public static Color Snow { get; } = new Color("Snow", 255, 255, 250, 250);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF00FF7F.
        /// </summary>
        public static Color SpringGreen { get; } = new Color("SpringGreen", 255, 0, 255, 127);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF4682B4.
        /// </summary>
        public static Color SteelBlue { get; } = new Color("SteelBlue", 255, 70, 130, 180);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFD2B48C.
        /// </summary>
        public static Color Tan { get; } = new Color("Tan", 255, 210, 180, 140);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF008080.
        /// </summary>
        public static Color Teal { get; } = new Color("Teal", 255, 0, 128, 128);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFD8BFD8.
        /// </summary>
        public static Color Thistle { get; } = new Color("Thistle", 255, 216, 191, 216);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFF6347.
        /// </summary>
        public static Color Tomato { get; } = new Color("Tomato", 255, 255, 99, 71);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF40E0D0.
        /// </summary>
        public static Color Turquoise { get; } = new Color("Turquoise", 255, 64, 224, 208);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFEE82EE.
        /// </summary>
        public static Color Violet { get; } = new Color("Violet", 255, 238, 130, 238);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF5DEB3.
        /// </summary>
        public static Color Wheat { get; } = new Color("Wheat", 255, 245, 222, 179);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFFFF.
        /// </summary>
        public static Color White { get; } = new Color("White", 255, 255, 255, 255);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFF5F5F5.
        /// </summary>
        public static Color WhiteSmoke { get; } = new Color("WhiteSmoke", 255, 245, 245, 245);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FFFFFF00.
        /// </summary>
        public static Color Yellow { get; } = new Color("Yellow", 255, 255, 255, 0);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of #FF9ACD32.
        /// </summary>
        public static Color YellowGreen { get; } = new Color("YellowGreen", 255, 154, 205, 50);

        #endregion

        #region Name to color- table

        /// <summary>
        /// Internal method to generate the colortable if not already done
        /// </summary>
        static void GenerateColorsFromNameDict(Dictionary<string, Color> dict)
        {
            dict["transparent"] = Color.Transparent;
            dict["aliceblue"] = Color.AliceBlue;
            dict["antiquewhite"] = Color.AntiqueWhite;
            dict["aqua"] = Color.Aqua;
            dict["aquamarine"] = Color.Aquamarine;
            dict["azure"] = Color.Azure;
            dict["beige"] = Color.Beige;
            dict["bisque"] = Color.Bisque;
            dict["black"] = Color.Black;
            dict["blanchedalmond"] = Color.BlanchedAlmond;
            dict["blue"] = Color.Blue;
            dict["blueviolet"] = Color.BlueViolet;
            dict["brown"] = Color.Brown;
            dict["burlywood"] = Color.BurlyWood;
            dict["cadetblue"] = Color.CadetBlue;
            dict["chartreuse"] = Color.Chartreuse;
            dict["chocolate"] = Color.Chocolate;
            dict["coral"] = Color.Coral;
            dict["cornflowerblue"] = Color.CornflowerBlue;
            dict["cornsilk"] = Color.Cornsilk;
            dict["crimson"] = Color.Crimson;
            dict["cyan"] = Color.Cyan;
            dict["darkblue"] = Color.DarkBlue;
            dict["darkcyan"] = Color.DarkCyan;
            dict["darkgoldenrod"] = Color.DarkGoldenrod;
            dict["darkgray"] = Color.DarkGray;
            dict["darkgreen"] = Color.DarkGreen;
            dict["darkkhaki"] = Color.DarkKhaki;
            dict["darkmagenta"] = Color.DarkMagenta;
            dict["darkolivegreen"] = Color.DarkOliveGreen;
            dict["darkorange"] = Color.DarkOrange;
            dict["darkorchid"] = Color.DarkOrchid;
            dict["darkred"] = Color.DarkRed;
            dict["darksalmon"] = Color.DarkSalmon;
            dict["darkseagreen"] = Color.DarkSeaGreen;
            dict["darkslateblue"] = Color.DarkSlateBlue;
            dict["darkslategray"] = Color.DarkSlateGray;
            dict["darkturquoise"] = Color.DarkTurquoise;
            dict["darkviolet"] = Color.DarkViolet;
            dict["deeppink"] = Color.DeepPink;
            dict["deepskyblue"] = Color.DeepSkyBlue;
            dict["dimgray"] = Color.DimGray;
            dict["dodgerblue"] = Color.DodgerBlue;
            dict["firebrick"] = Color.Firebrick;
            dict["floralwhite"] = Color.FloralWhite;
            dict["forestgreen"] = Color.ForestGreen;
            dict["fuchsia"] = Color.Fuchsia;
            dict["gainsboro"] = Color.Gainsboro;
            dict["ghostwhite"] = Color.GhostWhite;
            dict["gold"] = Color.Gold;
            dict["goldenrod"] = Color.Goldenrod;
            dict["gray"] = Color.Gray;
            dict["green"] = Color.Green;
            dict["greenyellow"] = Color.GreenYellow;
            dict["honeydew"] = Color.Honeydew;
            dict["hotpink"] = Color.HotPink;
            dict["indianred"] = Color.IndianRed;
            dict["indigo"] = Color.Indigo;
            dict["ivory"] = Color.Ivory;
            dict["khaki"] = Color.Khaki;
            dict["lavender"] = Color.Lavender;
            dict["lavenderblush"] = Color.LavenderBlush;
            dict["lawngreen"] = Color.LawnGreen;
            dict["lemonchiffon"] = Color.LemonChiffon;
            dict["lightblue"] = Color.LightBlue;
            dict["lightcoral"] = Color.LightCoral;
            dict["lightcyan"] = Color.LightCyan;
            dict["lightgoldenrodyellow"] = Color.LightGoldenrodYellow;
            dict["lightgreen"] = Color.LightGreen;
            dict["lightgray"] = Color.LightGray;
            dict["lightpink"] = Color.LightPink;
            dict["lightsalmon"] = Color.LightSalmon;
            dict["lightseagreen"] = Color.LightSeaGreen;
            dict["lightskyblue"] = Color.LightSkyBlue;
            dict["lightslategray"] = Color.LightSlateGray;
            dict["lightsteelblue"] = Color.LightSteelBlue;
            dict["lightyellow"] = Color.LightYellow;
            dict["lime"] = Color.Lime;
            dict["limegreen"] = Color.LimeGreen;
            dict["linen"] = Color.Linen;
            dict["magenta"] = Color.Magenta;
            dict["maroon"] = Color.Maroon;
            dict["mediumaquamarine"] = Color.MediumAquamarine;
            dict["mediumblue"] = Color.MediumBlue;
            dict["mediumorchid"] = Color.MediumOrchid;
            dict["mediumpurple"] = Color.MediumPurple;
            dict["mediumseagreen"] = Color.MediumSeaGreen;
            dict["mediumslateblue"] = Color.MediumSlateBlue;
            dict["mediumspringgreen"] = Color.MediumSpringGreen;
            dict["mediumturquoise"] = Color.MediumTurquoise;
            dict["mediumvioletred"] = Color.MediumVioletRed;
            dict["midnightblue"] = Color.MidnightBlue;
            dict["mintcream"] = Color.MintCream;
            dict["mistyrose"] = Color.MistyRose;
            dict["moccasin"] = Color.Moccasin;
            dict["navajowhite"] = Color.NavajoWhite;
            dict["navy"] = Color.Navy;
            dict["oldlace"] = Color.OldLace;
            dict["olive"] = Color.Olive;
            dict["olivedrab"] = Color.OliveDrab;
            dict["orange"] = Color.Orange;
            dict["orangered"] = Color.OrangeRed;
            dict["orchid"] = Color.Orchid;
            dict["palegoldenrod"] = Color.PaleGoldenrod;
            dict["palegreen"] = Color.PaleGreen;
            dict["paleturquoise"] = Color.PaleTurquoise;
            dict["palevioletred"] = Color.PaleVioletRed;
            dict["papayawhip"] = Color.PapayaWhip;
            dict["peachpuff"] = Color.PeachPuff;
            dict["peru"] = Color.Peru;
            dict["pink"] = Color.Pink;
            dict["plum"] = Color.Plum;
            dict["powderblue"] = Color.PowderBlue;
            dict["purple"] = Color.Purple;
            dict["red"] = Color.Red;
            dict["rosybrown"] = Color.RosyBrown;
            dict["royalblue"] = Color.RoyalBlue;
            dict["saddlebrown"] = Color.SaddleBrown;
            dict["salmon"] = Color.Salmon;
            dict["sandybrown"] = Color.SandyBrown;
            dict["seagreen"] = Color.SeaGreen;
            dict["seashell"] = Color.SeaShell;
            dict["sienna"] = Color.Sienna;
            dict["silver"] = Color.Silver;
            dict["skyblue"] = Color.SkyBlue;
            dict["slateblue"] = Color.SlateBlue;
            dict["slategray"] = Color.SlateGray;
            dict["snow"] = Color.Snow;
            dict["springgreen"] = Color.SpringGreen;
            dict["steelblue"] = Color.SteelBlue;
            dict["tan"] = Color.Tan;
            dict["teal"] = Color.Teal;
            dict["thistle"] = Color.Thistle;
            dict["tomato"] = Color.Tomato;
            dict["turquoise"] = Color.Turquoise;
            dict["violet"] = Color.Violet;
            dict["wheat"] = Color.Wheat;
            dict["white"] = Color.White;
            dict["whitesmoke"] = Color.WhiteSmoke;
            dict["yellow"] = Color.Yellow;
            dict["yellowgreen"] = Color.YellowGreen;
        }

        static Dictionary<string, Color> colorsFromNameDict = null;

        #endregion
    }
}
