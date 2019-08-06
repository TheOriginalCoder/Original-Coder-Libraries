//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Threading.Locking.Concrete;

namespace OriginalCoder.Common.Threading.Locking
{
    /// <summary>
    /// Singleton class that provides standard factories for instantiating new instances of the various Thread Lock interfaces.
    /// Using this would allow a software system to switch out underlying lock implementations if needed in the future with virtually no effort.
    /// </summary>
    /// <remarks>
    /// NOTE: Any static members which use these factories to initialize their value will execute before the application would have the chance to assign new factory methods.
    /// </remarks>
    [PublicAPI]
    public static class OcThreadLockFactory
    {
      #region Factory Methods

        /// <summary>
        /// Create and return a new instance of <see cref="IOcThreadLock"/>.
        /// </summary>
        public static IOcThreadLock NewWrite(string lockName)
        {
            return _newWrite(lockName);
        }

        /// <summary>
        /// Create and return a new instance of <see cref="IOcThreadLock"/>.
        /// </summary>
        public static IOcThreadLockReadWrite NewReadWrite(string lockName)
        {
            return _newReadWrite(lockName);
        }

        /// <summary>
        /// Create and return a new instance of <see cref="IOcThreadLock"/>.
        /// </summary>
        public static IOcThreadLockReadUpgrade NewReadUpgrade(string lockName)
        {
            return _newReadUpgrade(lockName);
        }

      #endregion

      #region Private

        private static Func<string, IOcThreadLock> _newWrite = lockName => new OcThreadLockMonitor(lockName);
//        private static Func<string, IThreadLockTimeout> _newWriteTimeout = lockName => new ThreadLockMutex(lockName);

        private static Func<string, IOcThreadLockReadWrite> _newReadWrite = lockName => new OcThreadLockReadWrite(lockName);
//        private static Func<string, IThreadLockReadWriteTimeout> _newReadWriteTimeout = lockName => new ThreadLockReadWrite(lockName);

        private static Func<string, IOcThreadLockReadUpgrade> _newReadUpgrade = lockName => new OcThreadLockReadWrite(lockName);
//        private static Func<string, IThreadLockReadUpgradeTimeout> _newReadUpgradeTimeout = lockName => new ThreadLockReadWrite(lockName);

      #endregion

      #region Replace Factories

        /// <summary>
        /// Replace the method being used to create and return a new instances of <see cref="IOcThreadLock"/>.
        /// If <paramref name="factoryMethod"/> is null the built-in default implementation is restored.
        /// </summary>
        public static void SetFactoryWrite(Func<string, IOcThreadLock> factoryMethod)
        {
            _newWrite = factoryMethod ?? (lockName => new OcThreadLockMonitor(lockName));
        }

        /// <summary>
        /// Replace the method being used to create and return a new instances of <see cref="IOcThreadLockReadWrite"/>.
        /// If <paramref name="factoryMethod"/> is null the built-in default implementation is restored.
        /// </summary>
        public static void SetFactoryReadWrite(Func<string, IOcThreadLockReadWrite> factoryMethod)
        {
            _newReadWrite = factoryMethod ?? (lockName => new OcThreadLockReadWrite(lockName));
        }

        /// <summary>
        /// Replace the method being used to create and return a new instances of <see cref="IOcThreadLockReadUpgrade"/>.
        /// If <paramref name="factoryMethod"/> is null the built-in default implementation is restored.
        /// </summary>
        public static void SetFactoryReadUpgrade(Func<string, IOcThreadLockReadUpgrade> factoryMethod)
        {
            _newReadUpgrade = factoryMethod ?? (lockName => new OcThreadLockReadWrite(lockName));
        }

      #endregion
    }
}
