﻿// Decompiled with JetBrains decompiler
// Type: .
// Assembly: Xtra.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9ECEE879-7EA6-4124-AFF1-825108ED52BD
// Assembly location: C:\Project\HH.CanOTo\references\CAN TRI VIET\Xtra.Data.dll

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace \u0002
{
  internal class \u0001
  {
    private static readonly string \u0001;
    private static readonly string \u0002;
    private static readonly byte[] \u0003;
    private static readonly Hashtable \u0004;
    private static readonly bool \u0005;
    private static readonly int \u0006;

    public static string \u0001(int _param0)
    {
      int index1;
      int num1;
      int count;
      string str1;
      int num2;
      do
      {
        int num3 = _param0;
        int num4;
        while (true)
        {
          int num5 = \u0002.\u0001.\u0006;
          _param0 = num3 - num5;
          do
          {
            if (\u0002.\u0001.\u0005)
            {
              str1 = (string) \u0002.\u0001.\u0004[(object) _param0];
              if (str1 == null)
                goto label_5;
            }
            else
              goto label_5;
          }
          while (false);
          goto label_27;
label_5:
          int num6 = _param0;
          byte[] numArray = \u0002.\u0001.\u0003;
          int index2 = num6;
          index1 = index2 + 1;
          num1 = (int) numArray[index2];
          num4 = num1 & 128;
          if (false)
            num3 = num4;
          else
            break;
        }
        if (num4 == 0)
        {
          num2 = num1;
          if (true)
          {
            count = num2;
            if (count != 0)
              goto label_21;
          }
          else
            goto label_28;
        }
        else
          goto label_11;
      }
      while (false);
      return string.Empty;
label_11:
      int num7;
      if ((num1 & 64) == 0)
      {
        int num8 = num1 & 63;
        if (true)
          num8 <<= 8;
        int num9 = (int) \u0002.\u0001.\u0003[index1++];
        count = num8 + num9;
        goto label_21;
      }
      else
        num7 = num1;
label_16:
      int num10 = (num7 & 31) << 24;
      byte[] numArray1 = \u0002.\u0001.\u0003;
      int index3 = index1;
      int num11 = index3 + 1;
      int num12 = (int) numArray1[index3];
      int num13;
      if (true)
        num13 = 16;
      else
        goto label_19;
label_18:
      int num14 = num12 << num13;
      int num15 = num10 + num14;
      num12 = (int) \u0002.\u0001.\u0003[num11++];
      num10 = num15;
label_19:
      num13 = 8;
      if (num13 != 0)
      {
        int num16 = num12 << num13;
        int num17 = num10 + num16;
        byte[] numArray2 = \u0002.\u0001.\u0003;
        int index4 = num11;
        index1 = index4 + 1;
        int num18 = (int) numArray2[index4];
        count = num17 + num18;
        goto label_21;
      }
      else
        goto label_18;
label_28:
      num7 = num2;
      goto label_16;
label_21:
      try
      {
        byte[] bytes = Convert.FromBase64String(Encoding.UTF8.GetString(\u0002.\u0001.\u0003, index1, count));
        string str2 = string.Intern(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
        if (\u0002.\u0001.\u0005)
        {
          try
          {
            \u0002.\u0001.\u0004.Add((object) _param0, (object) str2);
          }
          catch
          {
          }
        }
        return str2;
      }
      catch
      {
        return (string) null;
      }
label_27:
      return str1;
    }

    static \u0001()
    {
      while (true)
      {
        \u0002.\u0001.\u0001 = "0";
        if (true)
        {
          \u0002.\u0001.\u0002 = "150";
          if (true)
          {
            \u0002.\u0001.\u0003 = (byte[]) null;
            \u0002.\u0001.\u0004 = (Hashtable) null;
            int num1 = 0;
            int num2;
            while (true)
            {
              \u0002.\u0001.\u0005 = num1 != 0;
              num2 = 0;
              if (num2 != 0)
                num1 = num2;
              else
                break;
            }
            \u0002.\u0001.\u0006 = num2;
            if (\u0002.\u0001.\u0001 == "1")
            {
              if (true)
              {
                \u0002.\u0001.\u0005 = true;
                if (true)
                {
                  \u0002.\u0001.\u0004 = new Hashtable();
                  break;
                }
                goto label_14;
              }
              else
                goto label_14;
            }
            else
              break;
          }
          else
            goto label_15;
        }
      }
      \u0002.\u0001.\u0006 = Convert.ToInt32(\u0002.\u0001.\u0002);
label_14:
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
label_15:
      Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("{ebca53df-06e7-4cae-bb4a-bca1650b7a2a}");
      if (true)
        goto label_16;
label_9:
      Stream stream;
      try
      {
        int int32 = Convert.ToInt32(stream.Length);
        \u0002.\u0001.\u0003 = new byte[int32];
        stream.Read(\u0002.\u0001.\u0003, 0, int32);
        stream.Close();
        return;
      }
      finally
      {
        stream?.Dispose();
      }
label_16:
      stream = manifestResourceStream;
      goto label_9;
    }
  }
}
