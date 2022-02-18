using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Purchaser : MonoBehaviour, IStoreListener
{
    public StorePanel storePanel;
    private static IStoreController m_StoreController;                                                                    // Reference to the Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider;  

    public void Init()
    {
        //如果我们尚未设置Unity购买参考
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(AdsConfigure.ProductID_Candy, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Diamond1, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Diamond2, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Diamond3, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Auto, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Income, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Attack, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_Bank, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_VIP, ProductType.NonConsumable);
        builder.AddProduct(AdsConfigure.ProductID_task, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    public void productId()
    {

    }


    public void BuyProductID(string productId)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip4"]);
            storePanel.HideMask();
            return;
        }
        // If the stores throw an unexpected exception, use try..catch to protect my logic here.
        try
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    storePanel.HideMask();
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                storePanel.HideMask();
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        // Complete the unexpected exception handling ...
        catch (Exception e)
        {
            // ... by reporting any unexpected exception for later diagnosis.
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }
    //bool isHide;
    //恢复购买
    // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
    public void RestorePurchases()
    {
        //Debug.Log("初始恢复购买");
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameManager.Instance.CloneTip(ExcelTool.lang["tip4"]);
            storePanel.HideMask();
            return;
        }
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            storePanel.HideMask();
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                if(!result)
                {
                    storePanel.HideMask();
                }
                // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            storePanel.HideMask();
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        //Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Candy, StringComparison.Ordinal))
        {
            storePanel.HideMask();
            if (UIManager.Instance)
            {
                UIManager.Instance.SetGold(200000);
            }
            else if (ChoiceControl.Instance)
            {
                ChoiceControl.Instance.SetGold(200000);
            }
            GameManager.Instance.ClonePrompt(200000, 0);
        } 
        else if(String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Diamond1, StringComparison.Ordinal))
        {
            storePanel.HideMask();
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(200);
            }
            else if(ChoiceControl.Instance)
            {
                ChoiceControl.Instance.SetDiamond(200);
            }
            GameManager.Instance.ClonePrompt(200, 1);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Diamond2, StringComparison.Ordinal))
        {
            storePanel.HideMask();
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(1200);
            }
            else if (ChoiceControl.Instance)
            {
                ChoiceControl.Instance.SetDiamond(1200);
            }
            GameManager.Instance.ClonePrompt(1200, 1);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Diamond3, StringComparison.Ordinal))
        {
            storePanel.HideMask();
            if (UIManager.Instance)
            {
                UIManager.Instance.SetStar(3000);
            }
            else if (ChoiceControl.Instance)
            {
                ChoiceControl.Instance.SetDiamond(3000);
            }
            GameManager.Instance.ClonePrompt(3000, 1);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Auto, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Auto");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Income, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Income");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Attack, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Attack");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_Bank, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Bank");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_VIP, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Vip");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, AdsConfigure.ProductID_task, StringComparison.Ordinal))
        {
            storePanel.HideBtn("Task");
        }
        // Or ... a subscription product has been purchased by this user.
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        storePanel.HideMask();
        GameManager.Instance.CloneTip(ExcelTool.lang["tip8"]);
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}