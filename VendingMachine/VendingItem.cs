namespace VendingMachineLib
{
    public struct VendingItem
    {
        public VendingItemId Id { get; }
        
        public ItemType Type { get; }
        
        public string Name { get; }
        
        public float Price { get; private set; }
        
        public VendingItem(VendingItemId id, ItemType type, string name, float price)
        {
            Id = id;
            Type = type;
            Name = name;
            Price = price;
        }
        
        public void SetNewPrice(float newPrice)
        {
            Price = newPrice;
        }
        
        public override string ToString()
        {
            return $"{Id.Value} {Name} {Type} {Price}";
        }
    }
}