namespace SimpleSettings
{
    internal static class DataConsts
    {
        /// <summary>
        ///     The Name for the created Database.
        /// </summary>
        internal const string DatabaseName = "SimpleSettings";

        /// <summary>
        ///     The Connection String for the EntityFramework. a
        /// </summary>
        internal const string ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;" + // <- LocalDB
            "Initial Catalog= " + DatabaseName + ";" +
            "Integrated Security=True;" +
            "Connect Timeout=30;" +
            "Encrypt=False;" +
            "TrustServerCertificate=True;" +
            "ApplicationIntent=ReadWrite;" +
            "MultiSubnetFailover=False;" +
            "MultipleActiveResultSets = True;";
    }
}