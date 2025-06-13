using Sciurus17;
using Sciurus17.Input;
using System;
using System.Threading;

namespace Sciurus17.Control;

public delegate void dele_Cycle();

    public static class MainControlSystem
    {
        public static ISciurusparts parts;
        public static IController Pad;
        public static Threadsystem threadsys;
        
        public static bool Runnig = true;
        public static bool Cycle_bool = false;
        /// <summary>
        /// 初期設定
        /// id，mode，ポートの設定
        /// </summary>
        public static void Main()
        {
            byte[] id = new byte[1] { 5}; ///動かしたいモーターのid
            byte[] mode = new byte[1] { 0};///モーターのオペレーションモードの設定　電流:0, 速度1, 位置3
            Pad = new ControllerOff();
            parts = new Sciurusparts(id, mode);
            parts.SetPort("COM3"); ///COMの設定
            parts.Motorboot();
            threadsys = new Threadsystem();
            dele_Cycle ScirusCycle = new (parts.Cycle_Update);
            dele_Cycle ControlCycle = new(MainControlSystem.Cycle_Update);
            threadsys.Addfunc(ScirusCycle, 1.0);///Sciurusの制御周期の設定ミリ秒
            threadsys.Addfunc(ControlCycle, 1.0);///controlの制御周期の設定ミリ秒
            threadsys.Start_Thread((Sciurusparts)parts, Pad);

            Console.ReadLine();
            Runnig = false;
        }

        private static void Cycle_Update() { Cycle_bool = true; }

    /// <summary>
    /// 更新部分
    /// ISciurusparts，IControllerインターフェイス参照
    /// </summary>
    /// 
        static double theta = 0.0, velo;
        public static void Update()
        {
            Console.WriteLine("current{0}: velo{1}: pos{2}", parts.Current[0], parts.Velocity[0], parts.Position[0]);
            Console.WriteLine("controllooptime{0}, Sciuruslooptime{1}", threadsys.ControlLooptime, parts.Looptime);
            velo = Pad.LeftThumbX;
        parts.SetGoalCurrent(5, 0.1 * (velo - parts.GetstateVelocity(5)));
/*        parts.SetGoalCurrent(5, Pad.LeftThumbX * 0.1);
*/
            if (Pad.ButtonStart) parts.Running = false;
        }
    }

