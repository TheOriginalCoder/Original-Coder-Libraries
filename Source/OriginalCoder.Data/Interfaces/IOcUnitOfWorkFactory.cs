//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2012, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces;

namespace OriginalCoder.Data.Interfaces
{
    /// <summary>
    /// Factory interface for creating new units of work
    /// </summary>
    [PublicAPI]
    public interface IOcUnitOfWorkFactory
    {
        IOcUnitOfWork NewUnitOfWork(string name);
        IOcUnitOfWork NewUnitOfWork(string name, IOcTextLog log);
    }

    /// <summary>
    /// Factory interface for creating new units of work of a descendant interface type
    /// </summary>
    [PublicAPI]
    public interface IOcUnitOfWorkFactory<out TWork>
        where TWork : class, IOcUnitOfWork
    {
        TWork NewUnitOfWork(string name);
        TWork NewUnitOfWork(string name, IOcTextLog log);
    }
}