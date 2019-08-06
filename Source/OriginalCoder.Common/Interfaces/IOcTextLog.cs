//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2008, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Interfaces
{
    [PublicAPI]
    public interface IOcTextLog
    {
        IDisposable Indent();

        void WriteLine();
        void WriteLine(string text);

        void WriteValue([NotNull] string name, string value, byte nameLength = 20);
        void WriteValue([NotNull] string name, object value, byte nameLength = 20);

//        void Write(Exception ex, string text = "");

        void Flush();
  }
}