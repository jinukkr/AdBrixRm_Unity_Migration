using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using AdBrixRmIOS;

#if UNITY_IPHONE && UNITY_IOS
using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;

#endif

//AdBrixRm.cs (브릿지 클래스)들의 함수를 호출하여 사용합니다.
//아래의 로직은 기본적 샘플이며, 실 개발환경에 맞추어 해당하는 값으로 변경하여 사용하시면 됩니다
public class AdBrixRmSample_iOS : MonoBehaviour {



	void Start () {
#if UNITY_IPHONE && UNITY_IOS

		// Set this to the name of your linked GameObject 
		AdBrixRm.SetCallbackHandler("iOS-SampleObject");			
		AdBrixRm.SetAdBrixDeeplinkDelegate();

		//IDFA initialize
		AdBrixRm.setAppleAdvertisingIdentifier(Device.advertisingIdentifier);

		//set log level
		AdBrixRm.setLogLevel(AdBrixRm.AdBrixLogLevel.ERROR);
		AdBrixRm.setEventUploadCountInterval(AdBrixRm.AdBrixEventUploadCountInterval.MIN);
		AdBrixRm.setEventUploadTimeInterval(AdBrixRm.AdBrixEventUploadTimeInterval.MIN);
	

		//app initialize
		AdBrixRm.initAdBrix("tiOfuTDHk0icyqiS9UAgiQ", "d8B12SfubUmBwIGqjDWw1Q");

    //    UnityEngine.iOS.NotificationServices.RegisterForNotifications
    //(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge
    //| UnityEngine.iOS.NotificationType.Sound, true);

#endif
    }

	void Awake() {
        // deep link
        AdBrixRm.didReceiveDeeplink += HandleDidReceiveDeeplink;
        AdBrixRm.didReceiveDeferredDeeplink += HandleDidReceiveDeferredDeeplink;
	}

	void OnDisable() {
        // deep link
        AdBrixRm.didReceiveDeeplink -= HandleDidReceiveDeeplink;
        AdBrixRm.didReceiveDeferredDeeplink -= HandleDidReceiveDeferredDeeplink;
	}

	// Update is called once per frame
	void Update () {
    }


