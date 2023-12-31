﻿// Decompiled with JetBrains decompiler
// Type: Xtra.Utility.ConvertFont
// Assembly: Xtra.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F0A554BF-0B1B-420F-A5E2-9214C58F15B4
// Assembly location: C:\Project\HH.CanOTo\references\CAN TRI VIET\Xtra.Utility.dll

using System.Runtime.InteropServices;

namespace Xtra.Utility
{
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class ConvertFont
  {
    private const int iCP1258 = 4;
    private const int iNCR = 0;
    private const int iNOSIGN = 7;
    private const int iTCV = 2;
    private const int iUNI = 6;
    private const int iUTF = 1;
    private const int iUTH = 5;
    private const int iVIQ = 8;
    private const int iVNI = 3;
    private const int nCode = 8;
    private string[,] Code;
    private FontCase m_CharCase = FontCase.Normal;

    public ConvertFont() => this.InitData();

    public bool Convert(ref string strConv, FontIndex iSource, FontIndex iDestination)
    {
      int index1 = (int) iSource;
      int index2 = (int) iDestination;
      if (strConv.Trim() == "" || index1 == index2)
        return false;
      string str1 = "";
      string str2 = "";
      if (index1 == -1)
      {
        int code = (int) this.getCode(strConv);
        if (code <= -1)
          return false;
        index1 = code;
      }
      if (index2 == -1)
        index2 = 0;
      int num1 = this.GetnChar((FontIndex) int.Parse(this.Code[0, index1]));
      int num2 = num1 > 1 ? num1 - 1 : 1;
      string str3 = "";
      bool flag = false;
      strConv += "       ";
      int startIndex = 0;
      while (startIndex < strConv.Length - 7)
      {
        for (int length = num1; length >= num2; --length)
        {
          str2 = "";
          if (strConv.Substring(startIndex, 1) == " ")
          {
            str1 = " ";
            break;
          }
          str1 = strConv.Substring(startIndex, length);
          for (int index3 = 1; index3 < 135; ++index3)
          {
            if (str1 == this.Code[index3, index1])
            {
              str2 = this.m_CharCase != FontCase.UpperCase || index3 >= 68 ? (this.m_CharCase != FontCase.LowerCase || index3 < 68 ? this.Code[index3, index2] : this.Code[index3 - 67, index2]) : this.Code[index3 + 67, index2];
              startIndex += length;
              break;
            }
          }
          if (str2 != "" || length == 5)
            break;
        }
        if (str2 != "")
        {
          str3 += str2;
          flag = true;
        }
        else
        {
          str3 = this.m_CharCase != FontCase.UpperCase ? (this.m_CharCase != FontCase.LowerCase ? str3 + str1.Substring(0, 1) : str3 + str1.Substring(0, 1).ToLower()) : str3 + str1.Substring(0, 1).ToUpper();
          ++startIndex;
        }
      }
      if (!flag)
      {
        strConv = strConv.Remove(strConv.Length - 7, 7);
        if (this.m_CharCase == FontCase.UpperCase)
        {
          strConv = strConv.ToUpper();
          return true;
        }
        if (this.m_CharCase != FontCase.LowerCase)
          return false;
        strConv = strConv.ToLower();
        return true;
      }
      strConv = str3.TrimEnd();
      return true;
    }

    private FontIndex getCode(string str)
    {
      if (str.Trim() == "")
        return FontIndex.iNotKnown;
      int index1 = -1;
      str += "       ";
      int startIndex = 0;
      while (startIndex < str.Length - 7)
      {
        if (str.Substring(startIndex, 1) == " ")
        {
          ++startIndex;
        }
        else
        {
          for (int length = 7; length > 0; --length)
          {
            string s = str.Substring(startIndex, length);
            for (int index2 = 0; index2 < 7 && length != 4 && length != 5 && (length < 6 || index2 == 0); ++index2)
            {
              if ((length != 3 || index2 == 1) && (index2 != 3 && index2 != 2 && index2 != 5 && index2 != 4 || length <= 2))
              {
                for (int index3 = 1; index3 < 135; ++index3)
                {
                  if (s == this.Code[index3, index2])
                  {
                    if (!this.isSpecialChar(s, index2 == 5 || index2 == 6))
                      return (FontIndex) int.Parse(this.Code[0, index2]);
                    index1 = index2;
                    break;
                  }
                }
              }
            }
          }
          ++startIndex;
        }
      }
      return index1 >= 0 ? (FontIndex) int.Parse(this.Code[0, index1]) : FontIndex.iNotKnown;
    }

    private int getIntCode(string code)
    {
      for (int intCode = 0; intCode < 8; ++intCode)
      {
        if (this.Code[0, intCode] == code)
          return intCode;
      }
      return -1;
    }

    private int GetnChar(FontIndex index)
    {
      int num;
      switch (index)
      {
        case FontIndex.iUTF:
          return 3;
        case FontIndex.iUTH:
        case FontIndex.iUNI:
          num = 0;
          break;
        default:
          num = index != FontIndex.iNOSIGN ? 1 : 0;
          break;
      }
      if (num == 0)
        return 1;
      return index == FontIndex.iNCR ? 7 : 2;
    }

    private void InitData()
    {
      this.Code = new string[135, 8];
      this.Code[0, 0] = 0.ToString();
      this.Code[0, 1] = 1.ToString();
      this.Code[0, 2] = 2.ToString();
      this.Code[0, 3] = 3.ToString();
      this.Code[0, 5] = 5.ToString();
      this.Code[0, 6] = 6.ToString();
      this.Code[0, 4] = 4.ToString();
      this.Code[0, 7] = 7.ToString();
      this.MapUnicode();
      this.MapVNI();
      this.MapTCV();
      this.MapUTH();
      this.MapUTF8();
      this.MapNCR();
      this.MapNoSign();
      this.MapCP1258();
    }

    private bool isSpecialChar(string s) => this.isSpecialChar(s, false);

    private bool isSpecialChar(string s, bool isUNI)
    {
      if (s.Length <= 2)
      {
        string[] strArray1 = new string[37]
        {
          "í",
          "ì",
          "ó",
          "ò",
          "ô",
          "ñ",
          "î",
          "Ê",
          "È",
          "É",
          "á",
          "à",
          "â",
          "è",
          "é",
          "ê",
          "ù",
          "ý",
          "ú",
          "ö",
          "Í",
          "Ì",
          "Ó",
          "Ò",
          "Ô",
          "Ñ",
          "Î",
          "Õ",
          "Ý",
          "Ã",
          "oà",
          "oá",
          "oã",
          "uû",
          "OÁ",
          "OÀ",
          "OÃ"
        };
        string[] strArray2 = new string[44]
        {
          "ă",
          "â",
          "ê",
          "ô",
          "ơ",
          "ư",
          "đ",
          "í",
          "ì",
          "ó",
          "ò",
          "ô",
          "ñ",
          "î",
          "Ê",
          "È",
          "É",
          "á",
          "à",
          "â",
          "è",
          "é",
          "ê",
          "ù",
          "ý",
          "ú",
          "ö",
          "Í",
          "Ì",
          "Ó",
          "Ò",
          "Ô",
          "Ñ",
          "Î",
          "Õ",
          "Ý",
          "Ã",
          "oà",
          "oá",
          "oã",
          "uû",
          "OÁ",
          "OÀ",
          "OÃ"
        };
        foreach (string strB in isUNI ? strArray2 : strArray1)
        {
          if (string.Compare(s, strB, true) == 0)
            return true;
        }
      }
      return false;
    }

    public bool isVietnamese(string s) => this.getCode(s) != FontIndex.iNotKnown;

    public bool isVietnamese(string s, ref FontIndex code)
    {
      code = this.getCode(s);
      return code > FontIndex.iNotKnown;
    }

    private void MapBKHCM1()
    {
      this.Code[1, 6] = "\u00BE";
      this.Code[2, 6] = "¿";
      this.Code[3, 6] = "À";
      this.Code[4, 6] = "Á";
      this.Code[5, 6] = "Â";
      this.Code[6, 6] = "×";
      this.Code[7, 6] = "Ø";
      this.Code[8, 6] = "Ù";
      this.Code[9, 6] = "Ú";
      this.Code[10, 6] = "Û";
      this.Code[11, 6] = "Ü";
      this.Code[12, 6] = "Ý";
      this.Code[13, 6] = "Þ";
      this.Code[14, 6] = "ß";
      this.Code[15, 6] = "à";
      this.Code[16, 6] = "á";
      this.Code[17, 6] = "â";
      this.Code[18, 6] = "Ã";
      this.Code[19, 6] = "Ä";
      this.Code[20, 6] = "Å";
      this.Code[21, 6] = "Æ";
      this.Code[22, 6] = "Ç";
      this.Code[23, 6] = "ã";
      this.Code[24, 6] = "ä";
      this.Code[25, 6] = "å";
      this.Code[26, 6] = "æ";
      this.Code[27, 6] = "ç";
      this.Code[28, 6] = "è";
      this.Code[29, 6] = "È";
      this.Code[30, 6] = "É";
      this.Code[31, 6] = "Ê";
      this.Code[32, 6] = "Ë";
      this.Code[33, 6] = "Ì";
      this.Code[34, 6] = "Í";
      this.Code[35, 6] = "Î";
      this.Code[36, 6] = "Ï";
      this.Code[37, 6] = "Ð";
      this.Code[38, 6] = "Ñ";
      this.Code[39, 6] = "é";
      this.Code[40, 6] = "ê";
      this.Code[41, 6] = "ë";
      this.Code[42, 6] = "ì";
      this.Code[43, 6] = "í";
      this.Code[44, 6] = "î";
      this.Code[45, 6] = "ï";
      this.Code[46, 6] = "ð";
      this.Code[47, 6] = "ñ";
      this.Code[48, 6] = "ò";
      this.Code[49, 6] = "ó";
      this.Code[50, 6] = "ô";
      this.Code[51, 6] = "Ò";
      this.Code[52, 6] = "Ó";
      this.Code[53, 6] = "Ô";
      this.Code[54, 6] = "Õ";
      this.Code[55, 6] = "Ö";
      this.Code[56, 6] = "õ";
      this.Code[57, 6] = "ö";
      this.Code[58, 6] = "÷";
      this.Code[59, 6] = "ø";
      this.Code[60, 6] = "ù";
      this.Code[61, 6] = "ú";
      this.Code[62, 6] = "û";
      this.Code[63, 6] = "ü";
      this.Code[64, 6] = "ý";
      this.Code[65, 6] = "þ";
      this.Code[66, 6] = "ÿ";
      this.Code[67, 6] = "\u00BD";
      this.Code[68, 6] = "€";
      this.Code[69, 6] = "\u0081";
      this.Code[70, 6] = "‚";
      this.Code[71, 6] = "ƒ";
      this.Code[72, 6] = "„";
      this.Code[73, 6] = "™";
      this.Code[74, 6] = "š";
      this.Code[75, 6] = "›";
      this.Code[76, 6] = "œ";
      this.Code[77, 6] = "\u009D";
      this.Code[78, 6] = "˜";
      this.Code[79, 6] = "Ÿ";
      this.Code[80, 6] = "~";
      this.Code[81, 6] = "¡";
      this.Code[82, 6] = "¢";
      this.Code[83, 6] = "£";
      this.Code[84, 6] = "¤";
      this.Code[85, 6] = "…";
      this.Code[86, 6] = "†";
      this.Code[87, 6] = "‡";
      this.Code[88, 6] = "ˆ";
      this.Code[89, 6] = "‰";
      this.Code[90, 6] = "¥";
      this.Code[91, 6] = "¦";
      this.Code[92, 6] = "§";
      this.Code[93, 6] = "¨";
      this.Code[94, 6] = "©";
      this.Code[95, 6] = "ª";
      this.Code[96, 6] = "Š";
      this.Code[97, 6] = "‹";
      this.Code[98, 6] = "Œ";
      this.Code[99, 6] = "\u008D";
      this.Code[100, 6] = "Ž";
      this.Code[101, 6] = "\u008F";
      this.Code[102, 6] = "\u0090";
      this.Code[103, 6] = "‘";
      this.Code[104, 6] = "’";
      this.Code[105, 6] = "“";
      this.Code[106, 6] = "«";
      this.Code[107, 6] = "¬";
      this.Code[108, 6] = "\u00AD";
      this.Code[109, 6] = "®";
      this.Code[110, 6] = "¯";
      this.Code[111, 6] = "°";
      this.Code[112, 6] = "±";
      this.Code[113, 6] = "\u00B2";
      this.Code[114, 6] = "\u00B3";
      this.Code[115, 6] = "´";
      this.Code[116, 6] = "µ";
      this.Code[117, 6] = "¶";
      this.Code[118, 6] = "”";
      this.Code[119, 6] = "•";
      this.Code[120, 6] = "–";
      this.Code[121, 6] = "—";
      this.Code[122, 6] = "˜";
      this.Code[123, 6] = "·";
      this.Code[124, 6] = "¸";
      this.Code[125, 6] = "\u00B9";
      this.Code[126, 6] = "º";
      this.Code[(int) sbyte.MaxValue, 6] = "»";
      this.Code[128, 6] = "\u00BC";
      this.Code[129, 6] = "{";
      this.Code[130, 6] = "^";
      this.Code[131, 6] = "`";
      this.Code[132, 6] = "|";
      this.Code[133, 6] = "Ž";
      this.Code[134, 6] = "}";
    }

    private void MapBKHCM2()
    {
      this.Code[1, 6] = "aá";
      this.Code[2, 6] = "aâ";
      this.Code[3, 6] = "aã";
      this.Code[4, 6] = "aä";
      this.Code[5, 6] = "aå";
      this.Code[6, 6] = "ù";
      this.Code[7, 6] = "ùæ";
      this.Code[8, 6] = "ùç";
      this.Code[9, 6] = "ùè";
      this.Code[10, 6] = "ùé";
      this.Code[11, 6] = "ùå";
      this.Code[12, 6] = "ê";
      this.Code[13, 6] = "êë";
      this.Code[14, 6] = "êì";
      this.Code[15, 6] = "êí";
      this.Code[16, 6] = "êî";
      this.Code[17, 6] = "êå";
      this.Code[18, 6] = "eá";
      this.Code[19, 6] = "eâ";
      this.Code[20, 6] = "eã";
      this.Code[21, 6] = "eä";
      this.Code[22, 6] = "eå";
      this.Code[23, 6] = "ï";
      this.Code[24, 6] = "ïë";
      this.Code[25, 6] = "ïì";
      this.Code[26, 6] = "ïí";
      this.Code[27, 6] = "ïî";
      this.Code[28, 6] = "ïå";
      this.Code[29, 6] = "ñ";
      this.Code[30, 6] = "ò";
      this.Code[31, 6] = "ó";
      this.Code[32, 6] = "ô";
      this.Code[33, 6] = "õ";
      this.Code[34, 6] = "oá";
      this.Code[35, 6] = "oâ";
      this.Code[36, 6] = "oã";
      this.Code[37, 6] = "oä";
      this.Code[38, 6] = "oå";
      this.Code[39, 6] = "ö";
      this.Code[40, 6] = "öë";
      this.Code[41, 6] = "öì";
      this.Code[42, 6] = "öí";
      this.Code[43, 6] = "öî";
      this.Code[44, 6] = "öå";
      this.Code[45, 6] = "ú";
      this.Code[46, 6] = "úá";
      this.Code[47, 6] = "úâ";
      this.Code[48, 6] = "úã";
      this.Code[49, 6] = "úä";
      this.Code[50, 6] = "úå";
      this.Code[51, 6] = "uá";
      this.Code[52, 6] = "uâ";
      this.Code[53, 6] = "uã";
      this.Code[54, 6] = "uä";
      this.Code[55, 6] = "uå";
      this.Code[56, 6] = "û";
      this.Code[57, 6] = "ûá";
      this.Code[58, 6] = "ûâ";
      this.Code[59, 6] = "ûã";
      this.Code[60, 6] = "ûä";
      this.Code[61, 6] = "ûå";
      this.Code[62, 6] = "yá";
      this.Code[63, 6] = "yâ";
      this.Code[64, 6] = "yã";
      this.Code[65, 6] = "yä";
      this.Code[66, 6] = "yå";
      this.Code[67, 6] = "à";
      this.Code[68, 6] = "AÁ";
      this.Code[69, 6] = "AÂ";
      this.Code[70, 6] = "AÃ";
      this.Code[71, 6] = "AÄ";
      this.Code[72, 6] = "AÅ";
      this.Code[73, 6] = "Ù";
      this.Code[74, 6] = "ÙÆ";
      this.Code[75, 6] = "ÙÇ";
      this.Code[76, 6] = "ÙÈ";
      this.Code[77, 6] = "ÙÉ";
      this.Code[78, 6] = "ÙÅ";
      this.Code[79, 6] = "Ê";
      this.Code[80, 6] = "ÊË";
      this.Code[81, 6] = "ÊÌ";
      this.Code[82, 6] = "ÊÍ";
      this.Code[83, 6] = "ÊÎ";
      this.Code[84, 6] = "ÊÅ";
      this.Code[85, 6] = "EÁ";
      this.Code[86, 6] = "EÂ";
      this.Code[87, 6] = "EÃ";
      this.Code[88, 6] = "EÄ";
      this.Code[89, 6] = "EÅ";
      this.Code[90, 6] = "Ï";
      this.Code[91, 6] = "ÏË";
      this.Code[92, 6] = "ÏÌ";
      this.Code[93, 6] = "ÏÍ";
      this.Code[94, 6] = "ÏÎ";
      this.Code[95, 6] = "Ïå";
      this.Code[96, 6] = "Ñ";
      this.Code[97, 6] = "Ò";
      this.Code[98, 6] = "Ó";
      this.Code[99, 6] = "Ô";
      this.Code[100, 6] = "Õ";
      this.Code[101, 6] = "OÁ";
      this.Code[102, 6] = "OÂ";
      this.Code[103, 6] = "OÃ";
      this.Code[104, 6] = "OÄ";
      this.Code[105, 6] = "OÅ";
      this.Code[106, 6] = "Ö";
      this.Code[107, 6] = "ÖË";
      this.Code[108, 6] = "ÖÌ";
      this.Code[109, 6] = "ÖÍ";
      this.Code[110, 6] = "ÖÎ";
      this.Code[111, 6] = "ÖÅ";
      this.Code[112, 6] = "Ú";
      this.Code[113, 6] = "ÚÁ";
      this.Code[114, 6] = "ÚÂ";
      this.Code[115, 6] = "ÚÃ";
      this.Code[116, 6] = "ÚÄ";
      this.Code[117, 6] = "ÚÅ";
      this.Code[118, 6] = "UÁ";
      this.Code[119, 6] = "UÂ";
      this.Code[120, 6] = "UÃ";
      this.Code[121, 6] = "UÄ";
      this.Code[122, 6] = "UÅ";
      this.Code[123, 6] = "Û";
      this.Code[124, 6] = "ÛÁ";
      this.Code[125, 6] = "ÛÂ";
      this.Code[126, 6] = "ÛÃ";
      this.Code[(int) sbyte.MaxValue, 6] = "ÛÄ";
      this.Code[128, 6] = "ÛÅ";
      this.Code[129, 6] = "YÁ";
      this.Code[130, 6] = "YÂ";
      this.Code[131, 6] = "YÃ";
      this.Code[132, 6] = "YÄ";
      this.Code[133, 6] = "YÅ";
      this.Code[134, 6] = "À";
    }

    private void MapCP1258()
    {
      this.Code[1, 4] = "aì";
      this.Code[2, 4] = "aÌ";
      this.Code[3, 4] = "aÒ";
      this.Code[4, 4] = "aÞ";
      this.Code[5, 4] = "aò";
      this.Code[6, 4] = "ã";
      this.Code[7, 4] = "ãì";
      this.Code[8, 4] = "ãÌ";
      this.Code[9, 4] = "ãÒ";
      this.Code[10, 4] = "ãÞ";
      this.Code[11, 4] = "ãò";
      this.Code[12, 4] = "â";
      this.Code[13, 4] = "âì";
      this.Code[14, 4] = "âÌ";
      this.Code[15, 4] = "âÒ";
      this.Code[16, 4] = "âÞ";
      this.Code[17, 4] = "âò";
      this.Code[18, 4] = "eì";
      this.Code[19, 4] = "eÌ";
      this.Code[20, 4] = "eÒ";
      this.Code[21, 4] = "eÞ";
      this.Code[22, 4] = "eò";
      this.Code[23, 4] = "ê";
      this.Code[24, 4] = "êì";
      this.Code[25, 4] = "êÌ";
      this.Code[26, 4] = "êÒ";
      this.Code[27, 4] = "êÞ";
      this.Code[28, 4] = "êò";
      this.Code[29, 4] = "iì";
      this.Code[30, 4] = "iÌ";
      this.Code[31, 4] = "iÒ";
      this.Code[32, 4] = "iÞ";
      this.Code[33, 4] = "iò";
      this.Code[34, 4] = "oì";
      this.Code[35, 4] = "oÌ";
      this.Code[36, 4] = "oÒ";
      this.Code[37, 4] = "oÞ";
      this.Code[38, 4] = "oò";
      this.Code[39, 4] = "ô";
      this.Code[40, 4] = "ôì";
      this.Code[41, 4] = "ôÌ";
      this.Code[42, 4] = "ôÒ";
      this.Code[43, 4] = "ôÞ";
      this.Code[44, 4] = "ôò";
      this.Code[45, 4] = "õ";
      this.Code[46, 4] = "õì";
      this.Code[47, 4] = "õÌ";
      this.Code[48, 4] = "õÒ";
      this.Code[49, 4] = "õÞ";
      this.Code[50, 4] = "õò";
      this.Code[51, 4] = "uì";
      this.Code[52, 4] = "uÌ";
      this.Code[53, 4] = "uÒ";
      this.Code[54, 4] = "uÞ";
      this.Code[55, 4] = "uò";
      this.Code[56, 4] = "ý";
      this.Code[57, 4] = "ýì";
      this.Code[58, 4] = "ýÌ";
      this.Code[59, 4] = "ýÒ";
      this.Code[60, 4] = "ýÞ";
      this.Code[61, 4] = "ýò";
      this.Code[62, 4] = "yì";
      this.Code[63, 4] = "yÌ";
      this.Code[64, 4] = "yÒ";
      this.Code[65, 4] = "yÞ";
      this.Code[66, 4] = "yò";
      this.Code[67, 4] = "ð";
      this.Code[68, 4] = "Aì";
      this.Code[69, 4] = "AÌ";
      this.Code[70, 4] = "AÒ";
      this.Code[71, 4] = "AÞ";
      this.Code[72, 4] = "Aò";
      this.Code[73, 4] = "Ã";
      this.Code[74, 4] = "Ãì";
      this.Code[75, 4] = "ÃÌ";
      this.Code[76, 4] = "ÃÒ";
      this.Code[77, 4] = "ÃÞ";
      this.Code[78, 4] = "Ãò";
      this.Code[79, 4] = "Â";
      this.Code[80, 4] = "Âì";
      this.Code[81, 4] = "ÂÌ";
      this.Code[82, 4] = "ÂÒ";
      this.Code[83, 4] = "ÂÞ";
      this.Code[84, 4] = "Âò";
      this.Code[85, 4] = "Eì";
      this.Code[86, 4] = "EÌ";
      this.Code[87, 4] = "EÒ";
      this.Code[88, 4] = "EÞ";
      this.Code[89, 4] = "Eò";
      this.Code[90, 4] = "Ê";
      this.Code[91, 4] = "Êì";
      this.Code[92, 4] = "ÊÌ";
      this.Code[93, 4] = "ÊÒ";
      this.Code[94, 4] = "ÊÞ";
      this.Code[95, 4] = "Êò";
      this.Code[96, 4] = "Iì";
      this.Code[97, 4] = "IÌ";
      this.Code[98, 4] = "IÒ";
      this.Code[99, 4] = "IÞ";
      this.Code[100, 4] = "Iò";
      this.Code[101, 4] = "Oì";
      this.Code[102, 4] = "OÌ";
      this.Code[103, 4] = "OÒ";
      this.Code[104, 4] = "OÞ";
      this.Code[105, 4] = "Oò";
      this.Code[106, 4] = "Ô";
      this.Code[107, 4] = "Ôì";
      this.Code[108, 4] = "ÔÌ";
      this.Code[109, 4] = "ÔÒ";
      this.Code[110, 4] = "ÔÞ";
      this.Code[111, 4] = "Ôò";
      this.Code[112, 4] = "Õ";
      this.Code[113, 4] = "Õì";
      this.Code[114, 4] = "ÕÌ";
      this.Code[115, 4] = "ÕÒ";
      this.Code[116, 4] = "ÕÞ";
      this.Code[117, 4] = "Õò";
      this.Code[118, 4] = "Uì";
      this.Code[119, 4] = "UÌ";
      this.Code[120, 4] = "UÒ";
      this.Code[121, 4] = "UÞ";
      this.Code[122, 4] = "Uò";
      this.Code[123, 4] = "Ý";
      this.Code[124, 4] = "Ýì";
      this.Code[125, 4] = "ÝÌ";
      this.Code[126, 4] = "ÝÒ";
      this.Code[(int) sbyte.MaxValue, 4] = "ÝÞ";
      this.Code[128, 4] = "Ýò";
      this.Code[129, 4] = "Yì";
      this.Code[130, 4] = "YÌ";
      this.Code[131, 4] = "YÒ";
      this.Code[132, 4] = "YÞ";
      this.Code[133, 4] = "Yò";
      this.Code[134, 4] = "Ð";
    }

    private void MapCString()
    {
      this.Code[1, 6] = "á";
      this.Code[2, 6] = "à";
      this.Code[3, 6] = "ả";
      this.Code[4, 6] = "ã";
      this.Code[5, 6] = "ạ";
      this.Code[6, 6] = "ă";
      this.Code[7, 6] = "ắ";
      this.Code[8, 6] = "ằ";
      this.Code[9, 6] = "ẳ";
      this.Code[10, 6] = "ẵ";
      this.Code[11, 6] = "ặ";
      this.Code[12, 6] = "â";
      this.Code[13, 6] = "ấ";
      this.Code[14, 6] = "ầ";
      this.Code[15, 6] = "ẩ";
      this.Code[16, 6] = "ẫ";
      this.Code[17, 6] = "ậ";
      this.Code[18, 6] = "é";
      this.Code[19, 6] = "è";
      this.Code[20, 6] = "ẻ";
      this.Code[21, 6] = "ẽ";
      this.Code[22, 6] = "ẹ";
      this.Code[23, 6] = "ê";
      this.Code[24, 6] = "ế";
      this.Code[25, 6] = "ề";
      this.Code[26, 6] = "ể";
      this.Code[27, 6] = "ễ";
      this.Code[28, 6] = "ệ";
      this.Code[29, 6] = "í";
      this.Code[30, 6] = "ì";
      this.Code[31, 6] = "ỉ";
      this.Code[32, 6] = "ĩ";
      this.Code[33, 6] = "ị";
      this.Code[34, 6] = "ó";
      this.Code[35, 6] = "ò";
      this.Code[36, 6] = "ỏ";
      this.Code[37, 6] = "õ";
      this.Code[38, 6] = "ọ";
      this.Code[39, 6] = "ô";
      this.Code[40, 6] = "ố";
      this.Code[41, 6] = "ồ";
      this.Code[42, 6] = "ổ";
      this.Code[43, 6] = "ỗ";
      this.Code[44, 6] = "ộ";
      this.Code[45, 6] = "ơ";
      this.Code[46, 6] = "ớ";
      this.Code[47, 6] = "ờ";
      this.Code[48, 6] = "ở";
      this.Code[49, 6] = "ỡ";
      this.Code[50, 6] = "ợ";
      this.Code[51, 6] = "ú";
      this.Code[52, 6] = "ù";
      this.Code[53, 6] = "ủ";
      this.Code[54, 6] = "ũ";
      this.Code[55, 6] = "ụ";
      this.Code[56, 6] = "ư";
      this.Code[57, 6] = "ứ";
      this.Code[58, 6] = "ừ";
      this.Code[59, 6] = "ử";
      this.Code[60, 6] = "ữ";
      this.Code[61, 6] = "ự";
      this.Code[62, 6] = "ý";
      this.Code[63, 6] = "ỳ";
      this.Code[64, 6] = "ỷ";
      this.Code[65, 6] = "ỹ";
      this.Code[66, 6] = "ỵ";
      this.Code[67, 6] = "đ";
      this.Code[68, 6] = "Á";
      this.Code[69, 6] = "À";
      this.Code[70, 6] = "Ả";
      this.Code[71, 6] = "Ã";
      this.Code[72, 6] = "Ạ";
      this.Code[73, 6] = "Ă";
      this.Code[74, 6] = "Ắ";
      this.Code[75, 6] = "Ằ";
      this.Code[76, 6] = "Ẳ";
      this.Code[77, 6] = "Ẵ";
      this.Code[78, 6] = "Ặ";
      this.Code[79, 6] = "Â";
      this.Code[80, 6] = "Ấ";
      this.Code[81, 6] = "Ầ";
      this.Code[82, 6] = "Ẩ";
      this.Code[83, 6] = "Ẫ";
      this.Code[84, 6] = "Ậ";
      this.Code[85, 6] = "É";
      this.Code[86, 6] = "È";
      this.Code[87, 6] = "Ẻ";
      this.Code[88, 6] = "Ẽ";
      this.Code[89, 6] = "Ẹ";
      this.Code[90, 6] = "Ê";
      this.Code[91, 6] = "Ế";
      this.Code[92, 6] = "Ề";
      this.Code[93, 6] = "Ể";
      this.Code[94, 6] = "Ễ";
      this.Code[95, 6] = "Ệ";
      this.Code[96, 6] = "Í";
      this.Code[97, 6] = "Ì";
      this.Code[98, 6] = "Ỉ";
      this.Code[99, 6] = "Ĩ";
      this.Code[100, 6] = "Ị";
      this.Code[101, 6] = "Ó";
      this.Code[102, 6] = "Ò";
      this.Code[103, 6] = "Ỏ";
      this.Code[104, 6] = "Õ";
      this.Code[105, 6] = "Ọ";
      this.Code[106, 6] = "Ô";
      this.Code[107, 6] = "Ố";
      this.Code[108, 6] = "Ồ";
      this.Code[109, 6] = "Ổ";
      this.Code[110, 6] = "Ỗ";
      this.Code[111, 6] = "Ộ";
      this.Code[112, 6] = "Ơ";
      this.Code[113, 6] = "Ớ";
      this.Code[114, 6] = "Ờ";
      this.Code[115, 6] = "Ở";
      this.Code[116, 6] = "Ỡ";
      this.Code[117, 6] = "Ợ";
      this.Code[118, 6] = "Ú";
      this.Code[119, 6] = "Ù";
      this.Code[120, 6] = "Ủ";
      this.Code[121, 6] = "Ũ";
      this.Code[122, 6] = "Ụ";
      this.Code[123, 6] = "Ư";
      this.Code[124, 6] = "Ứ";
      this.Code[125, 6] = "Ừ";
      this.Code[126, 6] = "Ử";
      this.Code[(int) sbyte.MaxValue, 6] = "Ữ";
      this.Code[128, 6] = "Ự";
      this.Code[129, 6] = "Ý";
      this.Code[130, 6] = "Ỳ";
      this.Code[131, 6] = "Ỷ";
      this.Code[132, 6] = "Ỹ";
      this.Code[133, 6] = "Ỵ";
      this.Code[134, 6] = "Đ";
    }

    private void MapNCR()
    {
      this.Code[1, 0] = "&#225;";
      this.Code[2, 0] = "&#224;";
      this.Code[3, 0] = "&#7843;";
      this.Code[4, 0] = "&#227;";
      this.Code[5, 0] = "&#7841;";
      this.Code[6, 0] = "&#259;";
      this.Code[7, 0] = "&#7855;";
      this.Code[8, 0] = "&#7857;";
      this.Code[9, 0] = "&#7859;";
      this.Code[10, 0] = "&#7861;";
      this.Code[11, 0] = "&#7863;";
      this.Code[12, 0] = "&#226;";
      this.Code[13, 0] = "&#7845;";
      this.Code[14, 0] = "&#7847;";
      this.Code[15, 0] = "&#7849;";
      this.Code[16, 0] = "&#7851;";
      this.Code[17, 0] = "&#7853;";
      this.Code[18, 0] = "&#233;";
      this.Code[19, 0] = "&#232;";
      this.Code[20, 0] = "&#7867;";
      this.Code[21, 0] = "&#7869;";
      this.Code[22, 0] = "&#7865;";
      this.Code[23, 0] = "&#234;";
      this.Code[24, 0] = "&#7871;";
      this.Code[25, 0] = "&#7873;";
      this.Code[26, 0] = "&#7875;";
      this.Code[27, 0] = "&#7877;";
      this.Code[28, 0] = "&#7879;";
      this.Code[29, 0] = "&#237;";
      this.Code[30, 0] = "&#236;";
      this.Code[31, 0] = "&#7881;";
      this.Code[32, 0] = "&#297;";
      this.Code[33, 0] = "&#7883;";
      this.Code[34, 0] = "&#243;";
      this.Code[35, 0] = "&#242;";
      this.Code[36, 0] = "&#7887;";
      this.Code[37, 0] = "&#245;";
      this.Code[38, 0] = "&#7885;";
      this.Code[39, 0] = "&#244;";
      this.Code[40, 0] = "&#7889;";
      this.Code[41, 0] = "&#7891;";
      this.Code[42, 0] = "&#7893;";
      this.Code[43, 0] = "&#7895;";
      this.Code[44, 0] = "&#7897;";
      this.Code[45, 0] = "&#417;";
      this.Code[46, 0] = "&#7899;";
      this.Code[47, 0] = "&#7901;";
      this.Code[48, 0] = "&#7903;";
      this.Code[49, 0] = "&#7905;";
      this.Code[50, 0] = "&#7907;";
      this.Code[51, 0] = "&#250;";
      this.Code[52, 0] = "&#249;";
      this.Code[53, 0] = "&#7911;";
      this.Code[54, 0] = "&#361;";
      this.Code[55, 0] = "&#7909;";
      this.Code[56, 0] = "&#432;";
      this.Code[57, 0] = "&#7913;";
      this.Code[58, 0] = "&#7915;";
      this.Code[59, 0] = "&#7917;";
      this.Code[60, 0] = "&#7919;";
      this.Code[61, 0] = "&#7921;";
      this.Code[62, 0] = "&#253;";
      this.Code[63, 0] = "&#7923;";
      this.Code[64, 0] = "&#7927;";
      this.Code[65, 0] = "&#7929;";
      this.Code[66, 0] = "&#7925;";
      this.Code[67, 0] = "&#273;";
      this.Code[68, 0] = "&#193;";
      this.Code[69, 0] = "&#192;";
      this.Code[70, 0] = "&#7842;";
      this.Code[71, 0] = "&#195;";
      this.Code[72, 0] = "&#7840;";
      this.Code[73, 0] = "&#258;";
      this.Code[74, 0] = "&#7854;";
      this.Code[75, 0] = "&#7856;";
      this.Code[76, 0] = "&#7858;";
      this.Code[77, 0] = "&#7860;";
      this.Code[78, 0] = "&#7862;";
      this.Code[79, 0] = "&#194;";
      this.Code[80, 0] = "&#7844;";
      this.Code[81, 0] = "&#7846;";
      this.Code[82, 0] = "&#7848;";
      this.Code[83, 0] = "&#7850;";
      this.Code[84, 0] = "&#7852;";
      this.Code[85, 0] = "&#201;";
      this.Code[86, 0] = "&#200;";
      this.Code[87, 0] = "&#7866;";
      this.Code[88, 0] = "&#7868;";
      this.Code[89, 0] = "&#7864;";
      this.Code[90, 0] = "&#202;";
      this.Code[91, 0] = "&#7870;";
      this.Code[92, 0] = "&#7872;";
      this.Code[93, 0] = "&#7874;";
      this.Code[94, 0] = "&#7876;";
      this.Code[95, 0] = "&#7878;";
      this.Code[96, 0] = "&#205;";
      this.Code[97, 0] = "&#204;";
      this.Code[98, 0] = "&#7880;";
      this.Code[99, 0] = "&#296;";
      this.Code[100, 0] = "&#7882;";
      this.Code[101, 0] = "&#211;";
      this.Code[102, 0] = "&#210;";
      this.Code[103, 0] = "&#7886;";
      this.Code[104, 0] = "&#213;";
      this.Code[105, 0] = "&#7884;";
      this.Code[106, 0] = "&#212;";
      this.Code[107, 0] = "&#7888;";
      this.Code[108, 0] = "&#7890;";
      this.Code[109, 0] = "&#7892;";
      this.Code[110, 0] = "&#7894;";
      this.Code[111, 0] = "&#7896;";
      this.Code[112, 0] = "&#416;";
      this.Code[113, 0] = "&#7898;";
      this.Code[114, 0] = "&#7900;";
      this.Code[115, 0] = "&#7902;";
      this.Code[116, 0] = "&#7904;";
      this.Code[117, 0] = "&#7906;";
      this.Code[118, 0] = "&#218;";
      this.Code[119, 0] = "&#217;";
      this.Code[120, 0] = "&#7910;";
      this.Code[121, 0] = "&#360;";
      this.Code[122, 0] = "&#7908;";
      this.Code[123, 0] = "&#431;";
      this.Code[124, 0] = "&#7912;";
      this.Code[125, 0] = "&#7914;";
      this.Code[126, 0] = "&#7916;";
      this.Code[(int) sbyte.MaxValue, 0] = "&#7918;";
      this.Code[128, 0] = "&#7920;";
      this.Code[129, 0] = "&#221;";
      this.Code[130, 0] = "&#7922;";
      this.Code[131, 0] = "&#7926;";
      this.Code[132, 0] = "&#7928;";
      this.Code[133, 0] = "&#7924;";
      this.Code[134, 0] = "&#272;";
    }

    private void MapNCRHex()
    {
      this.Code[1, 6] = "á";
      this.Code[2, 6] = "à";
      this.Code[3, 6] = "&#x1EA3;";
      this.Code[4, 6] = "ã";
      this.Code[5, 6] = "&#x1EA1;";
      this.Code[6, 6] = "&#x103;";
      this.Code[7, 6] = "&#x1EAF;";
      this.Code[8, 6] = "&#x1EB1;";
      this.Code[9, 6] = "&#x1EB3;";
      this.Code[10, 6] = "&#x1EB5;";
      this.Code[11, 6] = "&#x1EB7;";
      this.Code[12, 6] = "â";
      this.Code[13, 6] = "&#x1EA5;";
      this.Code[14, 6] = "&#x1EA7;";
      this.Code[15, 6] = "&#x1EA9;";
      this.Code[16, 6] = "&#x1EAB;";
      this.Code[17, 6] = "&#x1EAD;";
      this.Code[18, 6] = "é";
      this.Code[19, 6] = "è";
      this.Code[20, 6] = "&#x1EBB;";
      this.Code[21, 6] = "&#x1EBD;";
      this.Code[22, 6] = "&#x1EB9;";
      this.Code[23, 6] = "ê";
      this.Code[24, 6] = "&#x1EBF;";
      this.Code[25, 6] = "&#x1EC1;";
      this.Code[26, 6] = "&#x1EC3;";
      this.Code[27, 6] = "&#x1EC5;";
      this.Code[28, 6] = "&#x1EC7;";
      this.Code[29, 6] = "í";
      this.Code[30, 6] = "ì";
      this.Code[31, 6] = "&#x1EC9;";
      this.Code[32, 6] = "&#x129;";
      this.Code[33, 6] = "&#x1ECB;";
      this.Code[34, 6] = "ó";
      this.Code[35, 6] = "ò";
      this.Code[36, 6] = "&#x1ECF;";
      this.Code[37, 6] = "õ";
      this.Code[38, 6] = "&#x1ECD;";
      this.Code[39, 6] = "ô";
      this.Code[40, 6] = "&#x1ED1;";
      this.Code[41, 6] = "&#x1ED3;";
      this.Code[42, 6] = "&#x1ED5;";
      this.Code[43, 6] = "&#x1ED7;";
      this.Code[44, 6] = "&#x1ED9;";
      this.Code[45, 6] = "&#x1A1;";
      this.Code[46, 6] = "&#x1EDB;";
      this.Code[47, 6] = "&#x1EDD;";
      this.Code[48, 6] = "&#x1EDF;";
      this.Code[49, 6] = "&#x1EE1;";
      this.Code[50, 6] = "&#x1EE3;";
      this.Code[51, 6] = "ú";
      this.Code[52, 6] = "ù";
      this.Code[53, 6] = "&#x1EE7;";
      this.Code[54, 6] = "&#x169;";
      this.Code[55, 6] = "&#x1EE5;";
      this.Code[56, 6] = "&#x1B0;";
      this.Code[57, 6] = "&#x1EE9;";
      this.Code[58, 6] = "&#x1EEB;";
      this.Code[59, 6] = "&#x1EED;";
      this.Code[60, 6] = "&#x1EEF;";
      this.Code[61, 6] = "&#x1EF1;";
      this.Code[62, 6] = "ý";
      this.Code[63, 6] = "&#x1EF3;";
      this.Code[64, 6] = "&#x1EF7;";
      this.Code[65, 6] = "&#x1EF9;";
      this.Code[66, 6] = "&#x1EF5;";
      this.Code[67, 6] = "&#x111;";
      this.Code[68, 6] = "Á";
      this.Code[69, 6] = "À";
      this.Code[70, 6] = "&#x1EA2;";
      this.Code[71, 6] = "Ã";
      this.Code[72, 6] = "&#x1EA0;";
      this.Code[73, 6] = "&#x102;";
      this.Code[74, 6] = "&#x1EAE;";
      this.Code[75, 6] = "&#x1EB0;";
      this.Code[76, 6] = "&#x1EB2;";
      this.Code[77, 6] = "&#x1EB4;";
      this.Code[78, 6] = "&#x1EB6;";
      this.Code[79, 6] = "Â";
      this.Code[80, 6] = "&#x1EA4;";
      this.Code[81, 6] = "&#x1EA6;";
      this.Code[82, 6] = "&#x1EA8;";
      this.Code[83, 6] = "&#x1EAA;";
      this.Code[84, 6] = "&#x1EAC;";
      this.Code[85, 6] = "É";
      this.Code[86, 6] = "È";
      this.Code[87, 6] = "&#x1EBA;";
      this.Code[88, 6] = "&#x1EBC;";
      this.Code[89, 6] = "&#x1EB8;";
      this.Code[90, 6] = "Ê";
      this.Code[91, 6] = "&#x1EBE;";
      this.Code[92, 6] = "&#x1EC0;";
      this.Code[93, 6] = "&#x1EC2;";
      this.Code[94, 6] = "&#x1EC4;";
      this.Code[95, 6] = "&#x1EC6;";
      this.Code[96, 6] = "Í";
      this.Code[97, 6] = "Ì";
      this.Code[98, 6] = "&#x1EC8;";
      this.Code[99, 6] = "&#x128;";
      this.Code[100, 6] = "&#x1ECA;";
      this.Code[101, 6] = "Ó";
      this.Code[102, 6] = "Ò";
      this.Code[103, 6] = "&#x1ECE;";
      this.Code[104, 6] = "Õ";
      this.Code[105, 6] = "&#x1ECC;";
      this.Code[106, 6] = "Ô";
      this.Code[107, 6] = "&#x1ED0;";
      this.Code[108, 6] = "&#x1ED2;";
      this.Code[109, 6] = "&#x1ED4;";
      this.Code[110, 6] = "&#x1ED6;";
      this.Code[111, 6] = "&#x1ED8;";
      this.Code[112, 6] = "&#x1A0;";
      this.Code[113, 6] = "&#x1EDA;";
      this.Code[114, 6] = "&#x1EDC;";
      this.Code[115, 6] = "&#x1EDE;";
      this.Code[116, 6] = "&#x1EE0;";
      this.Code[117, 6] = "&#x1EE2;";
      this.Code[118, 6] = "Ú";
      this.Code[119, 6] = "Ù";
      this.Code[120, 6] = "&#x1EE6;";
      this.Code[121, 6] = "&#x168;";
      this.Code[122, 6] = "&#x1EE4;";
      this.Code[123, 6] = "&#x1AF;";
      this.Code[124, 6] = "&#x1EE8;";
      this.Code[125, 6] = "&#x1EEA;";
      this.Code[126, 6] = "&#x1EEC;";
      this.Code[(int) sbyte.MaxValue, 6] = "&#x1EEE;";
      this.Code[128, 6] = "&#x1EF0;";
      this.Code[129, 6] = "Ý";
      this.Code[130, 6] = "&#x1EF2;";
      this.Code[131, 6] = "&#x1EF6;";
      this.Code[132, 6] = "&#x1EF8;";
      this.Code[133, 6] = "&#x1EF4;";
      this.Code[134, 6] = "&#x110;";
    }

    private void MapNoSign()
    {
      this.Code[1, 7] = "a";
      this.Code[2, 7] = "a";
      this.Code[3, 7] = "a";
      this.Code[4, 7] = "a";
      this.Code[5, 7] = "a";
      this.Code[6, 7] = "a";
      this.Code[7, 7] = "a";
      this.Code[8, 7] = "a";
      this.Code[9, 7] = "a";
      this.Code[10, 7] = "a";
      this.Code[11, 7] = "a";
      this.Code[12, 7] = "a";
      this.Code[13, 7] = "a";
      this.Code[14, 7] = "a";
      this.Code[15, 7] = "a";
      this.Code[16, 7] = "a";
      this.Code[17, 7] = "a";
      this.Code[18, 7] = "e";
      this.Code[19, 7] = "e";
      this.Code[20, 7] = "e";
      this.Code[21, 7] = "e";
      this.Code[22, 7] = "e";
      this.Code[23, 7] = "e";
      this.Code[24, 7] = "e";
      this.Code[25, 7] = "e";
      this.Code[26, 7] = "e";
      this.Code[27, 7] = "e";
      this.Code[28, 7] = "e";
      this.Code[29, 7] = "i";
      this.Code[30, 7] = "i";
      this.Code[31, 7] = "i";
      this.Code[32, 7] = "i";
      this.Code[33, 7] = "i";
      this.Code[34, 7] = "o";
      this.Code[35, 7] = "o";
      this.Code[36, 7] = "o";
      this.Code[37, 7] = "o";
      this.Code[38, 7] = "o";
      this.Code[39, 7] = "o";
      this.Code[40, 7] = "o";
      this.Code[41, 7] = "o";
      this.Code[42, 7] = "o";
      this.Code[43, 7] = "o";
      this.Code[44, 7] = "o";
      this.Code[45, 7] = "o";
      this.Code[46, 7] = "o";
      this.Code[47, 7] = "o";
      this.Code[48, 7] = "o";
      this.Code[49, 7] = "o";
      this.Code[50, 7] = "o";
      this.Code[51, 7] = "u";
      this.Code[52, 7] = "u";
      this.Code[53, 7] = "u";
      this.Code[54, 7] = "u";
      this.Code[55, 7] = "u";
      this.Code[56, 7] = "u";
      this.Code[57, 7] = "u";
      this.Code[58, 7] = "u";
      this.Code[59, 7] = "u";
      this.Code[60, 7] = "u";
      this.Code[61, 7] = "u";
      this.Code[62, 7] = "y";
      this.Code[63, 7] = "y";
      this.Code[64, 7] = "y";
      this.Code[65, 7] = "y";
      this.Code[66, 7] = "y";
      this.Code[67, 7] = "d";
      this.Code[68, 7] = "A";
      this.Code[69, 7] = "A";
      this.Code[70, 7] = "A";
      this.Code[71, 7] = "A";
      this.Code[72, 7] = "A";
      this.Code[73, 7] = "A";
      this.Code[74, 7] = "A";
      this.Code[75, 7] = "A";
      this.Code[76, 7] = "A";
      this.Code[77, 7] = "A";
      this.Code[78, 7] = "A";
      this.Code[79, 7] = "A";
      this.Code[80, 7] = "A";
      this.Code[81, 7] = "A";
      this.Code[82, 7] = "A";
      this.Code[83, 7] = "A";
      this.Code[84, 7] = "A";
      this.Code[85, 7] = "E";
      this.Code[86, 7] = "E";
      this.Code[87, 7] = "E";
      this.Code[88, 7] = "E";
      this.Code[89, 7] = "E";
      this.Code[90, 7] = "E";
      this.Code[91, 7] = "E";
      this.Code[92, 7] = "E";
      this.Code[93, 7] = "E";
      this.Code[94, 7] = "E";
      this.Code[95, 7] = "E";
      this.Code[96, 7] = "I";
      this.Code[97, 7] = "I";
      this.Code[98, 7] = "I";
      this.Code[99, 7] = "I";
      this.Code[100, 7] = "I";
      this.Code[101, 7] = "O";
      this.Code[102, 7] = "O";
      this.Code[103, 7] = "O";
      this.Code[104, 7] = "O";
      this.Code[105, 7] = "O";
      this.Code[106, 7] = "O";
      this.Code[107, 7] = "O";
      this.Code[108, 7] = "O";
      this.Code[109, 7] = "O";
      this.Code[110, 7] = "O";
      this.Code[111, 7] = "O";
      this.Code[112, 7] = "O";
      this.Code[113, 7] = "O";
      this.Code[114, 7] = "O";
      this.Code[115, 7] = "O";
      this.Code[116, 7] = "O";
      this.Code[117, 7] = "O";
      this.Code[118, 7] = "U";
      this.Code[119, 7] = "U";
      this.Code[120, 7] = "U";
      this.Code[121, 7] = "U";
      this.Code[122, 7] = "U";
      this.Code[123, 7] = "U";
      this.Code[124, 7] = "U";
      this.Code[125, 7] = "U";
      this.Code[126, 7] = "U";
      this.Code[(int) sbyte.MaxValue, 7] = "U";
      this.Code[128, 7] = "U";
      this.Code[129, 7] = "Y";
      this.Code[130, 7] = "Y";
      this.Code[131, 7] = "Y";
      this.Code[132, 7] = "Y";
      this.Code[133, 7] = "Y";
      this.Code[134, 7] = "D";
    }

    private void MapTCV()
    {
      this.Code[1, 2] = "¸";
      this.Code[2, 2] = "µ";
      this.Code[3, 2] = "¶";
      this.Code[4, 2] = "·";
      this.Code[5, 2] = "\u00B9";
      this.Code[6, 2] = "¨";
      this.Code[7, 2] = "\u00BE";
      this.Code[8, 2] = "»";
      this.Code[9, 2] = "\u00BC";
      this.Code[10, 2] = "\u00BD";
      this.Code[11, 2] = "Æ";
      this.Code[12, 2] = "©";
      this.Code[13, 2] = "Ê";
      this.Code[14, 2] = "Ç";
      this.Code[15, 2] = "È";
      this.Code[16, 2] = "É";
      this.Code[17, 2] = "Ë";
      this.Code[18, 2] = "Ð";
      this.Code[19, 2] = "Ì";
      this.Code[20, 2] = "Î";
      this.Code[21, 2] = "Ï";
      this.Code[22, 2] = "Ñ";
      this.Code[23, 2] = "ª";
      this.Code[24, 2] = "Õ";
      this.Code[25, 2] = "Ò";
      this.Code[26, 2] = "Ó";
      this.Code[27, 2] = "Ô";
      this.Code[28, 2] = "Ö";
      this.Code[29, 2] = "Ý";
      this.Code[30, 2] = "×";
      this.Code[31, 2] = "Ø";
      this.Code[32, 2] = "Ü";
      this.Code[33, 2] = "Þ";
      this.Code[34, 2] = "ã";
      this.Code[35, 2] = "ß";
      this.Code[36, 2] = "á";
      this.Code[37, 2] = "â";
      this.Code[38, 2] = "ä";
      this.Code[39, 2] = "«";
      this.Code[40, 2] = "è";
      this.Code[41, 2] = "å";
      this.Code[42, 2] = "æ";
      this.Code[43, 2] = "ç";
      this.Code[44, 2] = "é";
      this.Code[45, 2] = "¬";
      this.Code[46, 2] = "í";
      this.Code[47, 2] = "ê";
      this.Code[48, 2] = "ë";
      this.Code[49, 2] = "ì";
      this.Code[50, 2] = "î";
      this.Code[51, 2] = "ó";
      this.Code[52, 2] = "ï";
      this.Code[53, 2] = "ñ";
      this.Code[54, 2] = "ò";
      this.Code[55, 2] = "ô";
      this.Code[56, 2] = "\u00AD";
      this.Code[57, 2] = "ø";
      this.Code[58, 2] = "õ";
      this.Code[59, 2] = "ö";
      this.Code[60, 2] = "÷";
      this.Code[61, 2] = "ù";
      this.Code[62, 2] = "ý";
      this.Code[63, 2] = "ú";
      this.Code[64, 2] = "û";
      this.Code[65, 2] = "ü";
      this.Code[66, 2] = "þ";
      this.Code[67, 2] = "®";
      this.Code[68, 2] = "¸";
      this.Code[69, 2] = "µ";
      this.Code[70, 2] = "¶";
      this.Code[71, 2] = "·";
      this.Code[72, 2] = "\u00B9";
      this.Code[73, 2] = "¡";
      this.Code[74, 2] = "\u00BE";
      this.Code[75, 2] = "»";
      this.Code[76, 2] = "\u00BC";
      this.Code[77, 2] = "\u00BD";
      this.Code[78, 2] = "Æ";
      this.Code[79, 2] = "¢";
      this.Code[80, 2] = "Ê";
      this.Code[81, 2] = "Ç";
      this.Code[82, 2] = "È";
      this.Code[83, 2] = "É";
      this.Code[84, 2] = "Ë";
      this.Code[85, 2] = "Ð";
      this.Code[86, 2] = "Ì";
      this.Code[87, 2] = "Î";
      this.Code[88, 2] = "Ï";
      this.Code[89, 2] = "Ñ";
      this.Code[90, 2] = "£";
      this.Code[91, 2] = "Õ";
      this.Code[92, 2] = "Ò";
      this.Code[93, 2] = "Ó";
      this.Code[94, 2] = "Ô";
      this.Code[95, 2] = "Ö";
      this.Code[96, 2] = "Ý";
      this.Code[97, 2] = "×";
      this.Code[98, 2] = "Ø";
      this.Code[99, 2] = "Ü";
      this.Code[100, 2] = "Þ";
      this.Code[101, 2] = "ã";
      this.Code[102, 2] = "ß";
      this.Code[103, 2] = "á";
      this.Code[104, 2] = "â";
      this.Code[105, 2] = "ä";
      this.Code[106, 2] = "¤";
      this.Code[107, 2] = "è";
      this.Code[108, 2] = "å";
      this.Code[109, 2] = "æ";
      this.Code[110, 2] = "ç";
      this.Code[111, 2] = "é";
      this.Code[112, 2] = "¥";
      this.Code[113, 2] = "í";
      this.Code[114, 2] = "ê";
      this.Code[115, 2] = "ë";
      this.Code[116, 2] = "ì";
      this.Code[117, 2] = "î";
      this.Code[118, 2] = "ó";
      this.Code[119, 2] = "ï";
      this.Code[120, 2] = "ñ";
      this.Code[121, 2] = "ò";
      this.Code[122, 2] = "ô";
      this.Code[123, 2] = "¦";
      this.Code[124, 2] = "ø";
      this.Code[125, 2] = "õ";
      this.Code[126, 2] = "ö";
      this.Code[(int) sbyte.MaxValue, 2] = "÷";
      this.Code[128, 2] = "ù";
      this.Code[129, 2] = "ý";
      this.Code[130, 2] = "ú";
      this.Code[131, 2] = "û";
      this.Code[132, 2] = "ü";
      this.Code[133, 2] = "þ";
      this.Code[134, 2] = "§";
    }

    private void MapUnicode()
    {
      this.Code[1, 6] = "á";
      this.Code[2, 6] = "à";
      this.Code[3, 6] = "ả";
      this.Code[4, 6] = "ã";
      this.Code[5, 6] = "ạ";
      this.Code[6, 6] = "ă";
      this.Code[7, 6] = "ắ";
      this.Code[8, 6] = "ằ";
      this.Code[9, 6] = "ẳ";
      this.Code[10, 6] = "ẵ";
      this.Code[11, 6] = "ặ";
      this.Code[12, 6] = "â";
      this.Code[13, 6] = "ấ";
      this.Code[14, 6] = "ầ";
      this.Code[15, 6] = "ẩ";
      this.Code[16, 6] = "ẫ";
      this.Code[17, 6] = "ậ";
      this.Code[18, 6] = "é";
      this.Code[19, 6] = "è";
      this.Code[20, 6] = "ẻ";
      this.Code[21, 6] = "ẽ";
      this.Code[22, 6] = "ẹ";
      this.Code[23, 6] = "ê";
      this.Code[24, 6] = "ế";
      this.Code[25, 6] = "ề";
      this.Code[26, 6] = "ể";
      this.Code[27, 6] = "ễ";
      this.Code[28, 6] = "ệ";
      this.Code[29, 6] = "í";
      this.Code[30, 6] = "ì";
      this.Code[31, 6] = "ỉ";
      this.Code[32, 6] = "ĩ";
      this.Code[33, 6] = "ị";
      this.Code[34, 6] = "ó";
      this.Code[35, 6] = "ò";
      this.Code[36, 6] = "ỏ";
      this.Code[37, 6] = "õ";
      this.Code[38, 6] = "ọ";
      this.Code[39, 6] = "ô";
      this.Code[40, 6] = "ố";
      this.Code[41, 6] = "ồ";
      this.Code[42, 6] = "ổ";
      this.Code[43, 6] = "ỗ";
      this.Code[44, 6] = "ộ";
      this.Code[45, 6] = "ơ";
      this.Code[46, 6] = "ớ";
      this.Code[47, 6] = "ờ";
      this.Code[48, 6] = "ở";
      this.Code[49, 6] = "ỡ";
      this.Code[50, 6] = "ợ";
      this.Code[51, 6] = "ú";
      this.Code[52, 6] = "ù";
      this.Code[53, 6] = "ủ";
      this.Code[54, 6] = "ũ";
      this.Code[55, 6] = "ụ";
      this.Code[56, 6] = "ư";
      this.Code[57, 6] = "ứ";
      this.Code[58, 6] = "ừ";
      this.Code[59, 6] = "ử";
      this.Code[60, 6] = "ữ";
      this.Code[61, 6] = "ự";
      this.Code[62, 6] = "ý";
      this.Code[63, 6] = "ỳ";
      this.Code[64, 6] = "ỷ";
      this.Code[65, 6] = "ỹ";
      this.Code[66, 6] = "ỵ";
      this.Code[67, 6] = "đ";
      this.Code[68, 6] = "Á";
      this.Code[69, 6] = "À";
      this.Code[70, 6] = "Ả";
      this.Code[71, 6] = "Ã";
      this.Code[72, 6] = "Ạ";
      this.Code[73, 6] = "Ă";
      this.Code[74, 6] = "Ắ";
      this.Code[75, 6] = "Ằ";
      this.Code[76, 6] = "Ẳ";
      this.Code[77, 6] = "Ẵ";
      this.Code[78, 6] = "Ặ";
      this.Code[79, 6] = "Â";
      this.Code[80, 6] = "Ấ";
      this.Code[81, 6] = "Ầ";
      this.Code[82, 6] = "Ẩ";
      this.Code[83, 6] = "Ẫ";
      this.Code[84, 6] = "Ậ";
      this.Code[85, 6] = "É";
      this.Code[86, 6] = "È";
      this.Code[87, 6] = "Ẻ";
      this.Code[88, 6] = "Ẽ";
      this.Code[89, 6] = "Ẹ";
      this.Code[90, 6] = "Ê";
      this.Code[91, 6] = "Ế";
      this.Code[92, 6] = "Ề";
      this.Code[93, 6] = "Ể";
      this.Code[94, 6] = "Ễ";
      this.Code[95, 6] = "Ệ";
      this.Code[96, 6] = "Í";
      this.Code[97, 6] = "Ì";
      this.Code[98, 6] = "Ỉ";
      this.Code[99, 6] = "Ĩ";
      this.Code[100, 6] = "Ị";
      this.Code[101, 6] = "Ó";
      this.Code[102, 6] = "Ò";
      this.Code[103, 6] = "Ỏ";
      this.Code[104, 6] = "Õ";
      this.Code[105, 6] = "Ọ";
      this.Code[106, 6] = "Ô";
      this.Code[107, 6] = "Ố";
      this.Code[108, 6] = "Ồ";
      this.Code[109, 6] = "Ổ";
      this.Code[110, 6] = "Ỗ";
      this.Code[111, 6] = "Ộ";
      this.Code[112, 6] = "Ơ";
      this.Code[113, 6] = "Ớ";
      this.Code[114, 6] = "Ờ";
      this.Code[115, 6] = "Ở";
      this.Code[116, 6] = "Ỡ";
      this.Code[117, 6] = "Ợ";
      this.Code[118, 6] = "Ú";
      this.Code[119, 6] = "Ù";
      this.Code[120, 6] = "Ủ";
      this.Code[121, 6] = "Ũ";
      this.Code[122, 6] = "Ụ";
      this.Code[123, 6] = "Ư";
      this.Code[124, 6] = "Ứ";
      this.Code[125, 6] = "Ừ";
      this.Code[126, 6] = "Ử";
      this.Code[(int) sbyte.MaxValue, 6] = "Ữ";
      this.Code[128, 6] = "Ự";
      this.Code[129, 6] = "Ý";
      this.Code[130, 6] = "Ỳ";
      this.Code[131, 6] = "Ỷ";
      this.Code[132, 6] = "Ỹ";
      this.Code[133, 6] = "Ỵ";
      this.Code[134, 6] = "Đ";
    }

    private void MapUTF8()
    {
      this.Code[1, 1] = "Ã¡";
      this.Code[2, 1] = "Ã ";
      this.Code[3, 1] = "áº£";
      this.Code[4, 1] = "Ã£";
      this.Code[5, 1] = "áº¡";
      this.Code[6, 1] = "Äƒ";
      this.Code[7, 1] = "áº¯";
      this.Code[8, 1] = "áº±";
      this.Code[9, 1] = "áº\u00B3";
      this.Code[10, 1] = "áºµ";
      this.Code[11, 1] = "áº·";
      this.Code[12, 1] = "Ã¢";
      this.Code[13, 1] = "áº¥";
      this.Code[14, 1] = "áº§";
      this.Code[15, 1] = "áº©";
      this.Code[16, 1] = "áº«";
      this.Code[17, 1] = "áº\u00AD";
      this.Code[18, 1] = "Ã©";
      this.Code[19, 1] = "Ã¨";
      this.Code[20, 1] = "áº»";
      this.Code[21, 1] = "áº\u00BD";
      this.Code[22, 1] = "áº\u00B9";
      this.Code[23, 1] = "Ãª";
      this.Code[24, 1] = "áº¿";
      this.Code[25, 1] = "á»\u0081";
      this.Code[26, 1] = "á»ƒ";
      this.Code[27, 1] = "á»…";
      this.Code[28, 1] = "á»‡";
      this.Code[29, 1] = "Ã\u00AD";
      this.Code[30, 1] = "Ã¬";
      this.Code[31, 1] = "á»‰";
      this.Code[32, 1] = "Ä©";
      this.Code[33, 1] = "á»‹";
      this.Code[34, 1] = "Ã\u00B3";
      this.Code[35, 1] = "Ã\u00B2";
      this.Code[36, 1] = "á»\u008F";
      this.Code[37, 1] = "Ãµ";
      this.Code[38, 1] = "á»\u008D";
      this.Code[39, 1] = "Ã´";
      this.Code[40, 1] = "á»‘";
      this.Code[41, 1] = "á»“";
      this.Code[42, 1] = "á»•";
      this.Code[43, 1] = "á»—";
      this.Code[44, 1] = "á»™";
      this.Code[45, 1] = "Æ¡";
      this.Code[46, 1] = "á»›";
      this.Code[47, 1] = "á»\u009D";
      this.Code[48, 1] = "á»Ÿ";
      this.Code[49, 1] = "á»¡";
      this.Code[50, 1] = "á»£";
      this.Code[51, 1] = "Ãº";
      this.Code[52, 1] = "Ã\u00B9";
      this.Code[53, 1] = "á»§";
      this.Code[54, 1] = "Å©";
      this.Code[55, 1] = "á»¥";
      this.Code[56, 1] = "Æ°";
      this.Code[57, 1] = "á»©";
      this.Code[58, 1] = "á»«";
      this.Code[59, 1] = "á»\u00AD";
      this.Code[60, 1] = "á»¯";
      this.Code[61, 1] = "á»±";
      this.Code[62, 1] = "Ã\u00BD";
      this.Code[63, 1] = "á»\u00B3";
      this.Code[64, 1] = "\u009Dá»·".Substring(1);
      this.Code[65, 1] = "á»\u00B9";
      this.Code[66, 1] = "á»µ";
      this.Code[67, 1] = "Ä‘";
      this.Code[68, 1] = "Ã\u0081";
      this.Code[69, 1] = "Ã€";
      this.Code[70, 1] = "áº¢";
      this.Code[71, 1] = "Ãƒ";
      this.Code[72, 1] = "áº ";
      this.Code[73, 1] = "Ä‚";
      this.Code[74, 1] = "áº®";
      this.Code[75, 1] = "áº°";
      this.Code[76, 1] = "áº\u00B2";
      this.Code[77, 1] = "áº´";
      this.Code[78, 1] = "áº¶";
      this.Code[79, 1] = "Ã‚";
      this.Code[80, 1] = "áº¤";
      this.Code[81, 1] = "áº¦";
      this.Code[82, 1] = "áº¨";
      this.Code[83, 1] = "áºª";
      this.Code[84, 1] = "áº¬";
      this.Code[85, 1] = "Ã‰";
      this.Code[86, 1] = "Ãˆ";
      this.Code[87, 1] = "áºº";
      this.Code[88, 1] = "áº\u00BC";
      this.Code[89, 1] = "áº¸";
      this.Code[90, 1] = "ÃŠ";
      this.Code[91, 1] = "áº\u00BE";
      this.Code[92, 1] = "á»€";
      this.Code[93, 1] = "á»‚";
      this.Code[94, 1] = "á»„";
      this.Code[95, 1] = "á»†";
      this.Code[96, 1] = "Ã\u008D";
      this.Code[97, 1] = "ÃŒ";
      this.Code[98, 1] = "á»ˆ";
      this.Code[99, 1] = "Ä¨";
      this.Code[100, 1] = "á»Š";
      this.Code[101, 1] = "Ã“";
      this.Code[102, 1] = "Ã’";
      this.Code[103, 1] = "á»Ž";
      this.Code[104, 1] = "Ã•";
      this.Code[105, 1] = "á»Œ";
      this.Code[106, 1] = "Ã”";
      this.Code[107, 1] = "á»\u0090";
      this.Code[108, 1] = "á»’";
      this.Code[109, 1] = "á»”";
      this.Code[110, 1] = "á»–";
      this.Code[111, 1] = "á»˜";
      this.Code[112, 1] = "Æ ";
      this.Code[113, 1] = "á»š";
      this.Code[114, 1] = "á»œ";
      this.Code[115, 1] = "á»ž";
      this.Code[116, 1] = "á» ";
      this.Code[117, 1] = "á»¢";
      this.Code[118, 1] = "Ãš";
      this.Code[119, 1] = "Ã™";
      this.Code[120, 1] = "á»¦";
      this.Code[121, 1] = "Å¨";
      this.Code[122, 1] = "á»¤";
      this.Code[123, 1] = "Æ¯";
      this.Code[124, 1] = "á»¨";
      this.Code[125, 1] = "á»ª";
      this.Code[126, 1] = "á»¬";
      this.Code[(int) sbyte.MaxValue, 1] = "á»®";
      this.Code[128, 1] = "á»°";
      this.Code[129, 1] = "Ã\u009D";
      this.Code[130, 1] = "á»\u00B2";
      this.Code[131, 1] = "á»¶";
      this.Code[132, 1] = "á»¸";
      this.Code[133, 1] = "á»´";
      this.Code[134, 1] = "Ä\u0090";
    }

    private void MapUTH()
    {
      this.Code[1, 5] = "á";
      this.Code[2, 5] = "à";
      this.Code[3, 5] = "ả";
      this.Code[4, 5] = "ã";
      this.Code[5, 5] = "ạ";
      this.Code[6, 5] = "ă";
      this.Code[7, 5] = "ắ";
      this.Code[8, 5] = "ằ";
      this.Code[9, 5] = "ẳ";
      this.Code[10, 5] = "ẵ";
      this.Code[11, 5] = "ặ";
      this.Code[12, 5] = "â";
      this.Code[13, 5] = "ấ";
      this.Code[14, 5] = "ầ";
      this.Code[15, 5] = "ẩ";
      this.Code[16, 5] = "ẫ";
      this.Code[17, 5] = "ậ";
      this.Code[18, 5] = "é";
      this.Code[19, 5] = "è";
      this.Code[20, 5] = "ẻ";
      this.Code[21, 5] = "ẽ";
      this.Code[22, 5] = "ẹ";
      this.Code[23, 5] = "ê";
      this.Code[24, 5] = "ế";
      this.Code[25, 5] = "ề";
      this.Code[26, 5] = "ể";
      this.Code[27, 5] = "ễ";
      this.Code[28, 5] = "ệ";
      this.Code[29, 5] = "í";
      this.Code[30, 5] = "ì";
      this.Code[31, 5] = "ỉ";
      this.Code[32, 5] = "ĩ";
      this.Code[33, 5] = "ị";
      this.Code[34, 5] = "ó";
      this.Code[35, 5] = "ò";
      this.Code[36, 5] = "ỏ";
      this.Code[37, 5] = "õ";
      this.Code[38, 5] = "ọ";
      this.Code[39, 5] = "ô";
      this.Code[40, 5] = "ố";
      this.Code[41, 5] = "ồ";
      this.Code[42, 5] = "ổ";
      this.Code[43, 5] = "ỗ";
      this.Code[44, 5] = "ộ";
      this.Code[45, 5] = "ơ";
      this.Code[46, 5] = "ớ";
      this.Code[47, 5] = "ờ";
      this.Code[48, 5] = "ở";
      this.Code[49, 5] = "ỡ";
      this.Code[50, 5] = "ợ";
      this.Code[51, 5] = "ú";
      this.Code[52, 5] = "ù";
      this.Code[53, 5] = "ủ";
      this.Code[54, 5] = "ũ";
      this.Code[55, 5] = "ụ";
      this.Code[56, 5] = "ư";
      this.Code[57, 5] = "ứ";
      this.Code[58, 5] = "ừ";
      this.Code[59, 5] = "ử";
      this.Code[60, 5] = "ữ";
      this.Code[61, 5] = "ự";
      this.Code[62, 5] = "ý";
      this.Code[63, 5] = "ỳ";
      this.Code[64, 5] = "ỷ";
      this.Code[65, 5] = "ỹ";
      this.Code[66, 5] = "ỵ";
      this.Code[67, 5] = "đ";
      this.Code[68, 5] = "Á";
      this.Code[69, 5] = "À";
      this.Code[70, 5] = "Ả";
      this.Code[71, 5] = "Ã";
      this.Code[72, 5] = "Ạ";
      this.Code[73, 5] = "Ă";
      this.Code[74, 5] = "Ắ";
      this.Code[75, 5] = "Ằ";
      this.Code[76, 5] = "Ẳ";
      this.Code[77, 5] = "Ẵ";
      this.Code[78, 5] = "Ặ";
      this.Code[79, 5] = "Â";
      this.Code[80, 5] = "Ấ";
      this.Code[81, 5] = "Ầ";
      this.Code[82, 5] = "Ẩ";
      this.Code[83, 5] = "Ẫ";
      this.Code[84, 5] = "Ậ";
      this.Code[85, 5] = "É";
      this.Code[86, 5] = "È";
      this.Code[87, 5] = "Ẻ";
      this.Code[88, 5] = "Ẽ";
      this.Code[89, 5] = "Ẹ";
      this.Code[90, 5] = "Ê";
      this.Code[91, 5] = "Ế";
      this.Code[92, 5] = "Ề";
      this.Code[93, 5] = "Ể";
      this.Code[94, 5] = "Ễ";
      this.Code[95, 5] = "Ệ";
      this.Code[96, 5] = "Í";
      this.Code[97, 5] = "Ì";
      this.Code[98, 5] = "Ỉ";
      this.Code[99, 5] = "Ĩ";
      this.Code[100, 5] = "Ị";
      this.Code[101, 5] = "Ó";
      this.Code[102, 5] = "Ò";
      this.Code[103, 5] = "Ỏ";
      this.Code[104, 5] = "Õ";
      this.Code[105, 5] = "Ọ";
      this.Code[106, 5] = "Ô";
      this.Code[107, 5] = "Ố";
      this.Code[108, 5] = "Ồ";
      this.Code[109, 5] = "Ổ";
      this.Code[110, 5] = "Ỗ";
      this.Code[111, 5] = "Ộ";
      this.Code[112, 5] = "Ơ";
      this.Code[113, 5] = "Ớ";
      this.Code[114, 5] = "Ờ";
      this.Code[115, 5] = "Ở";
      this.Code[116, 5] = "Ỡ";
      this.Code[117, 5] = "Ợ";
      this.Code[118, 5] = "Ú";
      this.Code[119, 5] = "Ù";
      this.Code[120, 5] = "Ủ";
      this.Code[121, 5] = "Ũ";
      this.Code[122, 5] = "Ụ";
      this.Code[123, 5] = "Ư";
      this.Code[124, 5] = "Ứ";
      this.Code[125, 5] = "Ừ";
      this.Code[126, 5] = "Ử";
      this.Code[(int) sbyte.MaxValue, 5] = "Ữ";
      this.Code[128, 5] = "Ự";
      this.Code[129, 5] = "Ý";
      this.Code[130, 5] = "Ỳ";
      this.Code[131, 5] = "Ỷ";
      this.Code[132, 5] = "Ỹ";
      this.Code[133, 5] = "Ỵ";
      this.Code[134, 5] = "Đ";
    }

    private void MapVietwareF()
    {
      this.Code[1, 6] = "À";
      this.Code[2, 6] = "ª";
      this.Code[3, 6] = "¶";
      this.Code[4, 6] = "º";
      this.Code[5, 6] = "Á";
      this.Code[6, 6] = "Ÿ";
      this.Code[7, 6] = "Å";
      this.Code[8, 6] = "Â";
      this.Code[9, 6] = "Ã";
      this.Code[10, 6] = "Ä";
      this.Code[11, 6] = "Æ";
      this.Code[12, 6] = "¡";
      this.Code[13, 6] = "Ê";
      this.Code[14, 6] = "Ç";
      this.Code[15, 6] = "È";
      this.Code[16, 6] = "É";
      this.Code[17, 6] = "Ë";
      this.Code[18, 6] = "Ï";
      this.Code[19, 6] = "Ì";
      this.Code[20, 6] = "Í";
      this.Code[21, 6] = "Î";
      this.Code[22, 6] = "Ñ";
      this.Code[23, 6] = "£";
      this.Code[24, 6] = "Õ";
      this.Code[25, 6] = "Ò";
      this.Code[26, 6] = "Ó";
      this.Code[27, 6] = "Ô";
      this.Code[28, 6] = "Ö";
      this.Code[29, 6] = "Û";
      this.Code[30, 6] = "Ø";
      this.Code[31, 6] = "Ù";
      this.Code[32, 6] = "Ú";
      this.Code[33, 6] = "Ü";
      this.Code[34, 6] = "â";
      this.Code[35, 6] = "ß";
      this.Code[36, 6] = "à";
      this.Code[37, 6] = "á";
      this.Code[38, 6] = "ã";
      this.Code[39, 6] = "¤";
      this.Code[40, 6] = "ç";
      this.Code[41, 6] = "ä";
      this.Code[42, 6] = "å";
      this.Code[43, 6] = "æ";
      this.Code[44, 6] = "è";
      this.Code[45, 6] = "¥";
      this.Code[46, 6] = "ì";
      this.Code[47, 6] = "é";
      this.Code[48, 6] = "ê";
      this.Code[49, 6] = "ë";
      this.Code[50, 6] = "í";
      this.Code[51, 6] = "ò";
      this.Code[52, 6] = "î";
      this.Code[53, 6] = "ï";
      this.Code[54, 6] = "ñ";
      this.Code[55, 6] = "ó";
      this.Code[56, 6] = "§";
      this.Code[57, 6] = "÷";
      this.Code[58, 6] = "ô";
      this.Code[59, 6] = "õ";
      this.Code[60, 6] = "ö";
      this.Code[61, 6] = "ø";
      this.Code[62, 6] = "ü";
      this.Code[63, 6] = "ù";
      this.Code[64, 6] = "ú";
      this.Code[65, 6] = "û";
      this.Code[66, 6] = "ÿ";
      this.Code[67, 6] = "¢";
      this.Code[68, 6] = "À";
      this.Code[69, 6] = "ª";
      this.Code[70, 6] = "¶";
      this.Code[71, 6] = "º";
      this.Code[72, 6] = "Á";
      this.Code[73, 6] = "–";
      this.Code[74, 6] = "Å";
      this.Code[75, 6] = "Â";
      this.Code[76, 6] = "Ã";
      this.Code[77, 6] = "Ä";
      this.Code[78, 6] = "Æ";
      this.Code[79, 6] = "—";
      this.Code[80, 6] = "Ê";
      this.Code[81, 6] = "Ç";
      this.Code[82, 6] = "È";
      this.Code[83, 6] = "É";
      this.Code[84, 6] = "Ë";
      this.Code[85, 6] = "Ï";
      this.Code[86, 6] = "Ì";
      this.Code[87, 6] = "Í";
      this.Code[88, 6] = "Î";
      this.Code[89, 6] = "Ñ";
      this.Code[90, 6] = "™";
      this.Code[91, 6] = "Õ";
      this.Code[92, 6] = "Ò";
      this.Code[93, 6] = "Ó";
      this.Code[94, 6] = "Ô";
      this.Code[95, 6] = "Ö";
      this.Code[96, 6] = "Û";
      this.Code[97, 6] = "Ø";
      this.Code[98, 6] = "Ù";
      this.Code[99, 6] = "Ú";
      this.Code[100, 6] = "Ü";
      this.Code[101, 6] = "â";
      this.Code[102, 6] = "ß";
      this.Code[103, 6] = "à";
      this.Code[104, 6] = "á";
      this.Code[105, 6] = "ã";
      this.Code[106, 6] = "š";
      this.Code[107, 6] = "ç";
      this.Code[108, 6] = "ä";
      this.Code[109, 6] = "å";
      this.Code[110, 6] = "æ";
      this.Code[111, 6] = "è";
      this.Code[112, 6] = "›";
      this.Code[113, 6] = "ì";
      this.Code[114, 6] = "é";
      this.Code[115, 6] = "ê";
      this.Code[116, 6] = "ë";
      this.Code[117, 6] = "í";
      this.Code[118, 6] = "ò";
      this.Code[119, 6] = "î";
      this.Code[120, 6] = "ï";
      this.Code[121, 6] = "ñ";
      this.Code[122, 6] = "ó";
      this.Code[123, 6] = "œ";
      this.Code[124, 6] = "÷";
      this.Code[125, 6] = "ô";
      this.Code[126, 6] = "õ";
      this.Code[(int) sbyte.MaxValue, 6] = "ö";
      this.Code[128, 6] = "ø";
      this.Code[129, 6] = "ü";
      this.Code[130, 6] = "ù";
      this.Code[131, 6] = "ú";
      this.Code[132, 6] = "û";
      this.Code[133, 6] = "ÿ";
      this.Code[134, 6] = "˜";
    }

    private void MapVietwareX()
    {
      this.Code[1, 6] = "aï";
      this.Code[2, 6] = "aì";
      this.Code[3, 6] = "aí";
      this.Code[4, 6] = "aî";
      this.Code[5, 6] = "aû";
      this.Code[6, 6] = "à";
      this.Code[7, 6] = "àõ";
      this.Code[8, 6] = "àò";
      this.Code[9, 6] = "àó";
      this.Code[10, 6] = "àô";
      this.Code[11, 6] = "àû";
      this.Code[12, 6] = "á";
      this.Code[13, 6] = "áú";
      this.Code[14, 6] = "áö";
      this.Code[15, 6] = "áø";
      this.Code[16, 6] = "áù";
      this.Code[17, 6] = "áû";
      this.Code[18, 6] = "eï";
      this.Code[19, 6] = "eì";
      this.Code[20, 6] = "eí";
      this.Code[21, 6] = "eî";
      this.Code[22, 6] = "eû";
      this.Code[23, 6] = "ã";
      this.Code[24, 6] = "ãú";
      this.Code[25, 6] = "ãö";
      this.Code[26, 6] = "ãø";
      this.Code[27, 6] = "ãù";
      this.Code[28, 6] = "ãû";
      this.Code[29, 6] = "ê";
      this.Code[30, 6] = "ç";
      this.Code[31, 6] = "è";
      this.Code[32, 6] = "é";
      this.Code[33, 6] = "ë";
      this.Code[34, 6] = "oï";
      this.Code[35, 6] = "oì";
      this.Code[36, 6] = "oí";
      this.Code[37, 6] = "oî";
      this.Code[38, 6] = "oü";
      this.Code[39, 6] = "ä";
      this.Code[40, 6] = "äú";
      this.Code[41, 6] = "äö";
      this.Code[42, 6] = "äø";
      this.Code[43, 6] = "äù";
      this.Code[44, 6] = "äü";
      this.Code[45, 6] = "å";
      this.Code[46, 6] = "åï";
      this.Code[47, 6] = "åì";
      this.Code[48, 6] = "åí";
      this.Code[49, 6] = "åî";
      this.Code[50, 6] = "åü";
      this.Code[51, 6] = "uï";
      this.Code[52, 6] = "uì";
      this.Code[53, 6] = "uí";
      this.Code[54, 6] = "uî";
      this.Code[55, 6] = "uû";
      this.Code[56, 6] = "æ";
      this.Code[57, 6] = "æï";
      this.Code[58, 6] = "æì";
      this.Code[59, 6] = "æí";
      this.Code[60, 6] = "æî";
      this.Code[61, 6] = "æû";
      this.Code[62, 6] = "yï";
      this.Code[63, 6] = "yì";
      this.Code[64, 6] = "yí";
      this.Code[65, 6] = "yî";
      this.Code[66, 6] = "yñ";
      this.Code[67, 6] = "â";
      this.Code[68, 6] = "AÏ";
      this.Code[69, 6] = "AÌ";
      this.Code[70, 6] = "AÍ";
      this.Code[71, 6] = "AÎ";
      this.Code[72, 6] = "AÛ";
      this.Code[73, 6] = "À";
      this.Code[74, 6] = "ÀÕ";
      this.Code[75, 6] = "ÀÒ";
      this.Code[76, 6] = "ÀÓ";
      this.Code[77, 6] = "ÀÔ";
      this.Code[78, 6] = "ÀÛ";
      this.Code[79, 6] = "Á";
      this.Code[80, 6] = "ÁÚ";
      this.Code[81, 6] = "ÁÖ";
      this.Code[82, 6] = "ÁØ";
      this.Code[83, 6] = "ÁÙ";
      this.Code[84, 6] = "ÁÛ";
      this.Code[85, 6] = "EÏ";
      this.Code[86, 6] = "EÌ";
      this.Code[87, 6] = "EÍ";
      this.Code[88, 6] = "EÎ";
      this.Code[89, 6] = "EÛ";
      this.Code[90, 6] = "Ã";
      this.Code[91, 6] = "ÃÚ";
      this.Code[92, 6] = "ÃÖ";
      this.Code[93, 6] = "ÃØ";
      this.Code[94, 6] = "ÃÙ";
      this.Code[95, 6] = "ÃÛ";
      this.Code[96, 6] = "Ê";
      this.Code[97, 6] = "Ç";
      this.Code[98, 6] = "È";
      this.Code[99, 6] = "É";
      this.Code[100, 6] = "Ë";
      this.Code[101, 6] = "OÏ";
      this.Code[102, 6] = "OÌ";
      this.Code[103, 6] = "OÍ";
      this.Code[104, 6] = "OÎ";
      this.Code[105, 6] = "OÜ";
      this.Code[106, 6] = "Ä";
      this.Code[107, 6] = "ÄÚ";
      this.Code[108, 6] = "ÄÖ";
      this.Code[109, 6] = "ÄØ";
      this.Code[110, 6] = "ÄÙ";
      this.Code[111, 6] = "ÄÜ";
      this.Code[112, 6] = "Å";
      this.Code[113, 6] = "ÅÏ";
      this.Code[114, 6] = "ÅÌ";
      this.Code[115, 6] = "ÅÍ";
      this.Code[116, 6] = "ÅÎ";
      this.Code[117, 6] = "ÅÜ";
      this.Code[118, 6] = "UÏ";
      this.Code[119, 6] = "UÌ";
      this.Code[120, 6] = "UÍ";
      this.Code[121, 6] = "UÎ";
      this.Code[122, 6] = "UÛ";
      this.Code[123, 6] = "Æ";
      this.Code[124, 6] = "ÆÏ";
      this.Code[125, 6] = "ÆÌ";
      this.Code[126, 6] = "ÆÍ";
      this.Code[(int) sbyte.MaxValue, 6] = "ÆÎ";
      this.Code[128, 6] = "ÆÛ";
      this.Code[129, 6] = "YÏ";
      this.Code[130, 6] = "YÌ";
      this.Code[131, 6] = "YÍ";
      this.Code[132, 6] = "YÎ";
      this.Code[133, 6] = "YÑ";
      this.Code[134, 6] = "Â";
    }

    private void MapVIQR()
    {
      this.Code[1, 8] = "a'";
      this.Code[2, 8] = "a`";
      this.Code[3, 8] = "a?";
      this.Code[4, 8] = "a~";
      this.Code[5, 8] = "a.";
      this.Code[6, 8] = "a(";
      this.Code[7, 8] = "a('";
      this.Code[8, 8] = "a(`";
      this.Code[9, 8] = "a(?";
      this.Code[10, 8] = "a(~";
      this.Code[11, 8] = "a(.";
      this.Code[12, 8] = "a^";
      this.Code[13, 8] = "a^'";
      this.Code[14, 8] = "a^`";
      this.Code[15, 8] = "a^?";
      this.Code[16, 8] = "a^~";
      this.Code[17, 8] = "a^.";
      this.Code[18, 8] = "e'";
      this.Code[19, 8] = "e`";
      this.Code[20, 8] = "e?";
      this.Code[21, 8] = "e~";
      this.Code[22, 8] = "e.";
      this.Code[23, 8] = "e^";
      this.Code[24, 8] = "e^'";
      this.Code[25, 8] = "e^`";
      this.Code[26, 8] = "e^?";
      this.Code[27, 8] = "e^~";
      this.Code[28, 8] = "e^.";
      this.Code[29, 8] = "i'";
      this.Code[30, 8] = "i`";
      this.Code[31, 8] = "i?";
      this.Code[32, 8] = "i~";
      this.Code[33, 8] = "i.";
      this.Code[34, 8] = "o'";
      this.Code[35, 8] = "o`";
      this.Code[36, 8] = "o?";
      this.Code[37, 8] = "o~";
      this.Code[38, 8] = "o.";
      this.Code[39, 8] = "o^";
      this.Code[40, 8] = "o^'";
      this.Code[41, 8] = "o^`";
      this.Code[42, 8] = "o^?";
      this.Code[43, 8] = "o^~";
      this.Code[44, 8] = "o^.";
      this.Code[45, 8] = "o+";
      this.Code[46, 8] = "o+'";
      this.Code[47, 8] = "o+`";
      this.Code[48, 8] = "o+?";
      this.Code[49, 8] = "o+~";
      this.Code[50, 8] = "o+.";
      this.Code[51, 8] = "u'";
      this.Code[52, 8] = "u`";
      this.Code[53, 8] = "u?";
      this.Code[54, 8] = "u~";
      this.Code[55, 8] = "u.";
      this.Code[56, 8] = "u+";
      this.Code[57, 8] = "u+'";
      this.Code[58, 8] = "u+`";
      this.Code[59, 8] = "u+?";
      this.Code[60, 8] = "u+~";
      this.Code[61, 8] = "u+.";
      this.Code[62, 8] = "y'";
      this.Code[63, 8] = "y`";
      this.Code[64, 8] = "y?";
      this.Code[65, 8] = "y~";
      this.Code[66, 8] = "y.";
      this.Code[67, 8] = "dd";
      this.Code[68, 8] = "A'";
      this.Code[69, 8] = "A`";
      this.Code[70, 8] = "A?";
      this.Code[71, 8] = "A~";
      this.Code[72, 8] = "A.";
      this.Code[73, 8] = "A(";
      this.Code[74, 8] = "A('";
      this.Code[75, 8] = "A(`";
      this.Code[76, 8] = "A(?";
      this.Code[77, 8] = "A(~";
      this.Code[78, 8] = "A(.";
      this.Code[79, 8] = "A^";
      this.Code[80, 8] = "A^'";
      this.Code[81, 8] = "A^`";
      this.Code[82, 8] = "A^?";
      this.Code[83, 8] = "A^~";
      this.Code[84, 8] = "A^.";
      this.Code[85, 8] = "E'";
      this.Code[86, 8] = "E`";
      this.Code[87, 8] = "E?";
      this.Code[88, 8] = "E~";
      this.Code[89, 8] = "E.";
      this.Code[90, 8] = "E^";
      this.Code[91, 8] = "E^'";
      this.Code[92, 8] = "E^`";
      this.Code[93, 8] = "E^?";
      this.Code[94, 8] = "E^~";
      this.Code[95, 8] = "E^.";
      this.Code[96, 8] = "I'";
      this.Code[97, 8] = "I`";
      this.Code[98, 8] = "I?";
      this.Code[99, 8] = "I~";
      this.Code[100, 8] = "I.";
      this.Code[101, 8] = "O'";
      this.Code[102, 8] = "O`";
      this.Code[103, 8] = "O?";
      this.Code[104, 8] = "O~";
      this.Code[105, 8] = "O.";
      this.Code[106, 8] = "O^";
      this.Code[107, 8] = "O^'";
      this.Code[108, 8] = "O^`";
      this.Code[109, 8] = "O^?";
      this.Code[110, 8] = "O^~";
      this.Code[111, 8] = "O^.";
      this.Code[112, 8] = "O+";
      this.Code[113, 8] = "O+'";
      this.Code[114, 8] = "O+`";
      this.Code[115, 8] = "O+?";
      this.Code[116, 8] = "O+~";
      this.Code[117, 8] = "O+.";
      this.Code[118, 8] = "U'";
      this.Code[119, 8] = "U`";
      this.Code[120, 8] = "U?";
      this.Code[121, 8] = "U~";
      this.Code[122, 8] = "U.";
      this.Code[123, 8] = "U+";
      this.Code[124, 8] = "U+'";
      this.Code[125, 8] = "U+`";
      this.Code[126, 8] = "U+?";
      this.Code[(int) sbyte.MaxValue, 8] = "U+~";
      this.Code[128, 8] = "U+.";
      this.Code[129, 8] = "Y'";
      this.Code[130, 8] = "Y`";
      this.Code[131, 8] = "Y?";
      this.Code[132, 8] = "Y~";
      this.Code[133, 8] = "Y.";
      this.Code[134, 8] = "DD";
    }

    private void MapVISCII()
    {
      this.Code[1, 6] = "á";
      this.Code[2, 6] = "à";
      this.Code[3, 6] = "ä";
      this.Code[4, 6] = "ã";
      this.Code[5, 6] = "Õ";
      this.Code[6, 6] = "å";
      this.Code[7, 6] = "¡";
      this.Code[8, 6] = "¢";
      this.Code[9, 6] = "Æ";
      this.Code[10, 6] = "Ç";
      this.Code[11, 6] = "£";
      this.Code[12, 6] = "â";
      this.Code[13, 6] = "¤";
      this.Code[14, 6] = "¥";
      this.Code[15, 6] = "¦";
      this.Code[16, 6] = "ç";
      this.Code[17, 6] = "§";
      this.Code[18, 6] = "é";
      this.Code[19, 6] = "è";
      this.Code[20, 6] = "ë";
      this.Code[21, 6] = "¨";
      this.Code[22, 6] = "©";
      this.Code[23, 6] = "ê";
      this.Code[24, 6] = "ª";
      this.Code[25, 6] = "«";
      this.Code[26, 6] = "¬";
      this.Code[27, 6] = "\u00AD";
      this.Code[28, 6] = "®";
      this.Code[29, 6] = "í";
      this.Code[30, 6] = "ì";
      this.Code[31, 6] = "ï";
      this.Code[32, 6] = "î";
      this.Code[33, 6] = "¸";
      this.Code[34, 6] = "ó";
      this.Code[35, 6] = "ò";
      this.Code[36, 6] = "ö";
      this.Code[37, 6] = "õ";
      this.Code[38, 6] = "÷";
      this.Code[39, 6] = "ô";
      this.Code[40, 6] = "¯";
      this.Code[41, 6] = "°";
      this.Code[42, 6] = "±";
      this.Code[43, 6] = "\u00B2";
      this.Code[44, 6] = "µ";
      this.Code[45, 6] = "\u00BD";
      this.Code[46, 6] = "\u00BE";
      this.Code[47, 6] = "¶";
      this.Code[48, 6] = "·";
      this.Code[49, 6] = "Þ";
      this.Code[50, 6] = "þ";
      this.Code[51, 6] = "ú";
      this.Code[52, 6] = "ù";
      this.Code[53, 6] = "ü";
      this.Code[54, 6] = "û";
      this.Code[55, 6] = "ø";
      this.Code[56, 6] = "ß";
      this.Code[57, 6] = "Ñ";
      this.Code[58, 6] = "×";
      this.Code[59, 6] = "Ø";
      this.Code[60, 6] = "æ";
      this.Code[61, 6] = "ñ";
      this.Code[62, 6] = "ý";
      this.Code[63, 6] = "Ï";
      this.Code[64, 6] = "Ö";
      this.Code[65, 6] = "Û";
      this.Code[66, 6] = "Ü";
      this.Code[67, 6] = "ð";
      this.Code[68, 6] = "Á";
      this.Code[69, 6] = "À";
      this.Code[70, 6] = "Ä";
      this.Code[71, 6] = "Ã";
      this.Code[72, 6] = "€";
      this.Code[73, 6] = "Å";
      this.Code[74, 6] = "\u0081";
      this.Code[75, 6] = "‚";
      this.Code[76, 6] = "Æ";
      this.Code[77, 6] = "Ç";
      this.Code[78, 6] = "ƒ";
      this.Code[79, 6] = "Â";
      this.Code[80, 6] = "„";
      this.Code[81, 6] = "…";
      this.Code[82, 6] = "†";
      this.Code[83, 6] = "ç";
      this.Code[84, 6] = "‡";
      this.Code[85, 6] = "É";
      this.Code[86, 6] = "È";
      this.Code[87, 6] = "Ë";
      this.Code[88, 6] = "ˆ";
      this.Code[89, 6] = "‰";
      this.Code[90, 6] = "Ê";
      this.Code[91, 6] = "Š";
      this.Code[92, 6] = "‹";
      this.Code[93, 6] = "Œ";
      this.Code[94, 6] = "\u008D";
      this.Code[95, 6] = "Ž";
      this.Code[96, 6] = "Í";
      this.Code[97, 6] = "Ì";
      this.Code[98, 6] = "›";
      this.Code[99, 6] = "Î";
      this.Code[100, 6] = "˜";
      this.Code[101, 6] = "Ó";
      this.Code[102, 6] = "Ò";
      this.Code[103, 6] = "™";
      this.Code[104, 6] = "õ";
      this.Code[105, 6] = "š";
      this.Code[106, 6] = "Ô";
      this.Code[107, 6] = "\u008F";
      this.Code[108, 6] = "\u0090";
      this.Code[109, 6] = "‘";
      this.Code[110, 6] = "’";
      this.Code[111, 6] = "“";
      this.Code[112, 6] = "´";
      this.Code[113, 6] = "•";
      this.Code[114, 6] = "–";
      this.Code[115, 6] = "—";
      this.Code[116, 6] = "\u00B3";
      this.Code[117, 6] = "”";
      this.Code[118, 6] = "Ú";
      this.Code[119, 6] = "Ù";
      this.Code[120, 6] = "œ";
      this.Code[121, 6] = "\u009D";
      this.Code[122, 6] = "ž";
      this.Code[123, 6] = "¿";
      this.Code[124, 6] = "º";
      this.Code[125, 6] = "»";
      this.Code[126, 6] = "\u00BC";
      this.Code[(int) sbyte.MaxValue, 6] = "ÿ";
      this.Code[128, 6] = "\u00B9";
      this.Code[129, 6] = "Ý";
      this.Code[130, 6] = "Ÿ";
      this.Code[131, 6] = "Ö";
      this.Code[132, 6] = "Û";
      this.Code[133, 6] = "Ü";
      this.Code[134, 6] = "Ð";
    }

    private void MapVNI()
    {
      this.Code[1, 3] = "aù";
      this.Code[2, 3] = "aø";
      this.Code[3, 3] = "aû";
      this.Code[4, 3] = "aõ";
      this.Code[5, 3] = "aï";
      this.Code[6, 3] = "aê";
      this.Code[7, 3] = "aé";
      this.Code[8, 3] = "aè";
      this.Code[9, 3] = "aú";
      this.Code[10, 3] = "aü";
      this.Code[11, 3] = "aë";
      this.Code[12, 3] = "aâ";
      this.Code[13, 3] = "aá";
      this.Code[14, 3] = "aà";
      this.Code[15, 3] = "aå";
      this.Code[16, 3] = "aã";
      this.Code[17, 3] = "aä";
      this.Code[18, 3] = "eù";
      this.Code[19, 3] = "eø";
      this.Code[20, 3] = "eû";
      this.Code[21, 3] = "eõ";
      this.Code[22, 3] = "eï";
      this.Code[23, 3] = "eâ";
      this.Code[24, 3] = "eá";
      this.Code[25, 3] = "eà";
      this.Code[26, 3] = "eå";
      this.Code[27, 3] = "eã";
      this.Code[28, 3] = "eä";
      this.Code[29, 3] = "í";
      this.Code[30, 3] = "ì";
      this.Code[31, 3] = "æ";
      this.Code[32, 3] = "ó";
      this.Code[33, 3] = "ò";
      this.Code[34, 3] = "où";
      this.Code[35, 3] = "oø";
      this.Code[36, 3] = "oû";
      this.Code[37, 3] = "oõ";
      this.Code[38, 3] = "oï";
      this.Code[39, 3] = "oâ";
      this.Code[40, 3] = "oá";
      this.Code[41, 3] = "oà";
      this.Code[42, 3] = "oå";
      this.Code[43, 3] = "oã";
      this.Code[44, 3] = "oä";
      this.Code[45, 3] = "ô";
      this.Code[46, 3] = "ôù";
      this.Code[47, 3] = "ôø";
      this.Code[48, 3] = "ôû";
      this.Code[49, 3] = "ôõ";
      this.Code[50, 3] = "ôï";
      this.Code[51, 3] = "uù";
      this.Code[52, 3] = "uø";
      this.Code[53, 3] = "uû";
      this.Code[54, 3] = "uõ";
      this.Code[55, 3] = "uï";
      this.Code[56, 3] = "ö";
      this.Code[57, 3] = "öù";
      this.Code[58, 3] = "öø";
      this.Code[59, 3] = "öû";
      this.Code[60, 3] = "öõ";
      this.Code[61, 3] = "öï";
      this.Code[62, 3] = "yù";
      this.Code[63, 3] = "yø";
      this.Code[64, 3] = "yû";
      this.Code[65, 3] = "yõ";
      this.Code[66, 3] = "î";
      this.Code[67, 3] = "ñ";
      this.Code[68, 3] = "AÙ";
      this.Code[69, 3] = "AØ";
      this.Code[70, 3] = "AÛ";
      this.Code[71, 3] = "AÕ";
      this.Code[72, 3] = "AÏ";
      this.Code[73, 3] = "AÊ";
      this.Code[74, 3] = "AÉ";
      this.Code[75, 3] = "AÈ";
      this.Code[76, 3] = "AÚ";
      this.Code[77, 3] = "AÜ";
      this.Code[78, 3] = "AË";
      this.Code[79, 3] = "AÂ";
      this.Code[80, 3] = "AÁ";
      this.Code[81, 3] = "AÀ";
      this.Code[82, 3] = "AÅ";
      this.Code[83, 3] = "AÃ";
      this.Code[84, 3] = "AÄ";
      this.Code[85, 3] = "EÙ";
      this.Code[86, 3] = "EØ";
      this.Code[87, 3] = "EÛ";
      this.Code[88, 3] = "EÕ";
      this.Code[89, 3] = "EÏ";
      this.Code[90, 3] = "EÂ";
      this.Code[91, 3] = "EÁ";
      this.Code[92, 3] = "EÀ";
      this.Code[93, 3] = "EÅ";
      this.Code[94, 3] = "EÃ";
      this.Code[95, 3] = "EÄ";
      this.Code[96, 3] = "Í";
      this.Code[97, 3] = "Ì";
      this.Code[98, 3] = "Æ";
      this.Code[99, 3] = "Ó";
      this.Code[100, 3] = "Ò";
      this.Code[101, 3] = "OÙ";
      this.Code[102, 3] = "OØ";
      this.Code[103, 3] = "OÛ";
      this.Code[104, 3] = "OÕ";
      this.Code[105, 3] = "OÏ";
      this.Code[106, 3] = "OÂ";
      this.Code[107, 3] = "OÁ";
      this.Code[108, 3] = "OÀ";
      this.Code[109, 3] = "OÅ";
      this.Code[110, 3] = "OÃ";
      this.Code[111, 3] = "OÄ";
      this.Code[112, 3] = "Ô";
      this.Code[113, 3] = "ÔÙ";
      this.Code[114, 3] = "ÔØ";
      this.Code[115, 3] = "ÔÛ";
      this.Code[116, 3] = "ÔÕ";
      this.Code[117, 3] = "ÔÏ";
      this.Code[118, 3] = "UÙ";
      this.Code[119, 3] = "UØ";
      this.Code[120, 3] = "UÛ";
      this.Code[121, 3] = "UÕ";
      this.Code[122, 3] = "UÏ";
      this.Code[123, 3] = "Ö";
      this.Code[124, 3] = "ÖÙ";
      this.Code[125, 3] = "ÖØ";
      this.Code[126, 3] = "ÖÛ";
      this.Code[(int) sbyte.MaxValue, 3] = "ÖÕ";
      this.Code[128, 3] = "ÖÏ";
      this.Code[129, 3] = "YÙ";
      this.Code[130, 3] = "YØ";
      this.Code[131, 3] = "YÛ";
      this.Code[132, 3] = "YÕ";
      this.Code[133, 3] = "Î";
      this.Code[134, 3] = "Ñ";
    }

    private void MapVPS()
    {
      this.Code[1, 6] = "á";
      this.Code[2, 6] = "à";
      this.Code[3, 6] = "ä";
      this.Code[4, 6] = "ã";
      this.Code[5, 6] = "å";
      this.Code[6, 6] = "æ";
      this.Code[7, 6] = "¡";
      this.Code[8, 6] = "¢";
      this.Code[9, 6] = "£";
      this.Code[10, 6] = "¤";
      this.Code[11, 6] = "¥";
      this.Code[12, 6] = "â";
      this.Code[13, 6] = "Ã";
      this.Code[14, 6] = "À";
      this.Code[15, 6] = "Ä";
      this.Code[16, 6] = "Å";
      this.Code[17, 6] = "Æ";
      this.Code[18, 6] = "é";
      this.Code[19, 6] = "è";
      this.Code[20, 6] = "È";
      this.Code[21, 6] = "ë";
      this.Code[22, 6] = "Ë";
      this.Code[23, 6] = "ê";
      this.Code[24, 6] = "‰";
      this.Code[25, 6] = "Š";
      this.Code[26, 6] = "‹";
      this.Code[27, 6] = "Í";
      this.Code[28, 6] = "Œ";
      this.Code[29, 6] = "í";
      this.Code[30, 6] = "ì";
      this.Code[31, 6] = "Ì";
      this.Code[32, 6] = "ï";
      this.Code[33, 6] = "Î";
      this.Code[34, 6] = "ó";
      this.Code[35, 6] = "ò";
      this.Code[36, 6] = "Õ";
      this.Code[37, 6] = "õ";
      this.Code[38, 6] = "†";
      this.Code[39, 6] = "ô";
      this.Code[40, 6] = "Ó";
      this.Code[41, 6] = "Ò";
      this.Code[42, 6] = "°";
      this.Code[43, 6] = "‡";
      this.Code[44, 6] = "¶";
      this.Code[45, 6] = "Ö";
      this.Code[46, 6] = "§";
      this.Code[47, 6] = "©";
      this.Code[48, 6] = "ª";
      this.Code[49, 6] = "«";
      this.Code[50, 6] = "®";
      this.Code[51, 6] = "ú";
      this.Code[52, 6] = "ù";
      this.Code[53, 6] = "û";
      this.Code[54, 6] = "Û";
      this.Code[55, 6] = "ø";
      this.Code[56, 6] = "Ü";
      this.Code[57, 6] = "Ù";
      this.Code[58, 6] = "Ø";
      this.Code[59, 6] = "º";
      this.Code[60, 6] = "»";
      this.Code[61, 6] = "¿";
      this.Code[62, 6] = "š";
      this.Code[63, 6] = "ÿ";
      this.Code[64, 6] = "›";
      this.Code[65, 6] = "Ï";
      this.Code[66, 6] = "œ";
      this.Code[67, 6] = "Ç";
      this.Code[68, 6] = "Á";
      this.Code[69, 6] = "€";
      this.Code[70, 6] = "\u0081";
      this.Code[71, 6] = "‚";
      this.Code[72, 6] = "å";
      this.Code[73, 6] = "ˆ";
      this.Code[74, 6] = "\u008D";
      this.Code[75, 6] = "Ž";
      this.Code[76, 6] = "\u008F";
      this.Code[77, 6] = "ð";
      this.Code[78, 6] = "¥";
      this.Code[79, 6] = "Â";
      this.Code[80, 6] = "ƒ";
      this.Code[81, 6] = "„";
      this.Code[82, 6] = "…";
      this.Code[83, 6] = "Å";
      this.Code[84, 6] = "Æ";
      this.Code[85, 6] = "É";
      this.Code[86, 6] = "×";
      this.Code[87, 6] = "Þ";
      this.Code[88, 6] = "þ";
      this.Code[89, 6] = "Ë";
      this.Code[90, 6] = "Ê";
      this.Code[91, 6] = "\u0090";
      this.Code[92, 6] = "“";
      this.Code[93, 6] = "”";
      this.Code[94, 6] = "•";
      this.Code[95, 6] = "Œ";
      this.Code[96, 6] = "´";
      this.Code[97, 6] = "µ";
      this.Code[98, 6] = "·";
      this.Code[99, 6] = "¸";
      this.Code[100, 6] = "Î";
      this.Code[101, 6] = "\u00B9";
      this.Code[102, 6] = "\u00BC";
      this.Code[103, 6] = "\u00BD";
      this.Code[104, 6] = "\u00BE";
      this.Code[105, 6] = "†";
      this.Code[106, 6] = "Ô";
      this.Code[107, 6] = "–";
      this.Code[108, 6] = "—";
      this.Code[109, 6] = "˜";
      this.Code[110, 6] = "™";
      this.Code[111, 6] = "¶";
      this.Code[112, 6] = "÷";
      this.Code[113, 6] = "\u009D";
      this.Code[114, 6] = "ž";
      this.Code[115, 6] = "Ÿ";
      this.Code[116, 6] = "¦";
      this.Code[117, 6] = "®";
      this.Code[118, 6] = "Ú";
      this.Code[119, 6] = "¨";
      this.Code[120, 6] = "Ñ";
      this.Code[121, 6] = "¬";
      this.Code[122, 6] = "ø";
      this.Code[123, 6] = "Ð";
      this.Code[124, 6] = "\u00AD";
      this.Code[125, 6] = "¯";
      this.Code[126, 6] = "±";
      this.Code[(int) sbyte.MaxValue, 6] = "»";
      this.Code[128, 6] = "¿";
      this.Code[129, 6] = "Ý";
      this.Code[130, 6] = "\u00B2";
      this.Code[131, 6] = "ý";
      this.Code[132, 6] = "\u00B3";
      this.Code[133, 6] = "œ";
      this.Code[134, 6] = "ñ";
    }

    public FontCase CharCase
    {
      get => this.m_CharCase;
      set => this.m_CharCase = value;
    }
  }
}
