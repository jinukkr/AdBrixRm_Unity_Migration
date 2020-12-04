using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AdBrixRmAOS;


//AdBrixRm.cs (브릿지 클래스)들의 함수를 호출하여 사용합니다.
//아래의 로직은 기본적 샘플이며, 실 개발환경에 맞추어 해당하는 값으로 변경하여 사용하시면 됩니다
public class AdBrixRmSample_AOS : MonoBehaviour {


    void Awake(){
        AdBrixRm.InitPlugin();
#if UNITY_ANDROID
        AdBrixRm.didReceiveDeferredDeeplink += HandleDidReceiveDeferredDeeplink;
        AdBrixRm.didReceiveDeeplink += HandleDidReceiveDeeplink;

#endif
}

	void OnDisable() {
        // deep link
#if UNITY_ANDROID
        AdBrixRm.didReceiveDeferredDeeplink -= HandleDidReceiveDeferredDeeplink;
        AdBrixRm.didReceiveDeeplink -= HandleDidReceiveDeeplink;
#endif
}


    void Start(){
#if UNITY_ANDROID         
        AdBrixRm.SetAdBrixDeferredDeeplinkDelegate();
        AdBrixRm.SetCallbackHandler("AOS-SampleObject");
        AdBrixRm.setLogLevel(AdBrixRm.AdBrixLogLevel.ERROR);
        AdBrixRm.setEventUploadCountInterval(AdBrixRm.AdBrixEventUploadCountInterval.NORMAL);
        AdBrixRm.setEventUploadTimeInterval(AdBrixRm.AdBrixEventUploadTimeInterval.NORMAL);

#endif
    }

	void Update () {
		#if UNITY_ANDROID

		#endif
	}

	void OnGUI(){
#if UNITY_ANDROID
		Screen.SetResolution( 1080, 1920, true ); //420dpi, Nexus 5X

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.normal.textColor = Color.white;
		labelStyle.wordWrap = true;
		labelStyle.fontSize = 40;

		float buttonWidth = 270;
		float buttonHeight = 100;
		int lowCnt = 0;
		int calCnt = 0;

		//Button style
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
		myButtonStyle.fontSize = 33;

		// Quit app on BACK key.
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }


		////////////////////////////////// USING OF FUNCTION SAMPLE START ////////////////////////////////
		GUI.Label(new Rect(10, 20, 800, 20), "AdBrix Remaster Sample App", labelStyle);

