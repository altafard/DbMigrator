using System;

namespace AltaDigital.DbMigrator.Exceptions
{
    /// <summary>
    /// Exception throws if something wrong happens with migration context.
    /// </summary>
    public class MigrationContextException : Exception
    {
        private const string Error = "Something wrong happened with migration context.";

        /// <summary>
        /// Default ctor.
        /// </summary>
        public MigrationContextException() : this(Error) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        public MigrationContextException(string errorMessage) : base(errorMessage) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        public MigrationContextException(Exception innerException) : this(Error, innerException) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public MigrationContextException(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    }
}
