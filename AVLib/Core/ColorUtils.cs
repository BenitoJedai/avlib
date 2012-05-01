using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVLib.Utils
{
    public static class ColorUtils
    {
        #region private

        private const double MaxBytePercent = (byte.MaxValue * 0.01);


        private static byte DarkColorChannel(byte chanel, Single pct)
        {
            if (pct < 0)
                return BrightColorChannel(chanel, -pct);
            var tmp = Math.Round(chanel - pct * MaxBytePercent);
            return (tmp < byte.MinValue) ? byte.MinValue : (byte)tmp;
        }

        private static byte BrightColorChannel(byte chanel, Single pct)
        {
            if (pct < 0)
                return DarkColorChannel(chanel, -pct);
            var tmp = Math.Round(chanel + pct * MaxBytePercent);
            return (tmp > byte.MaxValue) ? byte.MaxValue : (byte)tmp;
        }

        private static byte MergeColorChanel(byte chanel, byte mergeChanel, Single mergeChanelPct)
        {
            double mergeKoef = (double)mergeChanelPct/100;
            if (mergeKoef < 0) return chanel;
            if (mergeKoef > 1) return mergeChanel;
            var tmp = Math.Round(chanel + (double)(mergeChanel - chanel) * mergeKoef);
            return (tmp > byte.MaxValue) ? byte.MaxValue : (byte)tmp;
        }

        private static byte OverlapChanel(byte chanel, byte overlapChanel)
        {
            return (byte)Math.Min(byte.MaxValue, chanel + Math.Max(0, (float)(byte.MaxValue - chanel) * (overlapChanel / byte.MaxValue)));
        }

        #endregion

        public static Color BrightColor(this Color color, Single pct)
        {
            return Color.FromArgb(color.A,
                                  BrightColorChannel(color.R, pct),
                                  BrightColorChannel(color.G, pct),
                                  BrightColorChannel(color.B, pct));
        }

        public static Color DarkColor(this Color color, Single pct)
        {
            return Color.FromArgb(color.A,
                                  DarkColorChannel(color.R, pct),
                                  DarkColorChannel(color.G, pct),
                                  DarkColorChannel(color.B, pct));
        }


        public static Color OverlapColor(this Color color, Color overlapColor)
        {
            return Color.FromArgb(OverlapChanel(color.A, overlapColor.A),
                                  OverlapChanel(color.R, overlapColor.R),
                                  OverlapChanel(color.G, overlapColor.G),
                                  OverlapChanel(color.B, overlapColor.B));
        }

        public static Color MergeColor(this Color color, Color mergeColor, Single mergeColorPct)
        {
            return Color.FromArgb(MergeColorChanel(color.A, mergeColor.A, mergeColorPct),
                                  MergeColorChanel(color.R, mergeColor.R, mergeColorPct),
                                  MergeColorChanel(color.G, mergeColor.G, mergeColorPct),
                                  MergeColorChanel(color.B, mergeColor.B, mergeColorPct));
        }

        public static Color Transparent(this Color color, Single pct)
        {
            if (pct > 0)
                return Color.FromArgb((byte)Math.Min(byte.MaxValue, 1 - pct/100*byte.MaxValue),
                                      color.R,
                                      color.G,
                                      color.B);
            else
                return color;
        }
    }
}
