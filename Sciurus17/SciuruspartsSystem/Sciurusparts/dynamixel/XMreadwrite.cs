///*******************************************************************************
//* Copyright 2017 ROBOTIS CO., LTD.
//*
//* Licensed under the Apache License, Version 2.0 (the "License");
//* you may not use this file except in compliance with the License.
//* You may obtain a copy of the License at
//*
//*     http://www.apache.org/licenses/LICENSE-2.0
//*
//* Unless required by applicable law or agreed to in writing, software
//* distributed under the License is distributed on an "AS IS" BASIS,
//* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//* See the License for the specific language governing permissions and
//* limitations under the License.
//*******************************************************************************/

///* Author: Ryu Woon Jung (Leon) */

////
//// *********     Read and Write Example      *********
////
////
//// Available Dynamixel model on this example : All models using Protocol 2.0
//// This example is designed for using a Dynamixel PRO 54-200, and an USB2DYNAMIXEL.
//// To use another Dynamixel model, such as X series, see their details in E-Manual(emanual.robotis.com) and edit below variables yourself.
//// Be sure that Dynamixel PRO properties are already set as %% ID : 1 / Baudnum : 1 (Baudrate : 57600)
////

using System;
using System.Threading;
using static Sciurus17.Convert.SimpleConvert;

namespace Sciurus17.dynamixel
{
    class XMReadWrite
    {
        // RAM領域のアドレス　Control table address
        private const int ADDR_TORQUE_ENABLE = 64;
        private const int LEN_TORQUE_ENABLE = 1;

        private const int ADDR_OPERATING_MODE = 11;
        private const int LEN_OPERATING_MODE = 1;

        private const int ADDR_GOAL_CURRENT = 102;
        private const int LEN_GOAL_CURRENT = 2;
        private const int ADDR_GOAL_VELOCITY = 104;
        private const int LEN_GOAL_VELOCITY = 4;
        private const int ADDR_GOAL_POSITION = 116;
        private const int LEN_GOAL_POSITION = 4;

        private const int ADDR_PRESENT_CURRENT = 126;
        private const int LEN_PRESENT_CURRENT = 2;
        private const int ADDR_PRESENT_VELOCITY = 128;
        private const int LEN_PRESENT_VELOCITY = 4;
        private const int ADDR_PRESENT_POSITION = 132;
        private const int LEN_PRESENT_POSITION = 4;
        // Protocol version
        private const int PROTOCOL_VERSION = 2;                   // protocol versionの設定

        // Default setting
        private const int BAUDRATE = 4500000; ///バンドレート　ダイナミクセルと合わせないと動かない

        private string COMPort;
        private int port_num;
        private byte[] ID;                   // Dynamixel ID:
        private int[] Mode;

        private int[] TORQUE_ENABLE;
        private int[] TORQUE_DISENABLE;

        int group_Operateting_num;  ///モードの決定
        int group_TORQUEON_num;     ///トルクのオン
        int group_TORQUEOFF_num;    ///トルクのオフ

        int group_GoalCurrent_num;      ///目標電流の設定
        int group_GoalVelocity_num;     ///目標速度の設定
        int group_GoalPosition_num;     ///目標位置の設定

        int group_Feedback_num;     ///フィードバックの設定

        int[] GoalCurrent_bin;
        int[] GoalVelocity_bin;
        int[] GoalPosition_bin;

        private byte[] Id_SendGoalPosition;
        private byte[] Id_SendGoalVelocity;
        private byte[] Id_SendGoalCurrent;