    void OnGUI() {
#if UNITY_IPHONE && UNITY_IOS

		Screen.SetResolution( 480, 800, true );

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.normal.textColor = Color.white;
		labelStyle.wordWrap = true;
		labelStyle.fontSize = 20;

		float buttonWidth = 135;
		float buttonHeight = 50;
		int lowCnt = 0;
		int calCnt = 0;

		//Button style
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
		myButtonStyle.fontSize = 14;

		// Quit app on BACK key.
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

		GUI.Label(new Rect(10, 20, 400, 20), "AdBrix Remaster Sample App", labelStyle);

		//1. event
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "event", myButtonStyle))
		{
			
			AdBrixRm.events ("unityEvent");

			AdBrixRm.events("unityEvent", DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//2. event with dictionary
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "event sub", myButtonStyle))
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("detailInfo", "success");

			AdBrixRm.events("unityEventSub", dict);

			AdBrixRm.events("unityEventSub", dict, DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//3. set Age
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setAge-20", myButtonStyle))
		{
			AdBrixRm.setAgeWithInt (20);
		}
		lowCnt++;
		calCnt++;

		//4. set Gender
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setGender-Male", myButtonStyle))
		{
			AdBrixRm.setGenderWithAdBrixGenderType (AdBrixRm.Gender.FEMALE);
		}
		lowCnt++;
		calCnt++;

		//5. login event
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "Login-igaworks", myButtonStyle))
		{
            AdBrixRm.login("igaworks");
            AdBrixRm.login("igaworks", DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//6. set userproperties
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setUserProp", myButtonStyle))
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("nickname", "adbrixRM");

			AdBrixRm.setUserPropertiesWithDictionary (dict);
		}
		lowCnt++;
		calCnt++;

		//7. commerce - viewhome
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ViewHome", myButtonStyle))
		{
            AdBrixRm.commerceViewHome();

            AdBrixRm.commerceViewHome(DateTime.UtcNow);

            Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
            orderAttrs.Add("orderAttrKey1", "commerceExtValue1");
            orderAttrs.Add("orderAttrKey2", "commerceExtValue2");

            AdBrixRm.commerceViewHome(orderAttrs);
            AdBrixRm.commerceViewHome(orderAttrs, DateTime.UtcNow);
        }
        lowCnt++;
		calCnt++;

		//8. commerce - category view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Category", myButtonStyle))
		{

			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string> productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);


			AdBrixRm.commerceCategoryView (AdBrixRmCommerceProductCategoryModel.create("sale"), items);

			AdBrixRm.commerceCategoryView (AdBrixRmCommerceProductCategoryModel.create("sale", "summer"), items, DateTime.UtcNow);	

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceCategoryView (AdBrixRmCommerceProductCategoryModel.create("sale", "summer"), items, orderAttrs);	
			AdBrixRm.commerceCategoryView (AdBrixRmCommerceProductCategoryModel.create("sale", "summer"), items, orderAttrs, DateTime.UtcNow);	

		}
		lowCnt++;
		calCnt++;

		//9. commerce - product view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Product", myButtonStyle))
		{

			Dictionary<string, string> productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
															"productId01",
				                                            "productName01",
															10000.00,
															1, 
															5000,
				                                            USDName,
															AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
															AdBrixRmCommerceProductAttrModel.create (productAttrs)
			                                            );


			AdBrixRm.commerceProductView (productModel);

			AdBrixRm.commerceProductView (productModel, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceProductView (productModel, orderAttrs);
			AdBrixRm.commerceProductView (productModel, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//10. commerce - addToCart-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddCart-S", myButtonStyle))
		{

			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");
			productAttrs.Add ("Att4", "Value4");
			productAttrs.Add ("Att5", "Value5");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);

			AdBrixRm.commerceAddToCart(items);

			AdBrixRm.commerceAddToCart (items, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceAddToCart (items, orderAttrs);
			AdBrixRm.commerceAddToCart (items, orderAttrs, DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//11. commerce - addToCart-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddCart-B", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceAddToCart (items);

			AdBrixRm.commerceAddToCart (items, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceAddToCart (items, orderAttrs);
			AdBrixRm.commerceAddToCart (items, orderAttrs, DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//12. commerce - addToWish
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddWish-S", myButtonStyle))
		{
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRm.commerceAddToWishList (productModel);

			AdBrixRm.commerceAddToWishList (productModel, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceAddToWishList (productModel, orderAttrs);
			AdBrixRm.commerceAddToWishList (productModel, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;



		//13. commerce - ReviewOrder-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ReviewOrder-S", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
		
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00);

			AdBrixRm.commerceReviewOrder("30290121", items, 1000.00, 3500.00, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceReviewOrder("30290121", items, 1000.00, 3500.00, orderAttrs);
			AdBrixRm.commerceReviewOrder("30290121", items, 1000.00, 3500.00, orderAttrs, DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//14. commerce - ReviewOrder-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ReviewOrder-B", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");
			productAttrs.Add ("Att4", "Value4");
			productAttrs.Add ("Att5", "Value5");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00);

			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, orderAttrs);
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;



		//15. commerce - Refund-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Refund-S", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

		

			items.Add (productModel);
		

			AdBrixRm.commerceRefund ("30290121", items, 3500.00);

			AdBrixRm.commerceRefund ("30290121", items, 3500.00, DateTime.UtcNow);


			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceRefund ("30290121", items, 3500.00, orderAttrs);
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//16. commerce - Refund-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Refund-B", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");
			productAttrs.Add ("Att4", "Value4");
			productAttrs.Add ("Att5", "Value5");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);
		
			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceRefund ("30290121", items, 3500.00);

			AdBrixRm.commerceRefund ("30290121", items, 3500.00, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceRefund ("30290121", items, 3500.00, orderAttrs);
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//17. commerce - Search-single,bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Search", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"n24jsda922",
				"[나이키] 나이키NIKE WMNS AIR MAX THEA WHITE womens 599409-103",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("[나이키]운동화기획전"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"saf323dsa23d4f",
				"[나이키] 나이키NIKE WMNS AIR MAX THEA WHITE womens 599409-103",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("[나이키]운동화기획전"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceSearch(items, "나이키");

			AdBrixRm.commerceSearch(items, "나이키", DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceSearch(items, "나이키", orderAttrs);
			AdBrixRm.commerceSearch(items, "나이키", orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//18. commerce - Share
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Share", myButtonStyle))
		{
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");
			productAttrs.Add ("Att4", "Value4");
			productAttrs.Add ("Att5", "Value5");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);


			
			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel);

			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, orderAttrs);
			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, orderAttrs, DateTime.UtcNow);


		}
		lowCnt++;
		calCnt++;

		//19. commerce - Purchase-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Purchase-S", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");
			productAttrs.Add ("Att4", "Value4");
			productAttrs.Add ("Att5", "Value5");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);



			items.Add (productModel);


			AdBrixRm.commonPurchase("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD);

			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, orderAttrs);
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, orderAttrs, DateTime.UtcNow);

		}
		lowCnt++;
		calCnt++;

		//20. commerce - Purchase-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Purchase-B", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD);

			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, DateTime.UtcNow);

			Dictionary<string, object> orderAttrs = new Dictionary<string, object>();
			orderAttrs.Add ("orderAttrKey1", "Value1");
			orderAttrs.Add ("orderAttrKey2", "Value2");

			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, orderAttrs);
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CREDIT_CARD, orderAttrs, DateTime.UtcNow);
		}
		lowCnt++;
		calCnt++;

		//21. commerce - list view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ListView", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceListView (items);

		}
		lowCnt++;
		calCnt++;

		//22. commerce - cart view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-CartView", myButtonStyle))
		{
			List<AdBrixRmCommerceProductModel> items = new List<AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>productAttrs = new Dictionary<string, string>();
			productAttrs.Add ("Att1", "Value1");
			productAttrs.Add ("Att2", "Value2");
			productAttrs.Add ("Att3", "Value3");

			string USDName = AdBrixRm.AdBrixCurrencyName(AdBrixRm.Currency.KR_KRW);

			AdBrixRmCommerceProductModel productModel = AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			AdBrixRmCommerceProductModel productModel2 = AdBrixRmCommerceProductModel.create (
				"productId02",
				"productName02",
				10000.00,
				1, 
				5000.00,
				USDName,
				AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRmCommerceProductAttrModel.create (productAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			AdBrixRm.commerceCartView (items);

		}
		lowCnt++;
		calCnt++;

		//commerce - PaymentInfoAdded
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-PaymentInfoAdded", myButtonStyle))
		{
			Dictionary<string, object> paymentAttrs = new Dictionary<string, object>();
			paymentAttrs.Add ("creditcard", "kbcard");


			AdBrixRm.commercePaymentInfoAdded (paymentAttrs);

		}
		lowCnt++;
		calCnt++;

		//game - gameLevelAchieved
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-LevelAchieved", myButtonStyle))
		{
			AdBrixRm.gameLevelAchieved (15);

		}
		lowCnt++;
		calCnt++;

		//game - gameTutorialCompleted
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-LevelAchieved", myButtonStyle))
		{
			AdBrixRm.gameTutorialCompleted (false);

		}
		lowCnt++;
		calCnt++;

		//game -  gameCharacterCreated
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-CharacterCreated", myButtonStyle))
		{
			AdBrixRm.gameCharacterCreated();

		}
		lowCnt++;
		calCnt++;

		//game - gameStageCleared
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-StageCleared", myButtonStyle))
		{
			AdBrixRm.gameStageCleared("1-1");

		}
		lowCnt++;
		calCnt++;

		//gdpr
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "GDPR-ForgetMe", myButtonStyle))
		{
			AdBrixRm.gdprForgetMe();

		}
		lowCnt++;
		calCnt++;

		//common - signup
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-signup", myButtonStyle))		{
			AdBrixRm.commonSignUp(AdBrixRm.SignUpChannel.Kakao);
		}
		lowCnt++;
		calCnt++;
		//common - useCredit
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-useCredit", myButtonStyle))		{
			AdBrixRm.commonUseCredit();
		}
		lowCnt++;
		calCnt++;
		//common - appUpdate
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-appUpdate", myButtonStyle))		{
			AdBrixRm.commonAppUpdate("2.0","2.1");
		}
		lowCnt++;
		calCnt++;
		//common - invite
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-invite", myButtonStyle))		{
			AdBrixRm.commonInvite(AdBrixRm.InviteChannel.Line);
		}
		lowCnt++;
		calCnt++;

		#endif


	}

	public void HandleDidReceiveDeferredDeeplink(string deepLink) {
		Debug.Log ("AdBrixRm: HandleDidReceiveDeferredDeeplink " + deepLink);
	}
    public void HandleDidReceiveDeeplink(string deepLink)
    {
        Debug.Log("AdBrixRm: HandleDidReceiveDeeplink " + deepLink);
    }

}

