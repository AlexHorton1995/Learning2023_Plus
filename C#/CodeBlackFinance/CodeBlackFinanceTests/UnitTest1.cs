using CodeBlackFinance;

namespace CodeBlackFinanceTests
{
    [TestClass]
    public class UnitTest1
    {
        CBVerification? MockVerify { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            MockVerify = new CBVerification();
        }

        [TestMethod]
        public void TestValidateCard()
        {
            //Arrange
            long[] testAccountNumbers = new long[] { 5555555555554444, 5712555665254243 };

            //Act
            foreach (long testAccountNumber in testAccountNumbers)
            {
                var boolVal = MockVerify?.CardValidated(testAccountNumber);

                //Assert
                if (testAccountNumber == 5555555555554444)
                    Assert.IsTrue(boolVal);
                else
                    Assert.IsFalse(boolVal);
            }
        }
    }
}