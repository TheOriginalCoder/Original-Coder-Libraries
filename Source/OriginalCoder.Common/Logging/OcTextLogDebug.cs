//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Diagnostics;

namespace OriginalCoder.Common.Logging
{
    public class OcTextLogDebug : OcTextLogBase
    {
        /// <inheritdoc />
        protected override void DoWriteLine(string line)
        {
            Debug.WriteLine(line ?? "");
        }

        /// <inheritdoc />
        protected override void DoFlush()
        {
            Debug.Flush();
        }
    }
}