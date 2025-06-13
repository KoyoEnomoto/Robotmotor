namespace Sciurus17.Input
{
    public interface  IController
    {
        ///-1～1の値
        double RightThumbX { get; set; }
        double RightThumbY { get; set; }
        double LeftThumbX { get; set; }
        double LeftThumbY { get; set; }
        double RightTrigger  { get; set; }
        double LeftTrigger { get; set; }

        ///ボタンを押している間trueとなる
        bool DPadUp { get; set; }
        bool DPadDown { get; set; } 
        bool DPadRight { get; set; }
        bool DPadLeft { get; set; }
        bool RightShoulder {  get; set; }
        bool LeftShoulder { get; set; }
        bool ButtonX { get; set; }
        bool ButtonY { get; set; }
        bool ButtonA { get; set; }
        bool ButtonB { get; set; }
        bool ButtonStart { get; set; }
        bool ButtonBack { get; set; }

        ///ボタンを押した瞬間のみtrueになる
        bool DPadUp_keydown { get; set; }
        bool DPadDown_keydown { get; set; }
        bool DPadRight_keydown { get; set; }
        bool DPadLeft_keydown { get; set; }
        bool RightShoulder_keydown { get; set; }
        bool LeftShoulder_keydown { get; set; }
        bool ButtonX_keydown { get; set; }
        bool ButtonY_keydown { get; set; }
        bool ButtonA_keydown { get; set; }
        bool ButtonB_keydown { get; set; }
        bool ButtonStart_keydown { get; set; }
        bool ButtonBack_keydown { get; set; }



        bool Connect { get; set; }
        void Update();
    }
}