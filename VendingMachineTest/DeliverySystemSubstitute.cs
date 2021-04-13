using VendingMachineLib;

namespace VendingMachineTest
{
    public class DeliverySystemSubstitute : IDeliverySystem
    {
        public bool ResultOfIsItemPresent { get; set; }
        
        public bool ResultOfTryDeliverItem { get; set; }
        
        public bool ControlPassedIsItemPresent { get; private set; }
        
        public bool ControlPassedTryDeliverItem { get; private set; }
        
        public bool IsItemPresent(int itemID)
        {
            ControlPassedIsItemPresent = true;
            
            return ResultOfIsItemPresent;
        }

        public bool TryDeliverItem(int itemID)
        {
            ControlPassedTryDeliverItem = true;
            
            return ResultOfTryDeliverItem;
        }
    }
}