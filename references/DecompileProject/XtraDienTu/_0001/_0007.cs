﻿// Decompiled with JetBrains decompiler
// Type: .
// Assembly: XtraDienTu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F0E94B6-F573-4BC5-92AB-CE54C0CE94F8
// Assembly location: C:\Project\HH.CanOTo\references\CAN TRI VIET\ViNaCaFe\ViNaCaFe\Update VINA CAPHE 2014-11-24\XtraDienTu.exe

using System;

namespace \u0001
{
  internal class \u0007 : EventArgs
  {
    private Exception \u0001;

    public Exception FatalException => this.\u0001;

    internal \u0007(Exception fatalException) => this.\u0001 = fatalException;
  }
}