		//1. custom event
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "event", myButtonStyle))	{

			//custom event - just name
			AdBrixRm.events ("unityEvent");

			//cystom event - event name, event timestamp
			DateTime dt = DateTime.UtcNow;


			AdBrixRm.events("unityEvent", Convert.ToInt64(DateTime.UtcNow.Millisecond));
		
		}
		lowCnt++;
		calCnt++;

		//2. custom event with dictionary
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "event sub", myButtonStyle))	{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict.Add("detailInfo", "success");

			//event - event name, event's detail dictionary
			AdBrixRm.events ("unityEventSub", dict);

			//event - event name, event's detail dictionary, event timestamp
			AdBrixRm.events("unityEventSub", dict, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;

		//3. set Age
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setAge-20", myButtonStyle))	{
			//set age - 1~99
			AdBrixRm.setAge (20);
		}
		lowCnt++;
		calCnt++;

		//4. set Gender
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setGender-Male", myButtonStyle)) {
			//set Gender (AdBrixRm.Gender)
			AdBrixRm.setGender (AdBrixRm.Gender.FEMALE);
		}
		lowCnt++;
		calCnt++;

		//5. login event
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "Login-igaworks", myButtonStyle)) {
            //login - userid
            AdBrixRm.login ("igaworks");

            ////login - userid, event timestamp
            //DateTime dt = DateTime.UtcNow;
            AdBrixRm.login ("igaworks", Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//6. set userproperties
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "setUserProp", myButtonStyle))		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict.Add("nickname", "adbrixRM");

			//set user properties - dictionary
			AdBrixRm.setUserProperties (dict);
		}
		lowCnt++;
		calCnt++;

		//7. commerce - viewhome
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ViewHome", myButtonStyle))		{

			//view home
			AdBrixRm.commerceViewHome ();

			//view home - view home event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceViewHome (Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//view home - commerce extra attributes, event timestamp
			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			AdBrixRm.commerceViewHome(commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;

		//8. commerce - category view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Category", myButtonStyle))		{

			Dictionary<string, string> extraAttrs = new Dictionary<string, string>();
			extraAttrs.Add ("Att1", "Value1");
			extraAttrs.Add ("Att2", "Value2");
			extraAttrs.Add ("Att3", "Value3");
			extraAttrs.Add ("Att4", "Value4");
			extraAttrs.Add ("Att5", "Value5");

			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRmAOS.AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (extraAttrs)
			);

			List<AdBrixRm.AdBrixRmCommerceProductModel> productList = new List<AdBrixRm.AdBrixRmCommerceProductModel>();
			productList.Add(productModel);

			//CategoryView - category, products
			AdBrixRm.commerceCategoryView (AdBrixRm.AdBrixRmCommerceProductCategoryModel.create("sale"), productList);


			//CategoryView - category, products, commerce extra attributes
			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");
			AdBrixRm.commerceCategoryView (AdBrixRm.AdBrixRmCommerceProductCategoryModel.create("sale"), productList, commerceExtraAttrs);

			//CategoryView - category, products, commerce extra attributes
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceCategoryView (AdBrixRm.AdBrixRmCommerceProductCategoryModel.create("sale"), productList, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
			AdBrixRm.commerceCategoryView (AdBrixRm.AdBrixRmCommerceProductCategoryModel.create("sale", "summer"), productList, Convert.ToInt64(DateTime.UtcNow.Millisecond));	
				
		}
		lowCnt++;
		calCnt++;

		//9. commerce - product view
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Product", myButtonStyle))		{

			Dictionary<string, string> ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");

			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			//product view - product
			AdBrixRm.commerceProductView (productModel);

			//product view - product, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceProductView (productModel, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//product view - product, commerce extra attributes
			AdBrixRm.commerceProductView (productModel, commerceExtraAttrs);

			//product view - product, commerce extra attributes, event timestamp
			AdBrixRm.commerceProductView (productModel, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//10. commerce - addToCart-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddCart-S", myButtonStyle))		{

			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			items.Add (productModel);

			//addToCart - products
			AdBrixRm.commerceAddToCart (items);

			//addToCart - product, commerce extra attributes, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceAddToCart (items, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//addToCart - product, commerce extra attributes
			AdBrixRm.commerceAddToCart (items, commerceExtraAttrs);

			//addToCart - product, commerce extra attributes, event timestamp
			AdBrixRm.commerceAddToCart (items, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//11. commerce - addToCart-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddCart-B", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("att1", "Value1");
			ExtraAttrs.Add ("att2", "Value2");
			ExtraAttrs.Add ("att3", "Value3");
			ExtraAttrs.Add ("att4", "Value4");
			ExtraAttrs.Add ("att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			//addToCart - products
			AdBrixRm.commerceAddToCart (items);

			//addToCart - products, event timestamp
			DateTime dt = DateTime.UtcNow;
			//AdBrixRm.commerceAddToCart (items, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("attr1", "Value1");
			commerceExtraAttrs.Add ("attr2", "Value2");
			commerceExtraAttrs.Add ("attr3", "Value3");
			commerceExtraAttrs.Add ("attr4", "Value4");
			commerceExtraAttrs.Add ("attr5", "Value5");
			//addToCart - products, commerce extra attributes
			AdBrixRm.commerceAddToCart (items, commerceExtraAttrs);

			//addToCart - products, commerce extra attributes, event timestamp
			AdBrixRm.commerceAddToCart (items, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;

		//12. commerce - addToWish
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-AddWish-S", myButtonStyle))		{
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			//addToWishList - products
			AdBrixRm.commerceAddToWishList (productModel);

			//addToWishList - products, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceAddToWishList (productModel, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//addToWishList - products, commerce extra attributes
			AdBrixRm.commerceAddToWishList (productModel, commerceExtraAttrs);

			//addToWishList - products, commerce extra attributes, event timestamp
			AdBrixRm.commerceAddToWishList (productModel, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;



		//13. commerce - ReviewOrder-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ReviewOrder-S", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");

		
			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			items.Add (productModel);

			//ReviewOrder - order id, product, discount, delivery charge
			AdBrixRm.commerceReviewOrder("30290121", items, 1000.00, 3500.00);

			//ReviewOrder - order id, product, discount, delivery charge, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//ReviewOrder - order id, product, discount, delivery charge, commerce extra attributes
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, commerceExtraAttrs);

			//ReviewOrder - order id, product, discount, delivery charge, commerce extra attributes, event timestamp
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//14. commerce - ReviewOrder-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ReviewOrder-B", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);

			items.Add (productModel);
			items.Add (productModel2);

			//ReviewOrder - order id, products, discount, delivery charge
			AdBrixRm.commerceReviewOrder("30290121", items, 1000.00, 3500.00);

			//ReviewOrder - order id, products, discount, delivery charge, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//ReviewOrder - order id, products, discount, delivery charge, commerce extra attributes
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, commerceExtraAttrs);

			//ReviewOrder - order id, products, discount, delivery charge, commerce extra attributes, event timestamp
			AdBrixRm.commerceReviewOrder ("30290121", items, 1000.00, 3500.00, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;



		//17. commerce - Refund-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Refund-S", myButtonStyle))		{
			
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
		
			items.Add (productModel);

			//Refund - order id, product, penalty charge
			AdBrixRm.commerceRefund("30290121", items, 3500.00);

			//Refund - order id, product, penalty charge, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//Refund - order id, product, penalty charge, commerce extra attributes
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, commerceExtraAttrs);

			//Refund - order id, product, penalty charge, commerce extra attributes, event timestamp
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//18. commerce - Refund-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Refund-B", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);
			items.Add (productModel2);

			//Refund - order id, product, penalty charge
			AdBrixRm.commerceRefund("30290121", items, 3500.00);

			//Refund - order id, products, penalty charge, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//Refund - order id, products, penalty charge, commerce extra attributes
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, commerceExtraAttrs);

			//Refund - order id, products, penalty charge, commerce extra attributes, event timestamp
			AdBrixRm.commerceRefund ("30290121", items, 3500.00, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//19. commerce - Search-single,bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Search", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);
			items.Add (productModel2);

			//Refund - keyword, products
			AdBrixRm.commerceSearch("nike", items);

			//Refund - keyword, products, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceSearch ("nike", items, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//Refund - keyword, products, commerce extra attributes
			AdBrixRm.commerceSearch ("nike", items, commerceExtraAttrs);

			//Refund - keyword, products, commerce extra attributes, event timestamp
			AdBrixRm.commerceSearch ("nike", items, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;

		//20. commerce - Share
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Share", myButtonStyle))		{
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");
			ExtraAttrs.Add ("Att4", "Value4");
			ExtraAttrs.Add ("Att5", "Value5");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			//Refund - AdBrixRm.SharingChannel, product
			AdBrixRm.commerceShare(AdBrixRm.SharingChannel.KAKAOTALK, productModel);

			//Refund - AdBrixRm.SharingChannel, product, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");
			commerceExtraAttrs.Add ("Att4", "Value4");
			commerceExtraAttrs.Add ("Att5", "Value5");

			//Refund - AdBrixRm.SharingChannel, product, commerce extra attributes
			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, commerceExtraAttrs);

			//Refund - AdBrixRm.SharingChannel, product, commerce extra attributes, event timestamp
			AdBrixRm.commerceShare (AdBrixRm.SharingChannel.KAKAOTALK, productModel, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;


		//commerce - list view -single,bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-ListView", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);
			items.Add (productModel2);

			//commerceListView - keyword, products
			AdBrixRm.commerceListView(items);

			//commerceListView - keyword, products, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceListView (items, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");

			//commerceListView - keyword, products, commerce extra attributes
			AdBrixRm.commerceListView (items, commerceExtraAttrs);

			//commerceListView - keyword, products, commerce extra attributes, event timestamp
			AdBrixRm.commerceListView (items, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;

		//commerce - cart view -single,bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-CartView", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);
			items.Add (productModel2);

			//commerceCartView - products
			AdBrixRm.commerceCartView(items);

			//commerceCartView - products, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commerceCartView (items, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");

			//commerceCartView - products, commerce extra attributes
			AdBrixRm.commerceCartView (items, commerceExtraAttrs);

			//commerceCartView - products, commerce extra attributes, event timestamp
			AdBrixRm.commerceCartView (items, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
		}
		lowCnt++;
		calCnt++;


		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-PaymentAdded", myButtonStyle))		{

			//commercePaymentInfoAdded
			AdBrixRm.commercePaymentInfoAdded ();

			//commercePaymentInfoAdded event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commercePaymentInfoAdded (Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//commercePaymentInfoAdded - commerce extra attributes, event timestamp
			Dictionary<string, string> commerceExtraAttrs = new Dictionary<string, string>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");

			AdBrixRm.commercePaymentInfoAdded(commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;


		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-TutorialCompleted", myButtonStyle))		{

			bool is_skip = false;
			//gameTutorialCompleted
			AdBrixRm.gameTutorialCompleted (is_skip);

			//gameTutorialCompleted event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.gameTutorialCompleted (is_skip, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//commercePaymentInfoAdded - extra attributes, event timestamp
			Dictionary<string, string> extraAttrs = new Dictionary<string, string>();
			extraAttrs.Add ("Att1", "Value1");
			extraAttrs.Add ("Att2", "Value2");
			extraAttrs.Add ("Att3", "Value3");

			AdBrixRm.gameTutorialCompleted(is_skip, extraAttrs);

			AdBrixRm.gameTutorialCompleted(is_skip, extraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;


		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-LevelAchieved", myButtonStyle))		{

			int level = 15;
			//gameLevelAchieved
			AdBrixRm.gameLevelAchieved (level);

			//gameLevelAchieved event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.gameLevelAchieved (level, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//gameLevelAchieved -  extra attributes, event timestamp
			Dictionary<string, string> extraAttrs = new Dictionary<string, string>();
			extraAttrs.Add ("Att1", "Value1");
			extraAttrs.Add ("Att2", "Value2");
			extraAttrs.Add ("Att3", "Value3");

			AdBrixRm.gameLevelAchieved(level, extraAttrs);

			AdBrixRm.gameLevelAchieved(level, extraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-CharacterCreated", myButtonStyle))		{


			//gameCharacterCreated
			AdBrixRm.gameCharacterCreated ();

			//gameLevelAchieved event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.gameCharacterCreated (Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//gameLevelAchieved -  extra attributes, event timestamp
			Dictionary<string, string> extraAttrs = new Dictionary<string, string>();
			extraAttrs.Add ("Att1", "Value1");
			extraAttrs.Add ("Att2", "Value2");
			extraAttrs.Add ("Att3", "Value3");

			AdBrixRm.gameCharacterCreated(extraAttrs);

			AdBrixRm.gameCharacterCreated(extraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "G-StageCleared", myButtonStyle))		{

			string stageName = "1-1";
			//gameStageCleared
			AdBrixRm.gameStageCleared (stageName);

			//gameLevelAchieved event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.gameStageCleared (stageName, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			//gameLevelAchieved -  extra attributes, event timestamp
			Dictionary<string, string> extraAttrs = new Dictionary<string, string>();
			extraAttrs.Add ("Att1", "Value1");
			extraAttrs.Add ("Att2", "Value2");
			extraAttrs.Add ("Att3", "Value3");

			AdBrixRm.gameStageCleared(stageName, extraAttrs);

			AdBrixRm.gameStageCleared(stageName, extraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;



		//common - Purchase-single
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Purchase-S", myButtonStyle))		{


			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);

			//Purchase - order id, product, discount, delivery charge, AdBrixRm.PaymentMethod
			AdBrixRm.commonPurchase("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard);

			//Purchase - order id, product, discount, delivery charge, AdBrixRm.PaymentMethod, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");

			//Purchase - order id, product, discount, delivery charge, AdBrixRm.PaymentMethod, commerce extra attributes
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, commerceExtraAttrs);

			//Purchase - order id, product, discount, delivery charge, AdBrixRm.PaymentMethod, commerce extra attributes, event timestamp
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));

		}
		lowCnt++;
		calCnt++;

		//common - Purchase-bulk
		if (GUI.Button(new Rect(10 + (buttonWidth * (lowCnt%3)), 50 + (buttonHeight * (calCnt/3)), buttonWidth, buttonHeight), "C-Purchase-B", myButtonStyle))		{
			List<AdBrixRm.AdBrixRmCommerceProductModel> items = new List<AdBrixRm.AdBrixRmCommerceProductModel> ();
			Dictionary<string, string>ExtraAttrs = new Dictionary<string, string>();
			ExtraAttrs.Add ("Att1", "Value1");
			ExtraAttrs.Add ("Att2", "Value2");
			ExtraAttrs.Add ("Att3", "Value3");


			AdBrixRm.AdBrixRmCommerceProductModel productModel = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);
			AdBrixRm.AdBrixRmCommerceProductModel productModel2 = AdBrixRm.AdBrixRmCommerceProductModel.create (
				"productId01",
				"productName01",
				10000.00,
				1, 
				5000.00,
				AdBrixRm.Currency.KR_KRW,
				AdBrixRm.AdBrixRmCommerceProductCategoryModel.create ("Cate1", "Cate2", "Cate3"),
				AdBrixRm.AdBrixRmCommerceProductAttrModel.create (ExtraAttrs)
			);


			items.Add (productModel);
			items.Add (productModel2);

			//Purchase - order id, products, discount, delivery charge, AdBrixRm.PaymentMethod
			AdBrixRm.commonPurchase("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard);

			//Purchase - order id, products, discount, delivery charge, AdBrixRm.PaymentMethod, event timestamp
			DateTime dt = DateTime.UtcNow;
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, Convert.ToInt64(DateTime.UtcNow.Millisecond));


			Dictionary<string, object> commerceExtraAttrs = new Dictionary<string, object>();
			commerceExtraAttrs.Add ("Att1", "Value1");
			commerceExtraAttrs.Add ("Att2", "Value2");
			commerceExtraAttrs.Add ("Att3", "Value3");

			//Purchase - order id, products, discount, delivery charge, AdBrixRm.PaymentMethod, commerce extra attributes
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, commerceExtraAttrs);

			//Purchase - order id, products, discount, delivery charge, AdBrixRm.PaymentMethod, commerce extra attributes, event timestamp
			AdBrixRm.commonPurchase ("30290121", items, 1000.00, 3500.00, AdBrixRm.PaymentMethod.CreditCard, commerceExtraAttrs, Convert.ToInt64(DateTime.UtcNow.Millisecond));
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

    public void HandleDidReceiveDeeplink(string deepLink) {
		Debug.Log ("Sample : HandleDidReceiveDeeplink " + deepLink);
	}

    public void HandleDidReceiveDeferredDeeplink(string deferredDeeplink)
    {
        Debug.Log("Sample : HandleDidReceiveDeferredDeeplink " + deferredDeeplink);
    }

}

