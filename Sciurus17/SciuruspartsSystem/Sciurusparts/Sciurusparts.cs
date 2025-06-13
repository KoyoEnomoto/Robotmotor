using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Sciurus17;
using Sciurus17.dynamixel;
using static Sciurus17.Convert.SimpleConvert;


namespace Sciurus17
{
    public class Sciurusparts : ISciurusparts
    {
        private readonly Stopwatch sw = Stopwatch.StartNew();
        private long freq = Stopwatch.Frequency;
        private XMReadWrite xm { get; set; }
/*        private PHReadWrite ph { get; set; }*/

        public string name {  get; set; } 
        public byte[] Id { get; set; }
        public byte[] Mode { get; set; }
        public double[] PWM { get; set; }
        public double[] Position { get; set; }
        public double[] GoalPosition { get;
            set; }
        public double[] Velocity { get; set; }
        public double[] GoalVelocity { get; set; }
        public double[] Current { get; set; }
        public double[] GoalCurrent { get; set; }

        public double Looptime {  get; set; }
        public bool Running { get; set; } = true;//updateされているか確認する変数
        public bool Cycle_bool{ get; set; } = true;//updateされているか確認する変数


        public bool FlagCurrntMode { get; set; } = false;
        public bool FlagVelocityMode { get; set; } = false;
        public bool FlagPositionMode { get; set; } = false;

        public bool FlagfeedbackAll { get; set; } = false;

        short[] Current_bin;
        int[] Velocity_bin;
        int[] Position_bin;
        private byte[] Id_SendGoalPosition;
        private byte[] Id_SendGoalVelocity;
        private byte[] Id_SendGoalCurrent;
        private double[] SendGoalPosition;
        private double[] SendGoalVelocity;
        private double[] SendGoalCurrent;
        int countMode0;
        int countMode1;
        int countMode3;
        private int CountMakesend0;
        private int CountMakesend1;
        private int CountMakesend3;

        private List<int[]> data;
        private double[] feedback_state;

        public Sciurusparts(byte[] id, byte[] mode) ///idは配列で設定 //portも指定
        {
            this.name = name;
            Id = new byte[id.Length];
            Id = id;
            Mode = new byte[id.Length];
            Mode = mode;
            Position = new double[id.Length];
            GoalPosition = new double[id.Length];
            Velocity = new double[id.Length];
            GoalVelocity = new double[id.Length];
            Current = new double[id.Length];
            GoalCurrent = new double[id.Length];
            Current_bin = new short[id.Length];
            Velocity_bin = new int[id.Length];
            Position_bin = new int[id.Length];
            SetSendGoalstate();
            
            xm = new XMReadWrite(id, mode, Id_SendGoalCurrent, Id_SendGoalVelocity, Id_SendGoalPosition);
        }

        public void Cycle_Update() { Cycle_bool = true; }

        public void Loop()///更新
        {
            double start = 0, end;
            while (Running)
            {
                if (Cycle_bool)
                {
                    end = (double)sw.ElapsedTicks / freq * 1000.0;
                    Looptime = end - start;
                    start = (double)sw.ElapsedTicks / freq * 1000.0;
                    Feedback_all();   ///すべてのフィードバックを受け取る
                    MakeSendGoalstate();
                    SendGoalState();
                    Cycle_bool = false;
                }
                Thread.Yield();
            }
            Motoroff();
            ClosePort();
            Console.WriteLine("Finish_Robotloop");
        }

        private void Feedback_all()///モーターの情報を取得
        {
            xm.Feedback_PresentCurrVeloPosi(ref Current_bin, ref Velocity_bin, ref Position_bin);
            Current = ConvertValueIntoCurrent(Current_bin);
            Velocity = ConvertValueIntoVelocity(Velocity_bin);
            Position = ConvertValueIntoPosition(Position_bin);
        }

        private void SetSendGoalstate()
        {
            /*foreach (var item in Mode)Console.WriteLine("Mode{0}",item);*/
            
            countMode0 = Mode.Count(item => item == 0);
            if (countMode0 > 0)
            {
                SendGoalCurrent = new double[countMode0];
                Id_SendGoalCurrent = new byte[countMode0];
                FlagCurrntMode = true;
            }

            countMode1 = Mode.Count(item => item == 1);
            if (countMode1 > 0)
            {
                SendGoalVelocity = new double[countMode1];
                Id_SendGoalVelocity = new byte[countMode1];
                FlagVelocityMode = true;
            }

            countMode3 = Mode.Count(item => item == 3);
            if (countMode3 > 0)
            {
                SendGoalPosition = new double[countMode3];
                Id_SendGoalPosition = new byte[countMode3];
                FlagPositionMode = true;
            }
        }


