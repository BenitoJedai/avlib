using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AVLib.Utils;

namespace AVLib.Animations
{
    internal class SizeCalculator
    {
        private const double DecKoef = 2.5;
        private const double IncKoef = (double)1/DecKoef;

        private readonly int sizeChange;
        private SpeedMode speedMode;
        private int iterations;

        private double step;
        private int iterationsLeft;
        private double current;

        public SizeCalculator(int sizeChange, SpeedMode speedMode, int iterations)
        {
            this.sizeChange = sizeChange;
            this.speedMode = speedMode;
            this.iterations = iterations > 0 ? iterations : 1;
            this.iterationsLeft = iterations;

            current = 0;
            CalcFirst();
        }

        public int NextSize
        {
            get
            {
                int n = (int)current;
                CalcNext();
                if (sizeChange > 0 && n > sizeChange) n = sizeChange;
                if (sizeChange < 0 && n < sizeChange) n = sizeChange;
                return n;
            }
        }

        private double NotNormalStep(double koef)
        {
            if (iterationsLeft <= 0) return (sizeChange - current);
            double d = (sizeChange - current)/iterationsLeft * koef;
            if ((sizeChange < 0 && d > 0) || (sizeChange > 0 && d < 0)) d = 0;
            return d;
        }

        private void CalcFirst()
        {
            switch (speedMode)
            {
                case SpeedMode.Normal:
                    step = iterations <= 0 ? sizeChange : (double)sizeChange / iterations;
                    break;
                case SpeedMode.Dec:
                    step = NotNormalStep(DecKoef);
                    break;
                case SpeedMode.Inc:
                    step = NotNormalStep(IncKoef);
                    break;
            }
            current = step;
            iterationsLeft--;
            //Trace.WriteLine("Calc.First = " + current);
        }

        private void CalcNext()
        {
            switch (speedMode)
            {
                case SpeedMode.Normal:
                    current += step;
                    break;
                case SpeedMode.Dec:
                    step = NotNormalStep(DecKoef);
                    current += step;
                    break;
                case SpeedMode.Inc:
                    step = NotNormalStep(IncKoef);
                    current += step;
                    break;
            }
            iterationsLeft--;
            //Trace.WriteLine("Calc.Next = " + current);
        }
    }

    internal class StepProcesor
    {
        public class StepProcessorDirective
        {
            public bool Cancel = false;
            public int CurrentIteration = 0;
        }

        public delegate void StepProces(StepProcessorDirective directive);

        public event StepProces AfterSleep;

        private int iterations;
        private int time;

        private bool wasException = false;
        public bool WasException
        {
            get { return wasException; }
        }

        public StepProcesor(int iterations, int time)
        {
            this.iterations = iterations;
            this.time = time;
        }

        public void Start(StepProces method)
        {
            StepProcessorDirective directive = new StepProcessorDirective();
            int endTime = Environment.TickCount + time;
            int leftIterations = iterations;

            for (int i = 0; i < iterations; i++)
            {
                directive.CurrentIteration = i;
                try
                {
                    method(directive);
                    if (directive.Cancel) break;
                    leftIterations--;
                    int t = leftIterations <= 0 ? 0 : (endTime - Environment.TickCount) / leftIterations;
                    if (t > 0)
                    {
                        Thread.Sleep(t);
                        if (AfterSleep != null) AfterSleep(directive);
                        if (directive.Cancel) break;
                    }
                }
                catch (Exception ex)
                {
                    wasException = true;
                }
                if (wasException) break;
            }
        }
    }
}