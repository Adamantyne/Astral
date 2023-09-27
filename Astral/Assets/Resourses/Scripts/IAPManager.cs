// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Purchasing;
// using TMPro;

// public class IAPManager : MonoBehaviour, IStoreListener
// {
//     public static IAPManager instance;
//     public static IStoreController storeController;
//     public static IExtensionProvider extensionProvider;

//     [SerializeField] TextMeshProUGUI product500GoldTXT;
//     [SerializeField] TextMeshProUGUI productNoAdsIdTXT;
//     [SerializeField] TextMeshProUGUI productSubscriptionIdTXT;


//     public static string product500GoldId = "gold500";
//     public static string productNoAdsId = "noAds";
//     public static string productSubscriptionId = "subscription";


//     private void Awake ()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//             Destroy (gameObject);
//     }

//     [System.Obsolete]
//     private void Start ()
//     {
//         if (!IsInitialized())
//             InitializePurchasing ();
//     }


//     public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
//     {
//         storeController = controller;
//         extensionProvider = extensions;
//     }

//     public void OnInitializeFailed (InitializationFailureReason error)
//     {
//         Debug.LogWarning ("IAP Manager OnInitializeFailed Error: " + error);

//     }

//     public void OnInitializeFailed (InitializationFailureReason error, string message)
//     {


//     }

//     public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
//     {
//         Debug.Log ("OnPurchaseFailed " + failureReason);

//     }


//     public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs purchaseEvent)
//     {
//         return PurchaseProcessingResult.Complete;
//     }

//     private bool IsInitialized ()
//     {
//         return storeController != null && extensionProvider != null;
//     }

//     [System.Obsolete]
//     public void InitializePurchasing()
//     {
//         if (IsInitialized ()) return;

//         StandardPurchasingModule _module = StandardPurchasingModule.Instance ();
//         ConfigurationBuilder _builder = ConfigurationBuilder.Instance (_module);

//         _builder.AddProduct (product500GoldId, ProductType.Consumable);
//         _builder.AddProduct (productNoAdsId, ProductType.NonConsumable);
//         _builder.AddProduct (productSubscriptionId, ProductType.Subscription);

//         UnityPurchasing.Initialize (this, _builder);

//     }

//     public void BuyItem(string _productID)
//     {
//         if(IsInitialized ())
//         {
//             Product _product = storeController.products.WithID (_productID);

//             if(_product != null && _product.availableToPurchase)
//             {
//                 storeController.InitiatePurchase (_productID);

//                 Debug.Log ("Product: " + _product.definition.id + " - Price: " + _product.metadata.localizedPriceString);
//             }
//             else
//             {
//                 Debug.Log ("Product purchase error: " + _productID + " - Product not found!");
//             }
//         }
//         else
//         {
//             Debug.Log ("Product purchase error: " + _productID + " - Product not found!");
//         }
//     }


//     public void Buy500Coins()
//     {
//         BuyItem (product500GoldId);
//     }

//     public void BuyNoAds()
//     {
//         BuyItem (productNoAdsId);
//     }

//     public void BuySubscription()
//     {
//         BuyItem (productSubscriptionId);
//     }

//     public void UpdateProductPrices()
//     {
//         if (!IsInitialized ()) return;

//         product500GoldTXT.text = storeController.products.WithID(product500GoldId).metadata.localizedPriceString;
//         productNoAdsIdTXT.text = storeController.products.WithID (productNoAdsId).metadata.localizedPriceString;
//         productSubscriptionIdTXT.text = storeController.products.WithID (productSubscriptionId).metadata.localizedPriceString;
//     }

// }