        private void MakeSendGoalstate()
        {

            for (int i = 0; i < Id.Length; i++)
            {
                switch (Mode[i])
                {
                    case 0:
                        Id_SendGoalCurrent[CountMakesend0] = Id[i];
                        SendGoalCurrent[CountMakesend0] = GoalCurrent[i];
                        CountMakesend0++;
                        break;
                    case 1:
                        Id_SendGoalVelocity[CountMakesend1] = Id[i];
                        SendGoalVelocity[CountMakesend1] = GoalVelocity[i];
                        CountMakesend1++;
                        break;
                    case 3:
                        Id_SendGoalPosition[CountMakesend3] = Id[i];
                        SendGoalPosition[CountMakesend3] = GoalPosition[i];
                        CountMakesend3++;
                        break;
                    default:
                        throw new InvalidOperationException($"無効なモード値: {Mode[i]}");
                }
            }
            CountMakesend0 = 0; CountMakesend1 = 0; CountMakesend3 = 0;
        }


        private void SendGoalState()
        {
            if (FlagCurrntMode)
            {
                xm.GoalCurrent(SendGoalCurrent);
            }
            if (FlagVelocityMode)
            {
                xm.GoalVelocity(SendGoalVelocity);
            }
            if (FlagPositionMode)
            {
                xm.GoalPosition(SendGoalPosition);
            }
        }
        public void SetPort(string port) => xm.Openport(port);

        public void ClosePort() => xm.Closeport();

        public void Motorboot() => xm.Motorboot();

        public void Motoroff() => xm.MotorOff();

        
        ///これより下インターフェイスのメソッド
        
        public double GetstatePosition(byte id)
        {
            return Position[Array.IndexOf(Id, id)];
        }

        public double[] GetstatePosition(byte[] id)
        {
            feedback_state = new double[id.Length];
            for (int i = 0; i < id.Length; i++)
            {
                feedback_state[i] = Position[Array.IndexOf(Id, id[i])];
            }
            return feedback_state;
        }

        public double GetstateVelocity(byte id)
        {
            return Velocity[Array.IndexOf(Id, id)];
        }

        public double[] GetstateVelocity(byte[] id)
        {
            feedback_state = new double[id.Length];
            for (int i = 0; i < id.Length; i++)
            {
                feedback_state[i] = Velocity[Array.IndexOf(Id, id[i])];
            }
            return feedback_state;
        }

        public double GetstateCurrent(byte id)
        {
            return Current[Array.IndexOf(Id, id)];
        }

        public double[] GetstateCurrent(byte[] id)
        {
            feedback_state = new double[id.Length];
            for (int i = 0; i < id.Length; i++)
            {
                feedback_state[i] = Current[Array.IndexOf(Id, id[i])];
            }
            return feedback_state;
        }

        public void SetGoalPosition(byte id, double position)
        {
            GoalPosition[Array.IndexOf(Id, id)] = position;
        }

        public void SetGoalPosition(byte[] id, double[] position)
        {
            for (int i = 0; i < id.Length; i++)
            {
                GoalPosition[Array.IndexOf(Id, id[i])] = position[i];
            }
        }

        public void SetGoalVelocity(byte id, double velocity)
        {
            GoalVelocity[Array.IndexOf(Id, id)] = velocity;

        }
        

        public void SetGoalVelocity(byte[] id, double[] velocity)
        {
            for (int i = 0; i < id.Length; i++)
            {
                GoalVelocity[Array.IndexOf(Id, id[i])] = velocity[i];
            }
        }

        public void SetGoalCurrent(byte id, double currnt)
        {
            GoalCurrent[Array.IndexOf(Id, id)] = currnt;
        }

        public void SetGoalCurrent(byte[] id, double[] currnt)
        {
            for (int i = 0; i < id.Length ; i++)
            {
                GoalCurrent[Array.IndexOf(Id, id[i])] = currnt[i];
            }
        }

        void ISciurusparts.Sciurusparts(byte[] id, byte[] mode)
        {
            throw new NotImplementedException();
        }
    }




}
