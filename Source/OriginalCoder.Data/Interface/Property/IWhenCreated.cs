﻿//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;

namespace OriginalCoder.Data.Interface.Property
{
    /// <summary>
    /// Standard interface for obtaining the date and time an object was created.
    /// </summary>
    public interface IWhenCreated
    {
        DateTime WhenCreated { get; }
    }
}