namespace Sciurus17
{
    public interface ISciurusparts ///SciurusのRightArm,Spin,LeftArmのインターフェイス
    {
        void Sciurusparts(byte[] id, byte[] mode);
        string name {  get; set; }
        byte[] Id { get; set; }
        byte[] Mode { get; set; }
        double[] Position { get; set; }
        double[] GoalPosition { get; set; }
        double[] Velocity { get; set; }
        double[] GoalVelocity { get; set; }
        double[] Current { get; set; }
        double[] GoalCurrent { get; set; }
        double[] PWM { get; set; }
        bool Running { get; set; }

        double Looptime {  get; set; }
        void Motorboot(); ///モーターのモードを指定して起動
        void Motoroff();

        void Loop();
        void Cycle_Update();

        void SetPort(string comport);

        void ClosePort();

        double GetstatePosition(byte id);

        double[] GetstatePosition(byte[] id);

        double GetstateVelocity(byte id);

        double[] GetstateVelocity(byte[] id);

        double GetstateCurrent(byte id);

        double[] GetstateCurrent(byte[] id);

        void SetGoalPosition(byte id, double position);

        void SetGoalPosition(byte[] id, double[] position);

        void SetGoalVelocity(byte id, double velocity);

        void SetGoalVelocity(byte[] id, double[] velocity);

        void SetGoalCurrent(byte id, double currnt);

        void SetGoalCurrent(byte[] id, double[] currnt);

    }
}
