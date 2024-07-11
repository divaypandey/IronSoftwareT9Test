using T9Decoder.Services;

namespace T9CoreTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("33#", "E")]
        [DataRow("227*#", "B")]
        [DataRow("4433555 555666#", "HELLO")]
        [DataRow("8 88777444666*664#", "TURING")]
        [DataRow("4433555 55566608 88777444666*664#", "HELLO TURING")] //0 should add space between words, multiple spaces trimmed
        [DataRow("222#", "C")]
        [DataRow("222222#", "C")]
        [DataRow("2222222#", "A")]
        [DataRow("2222222*#", "")]
        [DataRow("99999999#", "Z")]
        [DataRow("999999999#", "W")]
        [DataRow("999999999*#", "")]
        [DataRow("2#", "A")]
        [DataRow("#", "")]
        [DataRow("***#", "")]
        [DataRow("222#*99999#", "C")] //input is considered finished after 1st # is encountered
        [DataRow("2*#", "")]
        [DataRow("222*#", "")]
        [DataRow("222222*#", "")]
        [DataRow("222222*1#", "&")]
        public void TestMethod(string input, string expected)
        {
            string output = T9TextService.T9ToString(input);
            Assert.AreEqual(expected, output, true);
        }
    }
}