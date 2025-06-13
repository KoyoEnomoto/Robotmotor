using Sciurus17.Control;
using System;
using System.Diagnostics;
using System.Threading;
using Sciurus17.Input;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sciurus17.Control;
    public class Threadsystem
    {
        ISciurusparts sciurusparts;
        IController Pad;
        public double ControlLooptime;
        public bool ControlRunnig = true;
        private readonly Stopwatch sw = Stopwatch.StartNew();
        private long freq = Stopwatch.Frequency;
        
        private dele_Cycle[] dele_Cycles;
        private double[] intervals;
        private double[] lastupdate;
        private List<dele_Cycle> List_dele_Cycle = new List<dele_Cycle>();
        private List<double> List_intervals = new List<double>();
        
        /// <summary>
        /// 周期で起動するデリゲートの追加
        /// interval=周期時間ミリ秒
        /// </summary>
        /// <param name="fuctions"></param>
        /// <param name="interval">周期時間ms</param>
        public void Addfunc( dele_Cycle fuctions, double interval)
        {
            List_dele_Cycle.Add(fuctions);
            List_intervals.Add(interval);
        }
        public void Start_Thread(Sciurusparts Robo, IController Pad)
        {
            dele_Cycles = List_dele_Cycle.ToArray();
            intervals = List_intervals.ToArray();
            lastupdate = new double[intervals.Length];

            sciurusparts = Robo;
            this.Pad = Pad;
            Thread Robo_core = new Thread(sciurusparts.Loop);
            Robo_core.Priority = ThreadPriority.Highest;
            Robo_core.Start();
            Thread control_core = new Thread(Control_Loop);
            control_core.Priority = ThreadPriority.Highest;
            control_core.Start();
            
            Console.WriteLine("SetSciurus_successful");

            Thread cycle_core = new Thread(Cycle_Loop);
            cycle_core.Priority = ThreadPriority.Highest;
            cycle_core.Start();

    }

        private void Cycle_Loop()
        {
            while (MainControlSystem.Runnig)
            {
                for (int i = 0; i < dele_Cycles.Length; i++)
                {
                    if( (double)sw.ElapsedTicks / freq * 1000.0 > intervals[i] + lastupdate[i])
                    {
                        dele_Cycles[i]();
                        lastupdate[i] = (double)sw.ElapsedTicks / freq * 1000.0;
                    }
                }
                Thread.Yield();
            }
            sciurusparts.Running = false;
            ControlRunnig = false;
            Console.WriteLine("Finish_Cycleloop");
        }

        private void Control_Loop()
        {
            double start = 0, end;
            while (ControlRunnig)
            {
                if (MainControlSystem.Cycle_bool)
                {
                    end = (double)sw.ElapsedTicks / freq * 1000.0;
                    ControlLooptime = end - start;
                    start = (double)sw.ElapsedTicks / freq * 1000.0;
                    Pad.Update();
                    MainControlSystem.Update();
                    MainControlSystem.Cycle_bool = false;
                }
                Thread.Yield();
            }
            Console.WriteLine("Finish_controlloop");
        }

}