        public XMReadWrite(byte[] id, byte[] mode, byte[] Id_SendGoalCurrent, byte[] Id_SendGoalVelocity, byte[] Id_SendGoalPosition)
        {
            ID = id;
            Mode = new int[id.Length];
            TORQUE_ENABLE = new int[id.Length];
            TORQUE_DISENABLE = new int[id.Length];
            for (int i = 0; i < id.Length; i++)
            {
                Mode[i] = mode[i];
                TORQUE_ENABLE[i] = 1;
                TORQUE_DISENABLE[i] = 0;
            }

            this.Id_SendGoalCurrent = Id_SendGoalCurrent;
            this.Id_SendGoalVelocity = Id_SendGoalVelocity;
            this.Id_SendGoalPosition = Id_SendGoalPosition;
        }
        public void Openport(string portname)
        {
            // 初期化
            COMPort = portname;
            port_num = dynamixel.portHandler(portname);
            Console.WriteLine($"使用ポート: {COMPort}, ハンドル: {port_num}");
            dynamixel.packetHandler();

            if (!dynamixel.openPort(port_num))
            {
                Console.WriteLine("Failed to open port");
                return;
            }

            if (!dynamixel.setBaudRate(port_num, BAUDRATE))
            {
                Console.WriteLine("Failed to set baudrate");
                return;
            }

            ///インスタンスの生成
            group_Operateting_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_OPERATING_MODE, LEN_OPERATING_MODE);
            group_TORQUEON_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_TORQUE_ENABLE, LEN_TORQUE_ENABLE);
            group_TORQUEOFF_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_TORQUE_ENABLE, LEN_TORQUE_ENABLE);
            group_GoalCurrent_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_GOAL_CURRENT, LEN_GOAL_CURRENT);
            group_GoalVelocity_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_GOAL_VELOCITY, LEN_GOAL_VELOCITY);
            group_GoalPosition_num = dynamixel.groupSyncWrite(port_num, PROTOCOL_VERSION, ADDR_GOAL_POSITION, LEN_GOAL_POSITION);

            group_Feedback_num = dynamixel.groupSyncRead(port_num, PROTOCOL_VERSION, ADDR_PRESENT_CURRENT, LEN_PRESENT_CURRENT + LEN_PRESENT_VELOCITY + LEN_PRESENT_POSITION);

            GroupAddReadParam(group_Feedback_num);
            Thread.Sleep(500);  // Wait motion
            ///フィードバックの設定
        }

        public void Closeport()
        {
            // 初期化
            dynamixel.closePort(port_num);
        }

        public void Motorboot() ///
        {
            Console.WriteLine($"ID: {ID[0]}, ADDR: {ADDR_PRESENT_CURRENT}, LEN: {LEN_PRESENT_CURRENT}");
/*            Console.WriteLine($"ID: {ID[1]}, ADDR: {ADDR_PRESENT_CURRENT}, LEN: {LEN_PRESENT_CURRENT}");
*/            GroupAddWriteParam(group_TORQUEOFF_num, TORQUE_DISENABLE, LEN_TORQUE_ENABLE);  ///トルクオフの設定
            Console.WriteLine(1);
            dynamixel.groupSyncWriteTxPacket(group_TORQUEOFF_num);
            dynamixel.groupSyncWriteClearParam(group_TORQUEOFF_num);
            Thread.Sleep(100);
            if (dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION) != 0) Console.WriteLine("Failed to transmit bootpacket");

            GroupAddWriteParam(group_Operateting_num, Mode, LEN_OPERATING_MODE);            ///オペレーションのパラメータの設定
            Console.WriteLine(2);
            dynamixel.groupSyncWriteTxPacket(group_Operateting_num);
            dynamixel.groupSyncWriteClearParam(group_Operateting_num);
            Thread.Sleep(100);
            if (dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION) != 0) Console.WriteLine("Failed to transmit bootpacket");

            GroupAddWriteParam(group_TORQUEON_num, TORQUE_ENABLE, LEN_TORQUE_ENABLE);       ///トルクオンの設定
            Console.WriteLine(3);
            dynamixel.groupSyncWriteTxPacket(group_TORQUEON_num);
            dynamixel.groupSyncWriteClearParam(group_TORQUEON_num);
            if (dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION) != 0) Console.WriteLine("Failed to transmit bootpacket");
            

            dynamixel.groupSyncWriteClearParam(group_TORQUEOFF_num);
            dynamixel.groupSyncWriteClearParam(group_Operateting_num);

        }

        public void MotorOff() 
        {
            dynamixel.groupSyncWriteTxPacket(group_TORQUEOFF_num);
            Thread.Sleep(10);
            dynamixel.groupSyncWriteClearParam(group_TORQUEOFF_num);
        }

        public void Feedback_PresentCurrVeloPosi(ref short[] Current_bin, ref int[] Velocity_bin, ref int[] Position_bin)
        {
            dynamixel.groupSyncReadTxRxPacket(group_Feedback_num);

            for (int i = 0; i < ID.Length; i++)
            {
                if (dynamixel.groupSyncReadIsAvailable(group_Feedback_num, ID[i], ADDR_PRESENT_CURRENT, LEN_PRESENT_CURRENT))
                    Current_bin[i] = (short)dynamixel.groupSyncReadGetData(group_Feedback_num, ID[i], ADDR_PRESENT_CURRENT, LEN_PRESENT_CURRENT);
                if (dynamixel.groupSyncReadIsAvailable(group_Feedback_num, ID[i], ADDR_PRESENT_VELOCITY, LEN_PRESENT_VELOCITY))
                    Velocity_bin[i] = (int)dynamixel.groupSyncReadGetData(group_Feedback_num, ID[i], ADDR_PRESENT_VELOCITY, LEN_PRESENT_VELOCITY);
                if (dynamixel.groupSyncReadIsAvailable(group_Feedback_num, ID[i], ADDR_PRESENT_POSITION, LEN_PRESENT_POSITION))
                    Position_bin[i] = (int)dynamixel.groupSyncReadGetData(group_Feedback_num, ID[i], ADDR_PRESENT_POSITION, LEN_PRESENT_POSITION);
            }
        }

        public void GoalCurrent(double[] Goalcurrent)
        {
            GoalCurrent_bin = ConvertCurrentIntoValue(Goalcurrent);
            GroupAddWriteParam(group_GoalCurrent_num, GoalCurrent_bin, LEN_GOAL_CURRENT);            ///オペレーションのパラメータの設定
            dynamixel.groupSyncWriteTxPacket(group_GoalCurrent_num);
            dynamixel.groupSyncWriteClearParam(group_GoalCurrent_num);
        }

        public void GoalVelocity(double[] Goalvelocity)
        {
            GoalVelocity_bin = ConvertVelocityIntoValue(Goalvelocity);
            GroupAddWriteParam(group_GoalVelocity_num, GoalVelocity_bin, LEN_GOAL_VELOCITY);            ///オペレーションのパラメータの設定
            dynamixel.groupSyncWriteTxPacket(group_GoalVelocity_num);
            dynamixel.groupSyncWriteClearParam(group_GoalVelocity_num);
        }

        public void GoalPosition(double[] Goalposition)
        {
            GoalPosition_bin = ConvertPositionIntoValue(Goalposition);
            GroupAddWriteParam(group_GoalPosition_num, GoalPosition_bin, LEN_GOAL_POSITION);            ///オペレーションのパラメータの設定
            dynamixel.groupSyncWriteTxPacket(group_GoalPosition_num);
            dynamixel.groupSyncWriteClearParam(group_GoalPosition_num);
        }
        byte[] id;
        private void GroupAddWriteParam(int group_num, int[] data, ushort data_length)
        {
            if (group_num == group_GoalCurrent_num) id = Id_SendGoalCurrent;
            else if (group_num == group_GoalVelocity_num) id = Id_SendGoalVelocity;
            else if (group_num == group_GoalPosition_num) id = Id_SendGoalPosition;
            else id = ID;

            for (int i = 0; i < id.Length; i++)
            {                                                       
                byte[] param = BitConverter.GetBytes(data[i]);  // Little Endian
                uint a = BitConverter.ToUInt32(param, 0);
                bool addparam_result = dynamixel.groupSyncWriteAddParam(group_num, id[i], a, data_length);
                if (!addparam_result)
                {
                    Console.WriteLine($"Failed to add writeparameter for DYNAMIXEL ID {ID[i]}");
                    throw new InvalidOperationException($"groupSyncWriteAddParam failed for ID {ID[i]}");
                }
            }
        }
        private void GroupAddReadParam(int group_num)
        {
            for (int i = 0; i < ID.Length; i++)
            {
                bool addparam_result = dynamixel.groupSyncReadAddParam(group_num, ID[i]);
                if (!addparam_result)
                {
                    Console.WriteLine($"Failed to add readparameter for DYNAMIXEL ID {ID[i]}");
                }

            }

        }

    }
}