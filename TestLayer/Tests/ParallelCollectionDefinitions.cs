using Xunit;

namespace TestLayer.Tests;

// parallel execution setup
[TestCaseOrderer("Xunit.Sdk.AlphabeticalOrderer", "xunit.core")]
public static class ParallelCollections
{
    [CollectionDefinition("UC-1 Login Tests", DisableParallelization = false)]
    public class UC1LoginTestsCollection : ICollectionFixture<object> { }

    [CollectionDefinition("UC-2 Favorites Tests", DisableParallelization = false)]
    public class UC2FavoritesTestsCollection : ICollectionFixture<object> { }

    [CollectionDefinition("UC-3 Ordering Tests", DisableParallelization = false)]
    public class UC3OrderingTestsCollection : ICollectionFixture<object> { }
}
