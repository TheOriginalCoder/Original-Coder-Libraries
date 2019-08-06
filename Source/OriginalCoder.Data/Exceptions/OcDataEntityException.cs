using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Data.Exceptions
{
    /// <summary>
    /// Exception for errors related to Entity / Domain / Data objects.
    /// </summary>
    [PublicAPI]
    public class OcDataEntityException : OcDataException
    {
      #region Constructors 

        public OcDataEntityException(string message, [CanBeNull] Exception exception, [CanBeNull] Type entityType = null, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Entity Error" : message, exception, callerName, callerFile, callerLine)
        {
            EntityType = entityType;
            if (EntityType != null)
                PropertySet(nameof(EntityType), EntityType);

            Entity = entity;
            if (Entity != null)
                PropertySet(nameof(Entity), Entity);

            EntityKey = entityKey;
            if (EntityKey != null)
                PropertySet(nameof(EntityKey), EntityKey);
        }

        public OcDataEntityException(string message, IReadOnlyDictionary<string, object> properties, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        {
            // NOTE: The below may not be obtainable from properties because they may be local to the method executing, not the class doing the execution.
            Entity = entity;
            if (Entity != null)
                PropertySet(nameof(Entity), Entity);
            EntityKey = entityKey;
            if (EntityKey != null)
                PropertySet(nameof(EntityKey), EntityKey);
        }

        public OcDataEntityException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        {
            // NOTE: The below may not be obtainable from properties because they may be local to the method executing, not the class doing the execution.
            Entity = entity;
            if (Entity != null)
                PropertySet(nameof(Entity), Entity);
            EntityKey = entityKey;
            if (EntityKey != null)
                PropertySet(nameof(EntityKey), EntityKey);
        }

      #endregion

        public Type EntityType { get; }
        public object Entity { get; }
        public object EntityKey { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Data Entity Error");

        protected override void SummaryBuildProperties()
        {
            SummaryAddProperties(("EntityType", EntityType?.FriendlyName()), ("Entity", Entity?.GetStatusSummary() ?? Entity?.GetSummary() ?? Entity?.GetName()), ("EntityKey", EntityKey));
        }

      #endregion
    }
}