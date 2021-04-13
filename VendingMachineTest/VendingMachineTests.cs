using System;
using System.Linq;
using NUnit.Framework;
using VendingMachineLib;

namespace VendingMachineTest
{
    public class VendingMachineTests
    {
        [Test]
        public void Catalogue_SetCatalogue_CatalogueChanged()
        {
            VendingMachine vendingMachine = new VendingMachine(new DeliverySystemSubstitute());

            var stateBeforeInit = vendingMachine.GetCatalogue();

            VendingItem[] catalogue = new VendingItem[]
            {
                new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1),
                new VendingItem(new VendingItemId(1), ItemType.Food, "Bread", 1),
                new VendingItem(new VendingItemId(2), ItemType.Weapon, "CheyTac M200", 12500)
            };
            
            vendingMachine.SetCatalogue(catalogue);
            
            var stateAfterInit = vendingMachine.GetCatalogue();
            VendingItem[] receivedCatalogue = stateAfterInit.ToArray();
            
            Assert.AreNotEqual(stateBeforeInit, stateAfterInit);
            Assert.AreEqual(catalogue, receivedCatalogue);
        }

        [Test]
        public void Catalogue_AddItem_CatalogueChanged()
        {
            VendingMachine vendingMachine = new VendingMachine(new DeliverySystemSubstitute());
            
            var stateBeforeInit = vendingMachine.GetCatalogue();

            VendingItem item = new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1);

            vendingMachine.AddItemToCatalogue(item);
            
            var stateAfterInit = vendingMachine.GetCatalogue();
            VendingItem[] receivedCatalogue = stateAfterInit.ToArray();
            
            Assert.AreNotEqual(stateBeforeInit, stateAfterInit);
            Assert.Contains(item, receivedCatalogue);
        }

        [Test]
        public void Catalogue_SetCatalogueWithNonUniqueId_ThrowsException()
        {
            VendingMachine vendingMachine = new VendingMachine(new DeliverySystemSubstitute());

            VendingItem[] catalogue = new VendingItem[]
            {
                new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1),
                new VendingItem(new VendingItemId(0), ItemType.Food, "Bread", 1)
            };
            
            Assert.Catch(() => vendingMachine.SetCatalogue(catalogue));
        }

        [Test]
        public void Buy_DenialOfPaymentSystem_ThrowsException()
        {
            DeliverySystemSubstitute deliverySystem = new DeliverySystemSubstitute();
            deliverySystem.ResultOfIsItemPresent = true;
            deliverySystem.ResultOfTryDeliverItem = true;
            
            VendingMachine vendingMachine = new VendingMachine(deliverySystem);

            VendingItem item = new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1);
            
            vendingMachine.AddItemToCatalogue(item);
            
            PaymentSystemSubstitute paymentSystem = new PaymentSystemSubstitute();
            paymentSystem.ResultOfWithholdOfPayment = false;
            
            Assert.Catch(() => vendingMachine.BuyItem(item, paymentSystem));
        }

        [Test]
        public void Buy_ControlPassedToPaymentSystem()
        {
            DeliverySystemSubstitute deliverySystem = new DeliverySystemSubstitute();
            deliverySystem.ResultOfIsItemPresent = true;
            deliverySystem.ResultOfTryDeliverItem = true;
            
            VendingMachine vendingMachine = new VendingMachine(deliverySystem);

            VendingItem item = new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1);
            
            vendingMachine.AddItemToCatalogue(item);
            
            PaymentSystemSubstitute paymentSystem = new PaymentSystemSubstitute();
            paymentSystem.ResultOfWithholdOfPayment = true;

            bool controlPassed;
            try
            {
                vendingMachine.BuyItem(item, paymentSystem);
                
                controlPassed = paymentSystem.ControlPassedTryWithholdPayment && paymentSystem.ControlPassedCompleteTransaction;
            }
            catch (Exception e)
            {
                controlPassed = paymentSystem.ControlPassedTryWithholdPayment && paymentSystem.ControlPassedRevokeTransaction;
                
                Assert.IsTrue(controlPassed);
            }
            Assert.IsTrue(controlPassed);
        }

        [Test]
        public void Buy_ControlPassedToDeliverySystem()
        {
            DeliverySystemSubstitute deliverySystem = new DeliverySystemSubstitute();
            deliverySystem.ResultOfIsItemPresent = true;
            deliverySystem.ResultOfTryDeliverItem = true;
            
            VendingMachine vendingMachine = new VendingMachine(deliverySystem);

            VendingItem item = new VendingItem(new VendingItemId(0), ItemType.Drink, "Water", 1);
            
            vendingMachine.AddItemToCatalogue(item);

            PaymentSystemSubstitute paymentSystem = new PaymentSystemSubstitute();
            paymentSystem.ResultOfWithholdOfPayment = true;

            vendingMachine.BuyItem(item, paymentSystem);
            
            bool controlPassed = deliverySystem.ControlPassedIsItemPresent && deliverySystem.ControlPassedTryDeliverItem;
            
            Assert.IsTrue(controlPassed);
        }

        [Test]
        public void SetLogger_ControlPassedToLogger()
        {
            VendingMachine vendingMachine = new VendingMachine(new DeliverySystemSubstitute());
            
            LoggerSubstitute logger = new LoggerSubstitute();
            
            vendingMachine.SetLogger(logger);
            
            Assert.IsTrue(logger.ControlPassed);
        }
    }
}