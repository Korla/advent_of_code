using NUnit.Framework;

namespace AoC2023.DayXXPartYY;

public class DayXXPartYY
{
    private int Run(IEnumerable<string> data)
    {
        return -1;
    }
      
    private class DayXXPartYYTests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"DayXXPartYY/testdata.txt");
            var sut = new DayXXPartYY();
            Assert.AreEqual(0, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"DayXXPartYY/data.txt");
            var sut = new DayXXPartYY();
            Assert.AreEqual(0, sut.Run(data));
        }
    }
}