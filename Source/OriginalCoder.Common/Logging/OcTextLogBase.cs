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
using System.Diagnostics;
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces;

namespace OriginalCoder.Common.Logging
{
    public abstract class OcTextLogBase : IOcTextLog
    {
        protected OcTextLogBase()
        {
            _indentSize = 4;
        }

        protected OcTextLogBase(byte indentSize)
        {
            _indentSize = indentSize;
        }

        private string _indentText = "";
        private byte _indentLevel;
        private readonly byte _indentSize;

        private void IndentChange(bool indent)
        {
            lock (this)
            {
                var level = indent ? _indentLevel + 1 : _indentLevel - 1;
                if (level < 0)
                    _indentLevel = 0;
                else if (level > 255)
                    _indentLevel = 255;
                else
                    _indentLevel = (byte) level;
                _indentText = new string(' ', _indentLevel * _indentSize);
            }
        }

        /// <inheritdoc />
        public IDisposable Indent()
        {
            IndentChange(true);
            return new IndentDisposable(this);
        }

        /// <inheritdoc />
        public void WriteLine()
            => DoWriteLine("");

        /// <inheritdoc />
        public void WriteLine(string text)
            => DoWriteLine(text == null ? "" : _indentText + text);

        /// <inheritdoc />
        public void WriteValue([NotNull] string name, string value, byte nameLength = 20)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var text = name;
            if (text.Length > nameLength)
                text = text.Substring(nameLength);
            text = text + new string(' ', (nameLength + 2) - text.Length) + " ";

            if (value == null)
                WriteLine(text + "<<[NULL]>>");
            else if (string.IsNullOrWhiteSpace(value))
                WriteLine(text + "<<[BLANK>>]");
            else
                WriteLine(text + value);
        }

        /// <inheritdoc />
        public void WriteValue([NotNull] string name, object value, byte nameLength = 20)
            => WriteValue(name, value?.ToString(), nameLength);

        /// <inheritdoc />
        public void Flush()
            => DoFlush();

        protected abstract void DoWriteLine(string line);

        protected virtual void DoFlush()
        { }

        private class IndentDisposable : IDisposable
        {
            internal IndentDisposable(OcTextLogBase textLog)
            {
                Debug.Assert(textLog != null);
                _textLog = textLog;
            }

            private OcTextLogBase _textLog;

            /// <inheritdoc />
            public void Dispose()
            {
                _textLog?.IndentChange(false);
                _textLog = null;
            }
        }
    }
}