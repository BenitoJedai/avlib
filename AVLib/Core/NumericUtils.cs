using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVLib.Utils
{
    public static class NumericUtils
    {
        public static int StepTo(this int value, double to, double step)
        {
            if (value == null) return 0;
            if (step <= 0) return 0;
            var dif = (to - value);
            if (dif >= 0) return (int)Math.Min(step, dif);
            return (int)Math.Max(-step, dif);
        }

        public static int NearBy(this int value, double to, double by)
        {
            if (value == null) return 0;
            var v = value + value.StepTo(to, by);
            return v;
        }

        public static int StepCount(this int value, double to, double by)
        {
            if (value == null) return 0;
            if (by == 0) return 0;
            int n = (int)((to - value)/by);
            if (to != value + n * by) return Math.Abs(n) + 1;
            return Math.Abs(n);
        }
    }
}
