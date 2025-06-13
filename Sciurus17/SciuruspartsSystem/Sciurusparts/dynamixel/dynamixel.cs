/*******************************************************************************
* Copyright 2017 ROBOTIS CO., LTD.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*******************************************************************************/

/* Author: Ryu Woon Jung (Leon) */

using System;
using System.Runtime.InteropServices;

namespace Sciurus17.dynamixel
{
  class dynamixel
  {
    const string dll_path = ".\\dxl_x64_c.dll";

    #region PortHandler
    [DllImport(dll_path)]
    public static extern int    portHandler         (string port_name);

    [DllImport(dll_path)]
    public static extern bool   openPort            (int port_num);
    [DllImport(dll_path)]
    public static extern void   closePort           (int port_num);
    [DllImport(dll_path)]
    public static extern void   clearPort           (int port_num);

    [DllImport(dll_path)]
    public static extern void   setPortName         (int port_num, string port_name);
    [DllImport(dll_path)]
    public static extern string getPortName         (int port_num);

    [DllImport(dll_path)]
    public static extern bool   setBaudRate         (int port_num, int baudrate);
    [DllImport(dll_path)]
    public static extern int    getBaudRate         (int port_num);

    [DllImport(dll_path)]
    public static extern int    readPort            (int port_num, byte[] packet, int length);
    [DllImport(dll_path)]
    public static extern int    writePort           (int port_num, byte[] packet, int length);

    [DllImport(dll_path)]
    public static extern void   setPacketTimeout    (int port_num, ushort packet_length);
    [DllImport(dll_path)]
    public static extern void   setPacketTimeoutMSec(int port_num, double msec);
    [DllImport(dll_path)]
    public static extern bool   isPacketTimeout     (int port_num);
    #endregion

    #region PacketHandler
    [DllImport(dll_path)]
    public static extern void   packetHandler       ();

    [DllImport(dll_path)]
    public static extern nint getTxRxResult       (int protocol_version, int result);
    [DllImport(dll_path)]
    public static extern nint getRxPacketError    (int protocol_version, byte error);

