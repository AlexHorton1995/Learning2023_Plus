using CodeBlackFinance;
using CBAccountIntake;
using CBAccountIntake.Models;

namespace CodeBlackFinanceTests
{
    [TestClass]
    public class UnitTest1
    {
        CBVerification? MockVerify { get; set; }
        CodeBlackTables? MockTables { get; set; }
        FileData? MockData { get; set; }
        

        [TestInitialize]
        public void TestInitialize()
        {
            MockData = new FileData();
            MockVerify = new CBVerification();
            MockTables = new CodeBlackTables();
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

        [TestMethod]
        public void TestCreateDataTable()
        {
            //Arrange
            int cols = 10;

            //Act
            var actual = MockTables?.CreateTempDataTable();

            //Assert
            Assert.AreEqual(cols, actual?.Columns.Count);
        }

        [TestMethod]
        public void TestPopulateDataTable()
        {
            //Arrange
            MockData.LastName = "Me";
            MockData.FirstName = "You";
            MockData.ExpDate = DateTime.Now;
            MockData.State = "NE";
            MockData.Email = "email";   
            MockData.AccountNumber = 1;
            MockData.Address1 = "address1";
            MockData.Address2 = "12345";    
            MockData.CardID = 123;
            MockData.City = "SLC";

            var actual = MockTables?.CreateTempDataTable();

            //Act
            MockTables?.PopulateDataTable(actual, MockData);

            //Assert
            Assert.AreEqual(actual?.Rows.Count, 1);

        }
    }
}