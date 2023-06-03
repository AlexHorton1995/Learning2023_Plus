namespace CodeBlackFinance
{
    public interface ICBVerification 
    {
        /// <summary>
        /// Validates a card number is valid usin the Mod10 Check
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>bool</returns>
        bool CardValidated(long cardNumber);
    }

    public class CBVerification : ICBVerification
    {
        /// <summary>
        /// Validates a card number is valid usin the Mod10 Check
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>bool</returns>
        public bool CardValidated(long cardNumber) 
        {
            return ValidateCard(cardNumber);
        }

        internal bool ValidateCard(long cardNumber)
        {
            try
            {
                var revCardNum = cardNumber.ToString().Reverse().ToArray();
                int totalNum = 0;

                //Pick even/odd numbers, double the numbers where needed
                for (int i = 0; i < revCardNum.Length; i++)
                {
                    if (int.TryParse(revCardNum[i].ToString(), out int gotNum) && (i % 2) == 0)
                        totalNum += gotNum;
                    else
                    {
                        var newNum = gotNum * 2;

                        if (newNum <= 9)
                            totalNum += newNum;
                        else
                            totalNum += (newNum % 10) + (newNum / 10);
                    }
                }

                return (totalNum % 10) == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}