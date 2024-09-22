using LinQ.LinQ;

namespace LinQ
{
    public class LinQTest
    {
        public static List<Person> GetSampleData()
        {
            return new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 25 },
                new Person { Id = 2, Name = "Bob", Age = 30 },
                new Person { Id = 3, Name = "Charlie", Age = 35 },
                new Person { Id = 3, Name = "Balan", Age = 30 },
            };
        }
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        [Fact]
        public void First_ShouldReturnMatchingElement()
        {
            // Arrange
            var data = GetSampleData();
            // Act
            var result = data.FirstInLinQ(x => x.Age >= 30);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Bob", result.Name);
        }
        [Fact]
        public void First_ShouldReturnNull()
        {
            // Arrange
            var data = GetSampleData();
            // Act
            var result = data.FirstInLinQ(x => x.Age >= 50);
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void WhereAge_ShouldReturnList()
        {
            // Arrange
            var data = GetSampleData();
            // Act
            var result = data.WhereInLinQ(x => x.Age == 30);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public void OrderByDescending_ShouldReturnList()
        {
            // Arrange
            var data = GetSampleData();
            // Act
            var result = data.OrderByDescendingInLinQ(x => x.Age);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }
        [Fact]
        public void SelectInLinQ_ShouldElements()
        {
            // Arrange
            var data = GetSampleData();

            // Act
            var result = data.SelectInLinQ(p => p.Name);

            // Assert
            Assert.Collection(result,
                item => Assert.Equal("Alice", item),
                item => Assert.Equal("Bob", item),
                item => Assert.Equal("Charlie", item),
                item => Assert.Equal("Balan", item)
            );
        }
        [Fact]
        public void GroupByInLinQ_ShouldGroupElementsByAge()
        {
            // Arrange
            var data = GetSampleData();

            // Act
            var result = data.GroupByInLinQ(p => p.Age);

            // Assert
            var group25 = Assert.Single(result, g => g.Key == 25);
            Assert.Collection(group25,
                item => Assert.Equal("Alice", item.Name));

            var group30 = Assert.Single(result, g => g.Key == 30);
            Assert.Collection(group30,
                item => Assert.Equal("Bob", item.Name),
                item => Assert.Equal("Balan", item.Name));

            var group35 = Assert.Single(result, g => g.Key == 35);
            Assert.Collection(group35,
                item => Assert.Equal("Charlie", item.Name));
        }
    }
}