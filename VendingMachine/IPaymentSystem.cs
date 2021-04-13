namespace VendingMachineLib
{
    public interface IPaymentSystem
    {
        bool TryWithholdPayment(float price);
        
        void CompleteTransaction();
        
        void RevokeTransaction();
    }
}