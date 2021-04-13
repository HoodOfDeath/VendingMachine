using VendingMachineLib;

namespace VendingMachineTest
{
    public class PaymentSystemSubstitute : IPaymentSystem
    {
        public bool ResultOfWithholdOfPayment { get; set; }
        
        public bool ControlPassedTryWithholdPayment { get; private set; }
        
        public bool ControlPassedCompleteTransaction { get; private set; }
        
        public bool ControlPassedRevokeTransaction { get; private set; }
        
        public bool TryWithholdPayment(float price)
        {
            ControlPassedTryWithholdPayment = true;
            return ResultOfWithholdOfPayment;
        }

        public void CompleteTransaction()
        {
            ControlPassedCompleteTransaction = true;
        }

        public void RevokeTransaction()
        {
            ControlPassedRevokeTransaction = true;
        }
    }
}