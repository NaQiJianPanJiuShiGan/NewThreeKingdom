using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class Protocol {
    /// <summary>
    /// 编码格式
    /// </summary>
    private Encoding encodelUtf8 = Encoding.UTF8;
    public byte[] recvBuffer;
    public byte[] sendBuffer;

    public int recvPos = 0;
    public int sendPos = 0;

    public virtual void WriteMsgBegin(string begin="MesBegin")
    {

    }
    public virtual void WriteMsgEnd(string end = "MegEnd")
    {

    }
    /// <summary>
    /// 高低位转换
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="isReveres"></param>
    public void ByteReverse(byte[] bytes,bool isReveres= true)
    {
        if (isReveres)
        {
            Array.Reverse(bytes);
        }
    }

    public void Write(byte[] bytes, int len,bool isReverse =true)
    {
        ByteReverse(bytes);
        Array.Copy(bytes,0,sendBuffer,sendPos,len);
        sendPos += len;
    }

    public void WriteBool(bool value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        Write(bytes,1);
    }

    public void WriteUInt8(byte i)
    {
        sendBuffer[sendPos] = i;
        sendPos += 1;
    }
    public void WriteInt8(sbyte i)
    {
        sendBuffer[sendPos] = (byte)i;
        sendPos += 1;
    }
    public void WriteUInt16(ushort i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Write(bytes, 2);
    }
    public void WriteInt16(short i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Write(bytes, 2);
    }
    public void WriteUInt32(uint i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Write(bytes, 4);
    }
    public void WriteInt32(int i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Write(bytes,4);
    }
    public void WriteInt64(long i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Write(bytes, 8);
    }
    public void WriteFloat(float f)
    {
        byte[] bytes = BitConverter.GetBytes(f);
        Write(bytes, 4);
    }

    public void WriteString(string s,bool isReverse=true)
    {
        byte[] bytes = encodelUtf8.GetBytes(s);
        WriteInt16((short)bytes.Length);//先将字符串长度写入
        byte[] byteLen = BitConverter.GetBytes((short)bytes.Length);
        Write(bytes, bytes.Length, isReverse);
    }
    public bool ReadBool()
    {
        bool temp = BitConverter.ToBoolean(recvBuffer, recvPos);
        recvPos += 1;
        return temp;
    }
    public byte ReadUint8()
    {
        byte temp = recvBuffer[recvPos];
        recvPos += 1;
        return temp;
    }
    public sbyte ReadInt8()
    {
        sbyte temp = (sbyte)(recvBuffer[recvPos]);
        recvPos += 1;
        return temp;
    }
    public ushort ReadUInt16()
    {
        ushort temp = BitConverter.ToUInt16(recvBuffer, recvPos);
        recvPos += 2;
        return temp;
    }
    public Int16 ReadInt16()
    {
        Int16 temp = BitConverter.ToInt16(recvBuffer, recvPos);
        recvPos += 2;
        return temp;
    }
    public UInt32 ReadUInt32()
    {
        UInt32 temp = BitConverter.ToUInt32(recvBuffer, recvPos);
        recvPos += 4;
        return temp;
    }
    public Int32 ReadInt32()
    {
        Int32 temp = BitConverter.ToInt32(recvBuffer, recvPos);
        recvPos += 4;
        return temp;
    }
    public long ReadInt64()
    {
        long temp = BitConverter.ToInt64(recvBuffer, recvPos);
        recvPos += 8;
        return temp;
    }
    public float ReadFloat()
    {
        float temp = BitConverter.ToSingle(recvBuffer, recvPos);
        recvPos += 4;
        return temp;
    }
    public string ReadString()
    {
        short Len =ReadInt16();//先取出字符串的长度
        byte[] lenByteLen = BitConverter.GetBytes(Len);
        Array.Reverse(lenByteLen);
        string temp = encodelUtf8.GetString(recvBuffer, recvPos, Len);
        recvPos += Len;
        return temp;
    }
    public string encodeDl = "yyMMdd";
    public int ReadString2()
    {
        string temp;
        temp = DateTime.Now.ToString(encodeDl);
        int temp1 = System.Int32.Parse(temp);
        return temp1;
    }
}
