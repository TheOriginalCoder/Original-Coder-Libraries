//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2014, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Logging;

namespace OriginalCoder.Common.Extensions
{
    [PublicAPI]
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns a string describing the <paramref name="exception"/>.
        /// If <paramref name="hierarchy"/> is true the entire inner expression tree is appended.
        /// </summary>
        public static string AsString(this Exception exception, bool hierarchy = true)
        {
            if (exception == null)
                return "";

            var builder = new StringBuilder();
            builder.Append($"EXCEPTION: {exception.GetType().FriendlyName()} -- {exception.Message}");

            if (hierarchy && exception.InnerException != null)
                AsStringHierarchy(builder, exception.InnerException);

            return builder.ToString();
        }

        private static void AsStringHierarchy([NotNull] StringBuilder builder, Exception exception)
        {
            Debug.Assert(builder != null);
            if (exception == null)
                return;

            builder.Append($"| {exception.GetType().FriendlyName()} -- {exception.Message}");

            if (exception is AggregateException asAggregate && asAggregate.InnerExceptions?.Count > 0)
            {
                foreach (var inner in asAggregate.InnerExceptions)
                    AsStringHierarchy(builder, inner);
            }
            else if (exception.InnerException != null)
                AsStringHierarchy(builder, exception.InnerException);
        }

        /// <summary>
        /// Write details about the exception to <see cref="Debug"/>.
        /// </summary>
        /// <param name="exception">Exception to be written.</param>
        /// <param name="properties">If true any available properties will also be written.</param>
        /// <param name="hierarchy">If true the full hierarchy of inner exceptions will be written.</param>
        /// <param name="stackTrace">If true and a stack trace is present it will also be written.</param>
        public static void DebugWrite([NotNull] this Exception exception, bool properties = true, bool hierarchy = true, bool stackTrace = false)
            => WriteToLog(exception, OcLog.Debug, properties, hierarchy, stackTrace);

        /// <summary>
        /// Write details about the exception to <paramref name="log"/>.
        /// </summary>
        /// <param name="exception">Exception to be written.</param>
        /// <param name="log">Text log output will be written to.</param>
        /// <param name="properties">If true any available properties will also be written.</param>
        /// <param name="hierarchy">If true the full hierarchy of inner exceptions will be written.</param>
        /// <param name="stackTrace">If true and a stack trace is present it will also be written.</param>
        public static void WriteToLog([NotNull] this Exception exception, [NotNull] IOcTextLog log, bool properties = true, bool hierarchy = true, bool stackTrace = false)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            log.WriteLine("--------------------");
            if (exception is ISummary asSummary)
                log.WriteLine($"EXCEPTION: {exception.GetType().FriendlyName()} -- {asSummary.Summary}");
            else
                log.WriteLine($"EXCEPTION: {exception.GetType().FriendlyName()} -- {exception.Message}");

            if (properties && exception is IProperties asProperties && asProperties.Properties?.Count > 0)
            {
                foreach (var property in asProperties.Properties)
                    log.WriteValue("> " + property.Key, property.Value, 28);
            }

            if (properties && exception.Data.Count > 0)
            {
                try
                {
                    foreach (var key in exception.Data.Keys)
                    {
                        var value = exception.Data[key];
                        log.WriteValue("> " + key, value, 28);
                    }
                }
                catch (Exception)
                {
                    // Ignore unimportant errors when writing exception Data properties to text log
                }
            }

            if (hierarchy && exception.InnerException != null)
                WriteToLogHierarchy(exception.InnerException, log, properties);

            if (stackTrace && !string.IsNullOrEmpty(exception.StackTrace))
            {
                log.WriteLine("STACK TRACE:");
                using (log.Indent())
                    log.WriteLine(exception.StackTrace);
            }

            log.WriteLine("--------------------");
            log.Flush();
        }

        private static void WriteToLogHierarchy([NotNull] Exception exception, [NotNull] IOcTextLog log, bool properties)
        {
            Debug.Assert(exception != null);
            Debug.Assert(log != null);

            if (exception is ISummary asSummary)
                log.WriteLine($"INNER EXCEPTION: {exception.GetType().FriendlyName()} -- {asSummary.Summary}");
            else
                log.WriteLine($"INNER EXCEPTION: {exception.GetType().FriendlyName()} -- {exception.Message}");

            if (properties && exception is IProperties asProperties && asProperties.Properties?.Count > 0)
            {
                foreach (var property in asProperties.Properties)
                    log.WriteValue("> " + property.Key, property.Value, 28);
            }

            if (exception is AggregateException asAggregate && asAggregate.InnerExceptions?.Count > 0)
            {
                foreach (var inner in asAggregate.InnerExceptions)
                    WriteToLogHierarchy(inner, log, properties);
            }
            else if (exception.InnerException != null)
                WriteToLogHierarchy(exception.InnerException, log, properties);
        }
    }
}
