namespace VendingMachineLib
{
    public interface IDeliverySystem
    {
        bool IsItemPresent(int itemID);

        bool TryDeliverItem(int itemID);
    }
}