    [DllImport(dll_path)]
    public static extern int    getLastTxRxResult   (int port_num, int protocol_version);
    [DllImport(dll_path)]
    public static extern byte   getLastRxPacketError(int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern void   setDataWrite        (int port_num, int protocol_version, ushort data_length, ushort data_pos, uint data);
    [DllImport(dll_path)]
    public static extern uint getDataRead         (int port_num, int protocol_version, ushort data_length, ushort data_pos);

    [DllImport(dll_path)]
    public static extern void   txPacket            (int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern void   rxPacket            (int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern void   txRxPacket          (int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern void   ping                (int port_num, int protocol_version, byte id);

    [DllImport(dll_path)]
    public static extern ushort pingGetModelNum     (int port_num, int protocol_version, byte id);

    [DllImport(dll_path)]
    public static extern void   broadcastPing       (int port_num, int protocol_version);
    [DllImport(dll_path)]
    public static extern bool   getBroadcastPingResult(int port_num, int protocol_version, int id);

    [DllImport(dll_path)]
    public static extern void   reboot              (int port_num, int protocol_version, byte id);

    [DllImport(dll_path)]
    public static extern void   factoryReset        (int port_num, int protocol_version, byte id, byte option);

    [DllImport(dll_path)]
    public static extern void   readTx              (int port_num, int protocol_version, byte id, ushort address, ushort length);
    [DllImport(dll_path)]
    public static extern void   readRx              (int port_num, int protocol_version, ushort length);
    [DllImport(dll_path)]
    public static extern void   readTxRx            (int port_num, int protocol_version, byte id, ushort address, ushort length);

    [DllImport(dll_path)]
    public static extern void   read1ByteTx         (int port_num, int protocol_version, byte id, ushort address);
    [DllImport(dll_path)]
    public static extern byte   read1ByteRx         (int port_num, int protocol_version);
    [DllImport(dll_path)]
    public static extern byte   read1ByteTxRx       (int port_num, int protocol_version, byte id, ushort address);

    [DllImport(dll_path)]
    public static extern void   read2ByteTx         (int port_num, int protocol_version, byte id, ushort address);
    [DllImport(dll_path)]
    public static extern ushort read2ByteRx         (int port_num, int protocol_version);
    [DllImport(dll_path)]
    public static extern ushort read2ByteTxRx       (int port_num, int protocol_version, byte id, ushort address);

    [DllImport(dll_path)]
    public static extern void   read4ByteTx         (int port_num, int protocol_version, byte id, ushort address);
    [DllImport(dll_path)]
    public static extern uint read4ByteRx         (int port_num, int protocol_version);
    [DllImport(dll_path)]
    public static extern uint read4ByteTxRx       (int port_num, int protocol_version, byte id, ushort address);

    [DllImport(dll_path)]
    public static extern void   writeTxOnly         (int port_num, int protocol_version, byte id, ushort address, ushort length);
    [DllImport(dll_path)]
    public static extern void   writeTxRx           (int port_num, int protocol_version, byte id, ushort address, ushort length);

    [DllImport(dll_path)]
    public static extern void   write1ByteTxOnly    (int port_num, int protocol_version, byte id, ushort address, byte data);
    [DllImport(dll_path)]
    public static extern void   write1ByteTxRx      (int port_num, int protocol_version, byte id, ushort address, byte data);

    [DllImport(dll_path)]
    public static extern void   write2ByteTxOnly    (int port_num, int protocol_version, byte id, ushort address, ushort data);
    [DllImport(dll_path)]
    public static extern void   write2ByteTxRx      (int port_num, int protocol_version, byte id, ushort address, ushort data);

    [DllImport(dll_path)]
    public static extern void   write4ByteTxOnly    (int port_num, int protocol_version, byte id, ushort address, uint data);
    [DllImport(dll_path)]
    public static extern void   write4ByteTxRx      (int port_num, int protocol_version, byte id, ushort address, uint data);

    [DllImport(dll_path)]
    public static extern void   regWriteTxOnly      (int port_num, int protocol_version, byte id, ushort address, ushort length);
    [DllImport(dll_path)]
    public static extern void   regWriteTxRx        (int port_num, int protocol_version, byte id, ushort address, ushort length);

    [DllImport(dll_path)]
    public static extern void   syncReadTx          (int port_num, int protocol_version, ushort start_address, ushort data_length, ushort param_length);
    // syncReadRx   -> GroupSyncRead
    // syncReadTxRx -> GroupSyncRead

    [DllImport(dll_path)]
    public static extern void   syncWriteTxOnly     (int port_num, int protocol_version, ushort start_address, ushort data_length, ushort param_length);

    [DllImport(dll_path)]
    public static extern void   bulkReadTx          (int port_num, int protocol_version, ushort param_length);
    // bulkReadRx   -> GroupBulkRead
    // bulkReadTxRx -> GroupBulkRead

    [DllImport(dll_path)]
    public static extern void   bulkWriteTxOnly     (int port_num, int protocol_version, ushort param_length);
    #endregion

    #region GroupBulkRead
    [DllImport(dll_path)]
    public static extern int    groupBulkRead       (int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern bool   groupBulkReadAddParam   (int group_num, byte id, ushort start_address, ushort data_length);
    [DllImport(dll_path)]
    public static extern void   groupBulkReadRemoveParam(int group_num, byte id);
    [DllImport(dll_path)]
    public static extern void   groupBulkReadClearParam (int group_num);

    [DllImport(dll_path)]
    public static extern void   groupBulkReadTxPacket   (int group_num);
    [DllImport(dll_path)]
    public static extern void   groupBulkReadRxPacket   (int group_num);
    [DllImport(dll_path)]
    public static extern void   groupBulkReadTxRxPacket (int group_num);

    [DllImport(dll_path)]
    public static extern bool   groupBulkReadIsAvailable(int group_num, byte id, ushort address, ushort data_length);
    [DllImport(dll_path)]
    public static extern uint groupBulkReadGetData    (int group_num, byte id, ushort address, ushort data_length);
    #endregion

    #region GroupBulkWrite
    [DllImport(dll_path)]
    public static extern int    groupBulkWrite            (int port_num, int protocol_version);

    [DllImport(dll_path)]
    public static extern bool   groupBulkWriteAddParam    (int group_num, byte id, ushort start_address, ushort data_length, uint data, ushort input_length);
    [DllImport(dll_path)]
    public static extern void   groupBulkWriteRemoveParam (int group_num, byte id);
    [DllImport(dll_path)]
    public static extern bool   groupBulkWriteChangeParam (int group_num, byte id, ushort start_address, ushort data_length, uint data, ushort input_length, ushort data_pos);
    [DllImport(dll_path)]
    public static extern void   groupBulkWriteClearParam  (int group_num);

    [DllImport(dll_path)]
    public static extern void   groupBulkWriteTxPacket    (int group_num);
    #endregion

    #region GroupSyncRead
    [DllImport(dll_path)]
    public static extern int    groupSyncRead             (int port_num, int protocol_version, ushort start_address, ushort data_length);

    [DllImport(dll_path)]
    public static extern bool   groupSyncReadAddParam     (int group_num, byte id);
    [DllImport(dll_path)]
    public static extern void   groupSyncReadRemoveParam  (int group_num, byte id);
    [DllImport(dll_path)]
    public static extern void   groupSyncReadClearParam   (int group_num);

    [DllImport(dll_path)]
    public static extern void   groupSyncReadTxPacket     (int group_num);
    [DllImport(dll_path)]
    public static extern void   groupSyncReadRxPacket     (int group_num);
    [DllImport(dll_path)]
    public static extern void   groupSyncReadTxRxPacket   (int group_num);

    [DllImport(dll_path)]
    public static extern bool   groupSyncReadIsAvailable  (int group_num, byte id, ushort address, ushort data_length);
    [DllImport(dll_path)]
    public static extern uint groupSyncReadGetData      (int group_num, byte id, ushort address, ushort data_length);
    #endregion

    #region GroupSyncWrite
    [DllImport(dll_path)]
    public static extern int    groupSyncWrite            (int port_num, int protocol_version, ushort start_address, ushort data_length);

    [DllImport(dll_path)]
    public static extern bool   groupSyncWriteAddParam    (int group_num, byte id, uint data, ushort data_length);
    [DllImport(dll_path)]
    public static extern void   groupSyncWriteRemoveParam (int group_num, byte id);
    [DllImport(dll_path)]
    public static extern bool   groupSyncWriteChangeParam (int group_num, byte id, uint data, ushort data_length, ushort data_pos);
    [DllImport(dll_path)]
    public static extern void   groupSyncWriteClearParam  (int group_num);

    [DllImport(dll_path)]
    public static extern void   groupSyncWriteTxPacket    (int group_num);
    #endregion
  }
}
