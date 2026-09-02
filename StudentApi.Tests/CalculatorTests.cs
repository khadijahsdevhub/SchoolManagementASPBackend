namespace SchoolManagementASPBackend.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_TwoNumbers_ReturnsTheirSum()
        {
            // Arrange 
            var calculator = new Calculator();

            // Act 
            var result = calculator.Add(10, 5);

            // Assert 
            Assert.Equal(15, result);
        }

        [Fact]
        public void Subtract_TwoNumbers_ReturnsTheirDifference()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Subtract(10, 5);

            // Assert
            Assert.Equal(5, result);
        }

        //[Theory]
        //[InlineData(2, 3, 5)]
        //[InlineData(10, 5, 15)]
        //[InlineData(100, 50, 150)]
        //[InlineData(-5, 10, 5)]

        //public void Add_TwoNumbers_ReturnsTheirSum(int a, int b, int expected)
        //{
        //    // Arrange
        //    var calculator = new Calculator();

        //    // Act
        //    var result = calculator.Add(a, b);

        //    // Assert
        //    Assert.Equal(expected, result);
        //}
    }
}
