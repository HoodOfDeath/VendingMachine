using System;
using System.Collections.Generic;

namespace VendingMachineLib
{
    public sealed class VendingMachine
    {
        private Dictionary<int, VendingItem> _catalogue = new Dictionary<int, VendingItem>();

        private ILogger _logger;

        private readonly IDeliverySystem _deliverySystem;

        public VendingMachine(IDeliverySystem deliverySystem)
        {
            _deliverySystem = deliverySystem;
        }

        public void SetCatalogue(ICollection<VendingItem> catalogue)
        {
            Dictionary<int, VendingItem> newCatalogue = new Dictionary<int, VendingItem>(catalogue.Count);
            
            foreach (var item in catalogue)
            {
                if (newCatalogue.ContainsKey(item.Id.Value))
                {
                    if (newCatalogue[item.Id.Value].Equals(item))
                    {
                        LogWarning($"Catalogue already contains same Item {item}");
                    }
                    else
                    {
                        string message = $"New catalogue contains different items with same ID {item.Id.Value}";
                        LogError(message);
                        throw new ArgumentException(message);
                    }
                }
                else
                {
                    newCatalogue.Add(item.Id.Value, item);
                }
            }
            
            _catalogue = newCatalogue;
            
            Log("Catalogue have successfully changed. New catalogue is:");
            foreach (var item in _catalogue)
            {
                Log($"Item: {item.Value}");
            }
        }

        public ICollection<VendingItem> GetCatalogue()
        {
            return new List<VendingItem>(_catalogue.Values);
        }

        public void AddItemToCatalogue(VendingItem item)
        {
            if (_catalogue.ContainsKey(item.Id.Value))
            {
                if (_catalogue[item.Id.Value].Equals(item))
                {
                    LogWarning($"Catalogue already contains item ID {item.Id.Value} Item {item}");
                }
                else
                {
                    string message = $"Catalogue contains different items with same ID {item.Id.Value}";
                    LogError(message);
                    throw new ArgumentException(message);
                }
            }
            else
            {
                Log($"Added item {item} with ID {item.Id.Value}");
                _catalogue.Add(item.Id.Value, item);
            }
        }

        public void BuyItem(VendingItem item, IPaymentSystem paymentSystem)
        {
            if (!_catalogue.ContainsValue(item))
            {
                string message = $"Catalogue does not contain item {item}. Aborting.";
                LogError(message);
                throw new Exception(message);
            }
            
            if (!_deliverySystem.IsItemPresent(item.Id.Value))
            {
                string message = $"Item with ID {item.Id.Value} is not present. Aborting.";
                LogError(message);
                throw new Exception(message);
            }

            if (!paymentSystem.TryWithholdPayment(item.Price))
            {
                string message = "Payment failed or was canceled by user. Aborting.";
                LogError(message);
                throw new Exception(message);
            }

            if (!_deliverySystem.TryDeliverItem(item.Id.Value))
            {
                string message = "Error in delivery system. Aborting.";
                LogError(message);
                paymentSystem.RevokeTransaction();

                throw new Exception(message);
            }

            paymentSystem.CompleteTransaction();
        }
        
        #region Logging

        public void SetLogger(ILogger newLogger)
        {
            _logger = newLogger;
            
            Log($"Added logger {newLogger}");
        }

        private void Log(string message)
        {
            _logger?.Log(message);
        }

        private void LogWarning(string message)
        {
            _logger?.LogWarning(message);
        }

        private void LogError(string message)
        {
            _logger?.LogError(message);
        }
        
#endregion

    }
}