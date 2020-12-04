
//using IgaworksUnityAOS.IgawLiveOpsPopupEventManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


//AdBrxRmSample.cs 클래스에서 본 브릿지의 static 함수를 호출하여 사용하며, 
//각 함수별로 AdBrix API를 사용하는 AdBrixRmBridge.mm 클래스 파일내의 함수들을 호출합니다.
//본 Android 브릿지 함수는 특별한 경우를 제외하고는 개발간 수정하지 않도록 유의해주십시오.
namespace AdBrixRmAOS {
	public class AdBrixRm : MonoBehaviour {
		
		public enum AdBrixLogLevel {
			NONE = 0,
			TRACE = 1,
			DEBUG = 2,
			INFO = 3,
			WARNING = 4,
			ERROR = 5
		}

		public enum AdBrixEventUploadCountInterval {
			MIN = 10,
			NORMAL = 30,
			MAX = 60
		}

		public enum AdBrixEventUploadTimeInterval {
			MIN = 30,
			NORMAL = 60,
			MAX = 120
		}

		public enum Gender {
			FEMALE = 1,
			MALE = 2
		}


		public enum Currency {
			KR_KRW,
			US_USD,
			JP_JPY,
			EU_EUR,
			UK_GBP,
			CH_CNY,
			TW_TWD,
			HK_HKD,
			ID_IDR,//Indonesia
			IN_INR,//India
			RU_RUB,//Russia
			TH_THB,//Thailand
			VN_VND,//Vietnam
			MY_MYR//Malaysia
		}

		public enum PaymentMethod {
			CreditCard ,
			BankTransfer,
			MobilePayment,
			ETC
		}

		public enum SharingChannel {
			FACEBOOK = 1,
			KAKAOTALK = 2,
			KAKAOSTORY = 3,
			LINE = 4,
			WHATSAPP = 5,
			QQ = 6,
			WECHAT = 7,
			SMS = 8,
			EMAIL = 9,
			COPYURL = 10,
			ETC = 11
		}

		public enum SignUpChannel {
			Kakao = 1,
			Naver = 2,
			Line = 3,
			Google = 4,
			Facebook = 5,
			Twitter = 6,
			WhatsApp = 7,
			QQ = 8,
			WeChat = 9,
			UserId = 10,
			ETC = 11,
			SkTid = 12,
			AppleId = 13
		}

		public enum InviteChannel {
			Kakao = 1,
			Naver = 2,
			Line = 3,
			Google = 4,
			Facebook = 5,
			Twitter = 6,
			WhatsApp = 7,
			QQ = 8,
			WeChat = 9,
			ETC = 10
		}


		private static int numOfObject = 0;
        private static bool initilizeSdkSucceed = false;
		private int currentObjectIndex;

		void Awake() {
			#if UNITY_EDITOR && UNITY_ANDROID
			currentObjectIndex = numOfObject;
			numOfObject++;
			Debug.Log("igaw awake, " + gameObject.name + ", index is " + currentObjectIndex);

			if (currentObjectIndex == 0)
				DontDestroyOnLoad(gameObject);
			else
				Destroy(gameObject);
			#endif
		}



		static AdBrixRm _igaworksUnityPluginAosInstance = null;
		static AndroidJavaClass _igaworksUnityPluginAosClass = null;

#if UNITY_ANDROID
        public delegate void deferredDeeplinkDelegate(string deferredDeeplink);
        public static deferredDeeplinkDelegate didReceiveDeferredDeeplink;

        public delegate void DeeplinkDelegate(string deeplink);
        public static DeeplinkDelegate didReceiveDeeplink;
#endif
        //디퍼드 딥링크 원래 함수


        public static void InitPlugin() {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID
				if(_igaworksUnityPluginAosInstance == null) {
					Debug.Log ("#########################################");
					Debug.Log ("IGAWorksAdbrixUnityPluginAOS GameObject Created!!!");
					_igaworksUnityPluginAosInstance = new GameObject("AOS-SampleObject").AddComponent<AdBrixRm>();
				}		

				_igaworksUnityPluginAosClass = new AndroidJavaClass("com.igaworks.v2.core.unity.AbxUnityPlugin");


				if(_igaworksUnityPluginAosClass != null){
					Debug.Log ("#########################################");
					Debug.Log ("IGAWorksAdbrixUnityPluginAOS Connected!!!");
					Debug.Log ("#########################################");
				}
				else{
					Debug.Log ("#########################################");
					Debug.Log ("IGAWorksAdbrixUnityPluginAOS Connect FAIL!!!");
					Debug.Log ("#########################################");
				}

				if(_igaworksUnityPluginAosClass != null){
					Debug.Log ("#########################################");
					Debug.Log ("AndroidJavaClass Connected!!!");
					Debug.Log ("#########################################");
				}
				else{
					Debug.Log ("#########################################");
					Debug.Log ("AndroidJavaClass Connect FAIL!!!");
					Debug.Log ("#########################################");
				}
			#endif 
		}

		//public static void initializeSdk(string appKey, string secretKey) {

		//	#if UNITY_EDITOR
		//		Debug.Log("igaworks:Editor mode Connected");
		//	#elif UNITY_ANDROID 
		//		_igaworksUnityPluginAosClass.CallStatic ("initializeSdk", appKey, secretKey);
		//	#endif 

		//}

		public static void SetCallbackHandler(string callbackClassNm )
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (Application.platform == RuntimePlatform.OSXEditor)
			return;

			_igaworksUnityPluginAosClass.CallStatic ("SetCallbackHandler",callbackClassNm);
#endif
        }

    public static void SetAdBrixDeferredDeeplinkDelegate()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (Application.platform == RuntimePlatform.OSXEditor)
			return;

			_igaworksUnityPluginAosClass.CallStatic ("SetAdBrixDeeplinkDelegate");
			#endif
		}


		public void DidReceiveDeferredDeeplink(string deferredDeepLink)
		{


       
            #if UNITY_ANDROID
            if (deferredDeepLink != null) 
			{
                Debug.Log("DeferredDeeplink Message Arrived!");
                didReceiveDeferredDeeplink(deferredDeepLink);
			}
			#endif
		}
        public void DidReceiveDeeplink(string DeepLink)
        {

            Debug.Log("DidReceiveDeeplink Message Arrived");
#if UNITY_ANDROID
            if (DeepLink != null)
            {
                Debug.Log("deeplink Open Message : "+DeepLink);
                didReceiveDeeplink(DeepLink);
                postDeeplinkOpenEvnet();
            }
#endif
        }




        public static void setLogLevel(AdBrixRm.AdBrixLogLevel logLevel) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				_igaworksUnityPluginAosClass.CallStatic ("setLogLevel", (int)logLevel);
			#endif 
		}

		public static void setEventUploadCountInterval(AdBrixRm.AdBrixEventUploadCountInterval interval) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				_igaworksUnityPluginAosClass.CallStatic ("setEventUploadCountInterval", (int)interval);
			#endif 

		}


		public static void setEventUploadTimeInterval(AdBrixRm.AdBrixEventUploadTimeInterval interval) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				_igaworksUnityPluginAosClass.CallStatic ("setEventUploadTimeInterval", (int)interval);
			#endif 

		}

		public static void gdprForgetMe() {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
			_igaworksUnityPluginAosClass.CallStatic ("gdprForgetMe");
			#endif 
		}

		public static void setAge(int age) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				_igaworksUnityPluginAosClass.CallStatic ("setAge", age);
			#endif 
		}

		public static void setGender(AdBrixRm.Gender gender) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				_igaworksUnityPluginAosClass.CallStatic ("setGender", (int)gender);
			#endif 
		}

		public static void setUserProperties(Dictionary<string, object> prop) {

			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 

			try {
				var listKey = prop.Keys.ToArray();
				var listValue =  prop.Values.ToArray();
				if (listKey != null && listValue != null && listKey.Length > 0 && listValue.Length > 0 && listKey.Length == listValue.Length) {
					foreach(KeyValuePair<string, object> entry in prop) {
						if (entry.Value.GetType() == typeof(string) || entry.Value.GetType() == typeof(int) || entry.Value.GetType() == typeof(float) || entry.Value.GetType() == typeof(double)
							|| entry.Value.GetType() == typeof(long) || entry.Value.GetType() == typeof(bool)) {
							_igaworksUnityPluginAosClass.CallStatic("setUserProperties", entry.Key, entry.Value);
						}
					} 
				}
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

			#endif 

		}

        public static void clearUserProperties() {
            #if UNITY_EDITOR
                        Debug.Log("igaworks:Editor mode Connected");
            #elif UNITY_ANDROID
				 _igaworksUnityPluginAosClass.CallStatic("clearUserProperties");
            #endif
        }

        public static void events(string eventName) {
			#if UNITY_ANDROID && !UNITY_EDITOR
				_igaworksUnityPluginAosClass.CallStatic("event", eventName);
			#endif

		}

		public static void events(string eventName, Dictionary<string, object> value) {
			
			#if UNITY_ANDROID && !UNITY_EDITOR
				if (value != null) {
					_igaworksUnityPluginAosClass.CallStatic("event", eventName, MiniJson_aos.Serialize(value));
				}
			#endif

		}

		public static void login(string userId) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
				_igaworksUnityPluginAosClass.CallStatic("login", userId);
			#endif 

		}

        public static void logout() { 
            #if UNITY_EDITOR
                        Debug.Log("igaworks:Editor mode Connected");
            #elif UNITY_ANDROID
			  _igaworksUnityPluginAosClass.CallStatic("logout");
            #endif
        }


		//start of COMMERCE --
		public static void commerceViewHome() {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
				_igaworksUnityPluginAosClass.CallStatic("commerceViewHome");
			#endif 
		}

		public static void commerceViewHome(Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID           
			_igaworksUnityPluginAosClass.CallStatic("commerceViewHome", MiniJson_aos.Serialize (extraAttr));
			#endif 
		}


		public static void commerceCategoryView(AdBrixRmCommerceProductCategoryModel category, List<AdBrixRmCommerceProductModel> productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceCategoryView", category.getCategoryFullString(), jsonDataString);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceCategoryViewBulk", category.getCategoryFullString(), jsonDataString);

					}
				}
			}
			else {
				_igaworksUnityPluginAosClass.CallStatic("commerceCategoryView", category.getCategoryFullString(), null);
			}
			#endif
		}


		public static void commerceCategoryView(AdBrixRmCommerceProductCategoryModel category, List<AdBrixRmCommerceProductModel> productInfo, Dictionary<string, object> extraAttr) {

			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
				
				if(productInfo != null && productInfo.Count > 0) {
					if (productInfo.Count == 1) {
						string jsonDataString = "[";
						jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
						_igaworksUnityPluginAosClass.CallStatic("commerceCategoryView", category.getCategoryFullString(), jsonDataString, MiniJson_aos.Serialize (extraAttr));
					} 
					else if(productInfo.Count >= 2){
						List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
						for(int i  = 0, j= productInfo.Count; i < j; i++) {
							if (productInfo [i] != null) {
								filterList.Add (productInfo[i]);
							}
						}

						if(filterList != null && filterList.Count > 0) {
							string jsonDataString = "[";
							for(int i = 0; i < filterList.Count; i++) {
								AdBrixRmCommerceProductModel item = filterList[i];
								if(i == (filterList.Count-1)) {
									jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
								}
								else {
									jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
								}
							}
							_igaworksUnityPluginAosClass.CallStatic("commerceCategoryViewBulk", category.getCategoryFullString(), jsonDataString, MiniJson_aos.Serialize (extraAttr));

						}
					}
				}
				else {
					_igaworksUnityPluginAosClass.CallStatic("commerceCategoryView", category.getCategoryFullString(), null, MiniJson_aos.Serialize (extraAttr));
				}

			#endif 
		}

		public static void commerceProductView(AdBrixRmCommerceProductModel productInfo) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
				if (productInfo == null) {					
					Debug.Log("igaworks:productView >> Null or Empty Item");
					return;
				}

				
				string jsonDataString = "[";
				jsonDataString = jsonDataString + stringifyCommerceItem (productInfo) + "]";
				_igaworksUnityPluginAosClass.CallStatic("commerceProductView", jsonDataString);
				
			#endif 

		}

		public static void commerceProductView(AdBrixRmCommerceProductModel productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID           
				if (productInfo == null) {					
					Debug.Log("igaworks:productView >> Null or Empty Item");
					return;
				}


				string jsonDataString = "[";
				jsonDataString = jsonDataString + stringifyCommerceItem (productInfo) + "]";
				_igaworksUnityPluginAosClass.CallStatic("commerceProductView", jsonDataString, MiniJson_aos.Serialize (extraAttr));
			#endif 
		}

		public static void commerceAddToCart(List<AdBrixRmCommerceProductModel> productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceAddToCart", jsonDataString);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceAddToCartBulk", jsonDataString);

					}
				}
			}
			#endif
		}

		public static void commerceAddToCart(List<AdBrixRmCommerceProductModel> productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceAddToCart", jsonDataString, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceAddToCartBulk", jsonDataString, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif
		}

		public static void commerceAddToWishList(AdBrixRmCommerceProductModel productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if (productInfo == null) {					
				Debug.Log("igaworks:productView >> Null or Empty Item");
				return;
			}


			string jsonDataString = "[";
			jsonDataString = jsonDataString + stringifyCommerceItem (productInfo) + "]";

			_igaworksUnityPluginAosClass.CallStatic("commerceAddToWishList", jsonDataString);
			#endif
		}

		public static void commerceAddToWishList(AdBrixRmCommerceProductModel productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if (productInfo == null) {					
				Debug.Log("igaworks:productView >> Null or Empty Item");
				return;
			}

			string jsonDataString = "[";
			jsonDataString = jsonDataString + stringifyCommerceItem (productInfo) + "]";

			_igaworksUnityPluginAosClass.CallStatic("commerceAddToWishList", jsonDataString, MiniJson_aos.Serialize (extraAttr));
			#endif
		}

		public static void commerceReviewOrder(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double discount,  double deliveryCharge) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceReviewOrder", orderId, jsonDataString, discount, deliveryCharge);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceReviewOrderBulk", orderId, jsonDataString, discount, deliveryCharge);

					}
				}
			}
			#endif
		}

		public static void commerceReviewOrder(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double discount, double deliveryCharge, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceReviewOrder", orderId, jsonDataString, discount, deliveryCharge, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceReviewOrderBulk", orderId, jsonDataString, discount, deliveryCharge, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif
		}

		//Refund
		public static void commerceRefund(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double penaltyCharge) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceRefund", orderId, jsonDataString, penaltyCharge);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceRefundBulk", orderId, jsonDataString, penaltyCharge);

					}
				}
			}
			#endif
		}

		public static void commerceRefund(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double penaltyCharge, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceRefund", orderId, jsonDataString, penaltyCharge, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceRefundBulk", orderId, jsonDataString, penaltyCharge, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif
		}

		//Search
		public static void commerceSearch(string keyword, List<AdBrixRmCommerceProductModel> productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceSearch", keyword, jsonDataString);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceSearchBulk", keyword, jsonDataString);

					}
				}
			}
			#endif
		}

		public static void commerceSearch(string keyword, List<AdBrixRmCommerceProductModel> productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceSearch", keyword, jsonDataString, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceSearchBulk", keyword, jsonDataString, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif
		}

		//Share
		public static void commerceShare(AdBrixRm.SharingChannel sharingChannel, AdBrixRmCommerceProductModel productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null) {
				string jsonDataString = "[";
				jsonDataString = jsonDataString + stringifyCommerceItem(productInfo) + "]";
				_igaworksUnityPluginAosClass.CallStatic("commerceShare", sharingChannel.ToString(), jsonDataString);
			}
			#endif
		}

		public static void commerceShare(AdBrixRm.SharingChannel sharingChannel, AdBrixRmCommerceProductModel productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null) {
				string jsonDataString = "[";
				jsonDataString = jsonDataString + stringifyCommerceItem(productInfo) + "]";
			_igaworksUnityPluginAosClass.CallStatic("commerceShare", sharingChannel.ToString(), jsonDataString, MiniJson_aos.Serialize (extraAttr));
			}
			#endif
		}

		//list view
		public static void commerceListView(List<AdBrixRmCommerceProductModel> productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceListView", jsonDataString);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceListViewBulk", jsonDataString);

					}
				}
			}
			#endif 
		}


		public static void commerceListView(List<AdBrixRmCommerceProductModel> productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceListView", jsonDataString, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceListViewBulk", jsonDataString, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif 
		}

		//cart view
		public static void commerceCartView(List<AdBrixRmCommerceProductModel> productInfo) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceCartView", jsonDataString);
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceCartViewBulk", jsonDataString);

					}
				}
			}
			#endif 
		}


		public static void commerceCartView(List<AdBrixRmCommerceProductModel> productInfo, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commerceCartView", jsonDataString, MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commerceCartViewBulk", jsonDataString, MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif 
		}

		public static void commercePaymentInfoAdded() {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("commercePaymentInfoAdded");
			#endif 
		}


		public static void commercePaymentInfoAdded(Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID           
			_igaworksUnityPluginAosClass.CallStatic("commercePaymentInfoAdded", MiniJson_aos.Serialize (extraAttr));
			#endif 
		}
		//end of COMMERCE --


		//start of COMMON --
		//purchase
		public static void commonPurchase(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double discount, double deliveryCharge, AdBrixRm.PaymentMethod paymentMethod) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commonPurchase", orderId, jsonDataString, discount, deliveryCharge, paymentMethod.ToString());
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commonPurchaseBulk", orderId, jsonDataString, discount, deliveryCharge, paymentMethod.ToString());

					}
				}
			}
			#endif
		}



		public static void commonPurchase(string orderId, List<AdBrixRmCommerceProductModel> productInfo, double discount, double deliveryCharge, AdBrixRm.PaymentMethod paymentMethod, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID 
			if(productInfo != null && productInfo.Count > 0) {
				if (productInfo.Count == 1) {
					string jsonDataString = "[";
					jsonDataString = jsonDataString + stringifyCommerceItem(productInfo[0]) + "]";
					_igaworksUnityPluginAosClass.CallStatic("commonPurchase", orderId, jsonDataString, discount, deliveryCharge, paymentMethod.ToString(), MiniJson_aos.Serialize (extraAttr));
				} 
				else if(productInfo.Count >= 2){
					List<AdBrixRmCommerceProductModel> filterList = new List<AdBrixRmCommerceProductModel>();
					for(int i  = 0, j= productInfo.Count; i < j; i++) {
						if (productInfo [i] != null) {
							filterList.Add (productInfo[i]);
						}
					}

					if(filterList != null && filterList.Count > 0) {
						string jsonDataString = "[";
						for(int i = 0; i < filterList.Count; i++) {
							AdBrixRmCommerceProductModel item = filterList[i];
							if(i == (filterList.Count-1)) {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + "]";
							}
							else {
								jsonDataString = jsonDataString + stringifyCommerceItem(item) + ",";		
							}
						}
						_igaworksUnityPluginAosClass.CallStatic("commonPurchaseBulk", orderId, jsonDataString, discount, deliveryCharge, paymentMethod.ToString(), MiniJson_aos.Serialize (extraAttr));

					}
				}
			}
			#endif
		}


		public static void commonSignUp(AdBrixRm.SignUpChannel channel) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonSignUp", channel.ToString());
			#endif
		}

		public static void commonSignUp(AdBrixRm.SignUpChannel channel, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonSignUp", channel.ToString(), MiniJson_aos.Serialize (extraAttr));
			#endif
		}

		public static void commonUseCredit() {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonUseCredit");
			#endif
		}

		public static void commonUseCredit(Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonUseCredit", MiniJson_aos.Serialize (extraAttr));
			#endif
		}

		public static void commonAppUpdate(string prev_ver, string curr_ver) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				if(prev_ver != null && curr_ver != null) {
					_igaworksUnityPluginAosClass.CallStatic("commonAppUpdate", prev_ver, curr_ver);
				}
			#endif
		}

		public static void commonAppUpdate(string prev_ver, string curr_ver, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID
				if(prev_ver != null && curr_ver != null) {
					_igaworksUnityPluginAosClass.CallStatic("commonAppUpdate", prev_ver, curr_ver, MiniJson_aos.Serialize (extraAttr));
				}
			#endif
		}

		public static void commonInvite(AdBrixRm.InviteChannel channel) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonInvite", channel.ToString());
			#endif
		}

		public static void commonInvite(AdBrixRm.InviteChannel channel, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
				Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID    
				_igaworksUnityPluginAosClass.CallStatic("commonInvite", channel.ToString(), MiniJson_aos.Serialize (extraAttr));
			#endif
		}

		//end of COMMON --


		//start of GAME --
		public static void gameTutorialCompleted(bool is_skip) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameTutorialCompleted", is_skip);
			#endif 
		}

		public static void gameTutorialCompleted(bool is_skip, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameTutorialCompleted", is_skip, MiniJson_aos.Serialize (extraAttr));
			#endif 
		}

		//gameLevelAchieved
		public static void gameLevelAchieved(int level) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameLevelAchieved", level);
			#endif 
		}


		public static void gameLevelAchieved(int level, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameLevelAchieved", level, MiniJson_aos.Serialize (extraAttr));
			#endif 
		}

		//gameCharacterCreated
		public static void gameCharacterCreated() {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameCharacterCreated");
			#endif 
		}

		public static void gameCharacterCreated(Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameCharacterCreated", MiniJson_aos.Serialize (extraAttr));
			#endif 
		}

		//gameStageCleared

		public static void gameStageCleared() {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameStageCleared");
			#endif 
		}

		public static void gameStageCleared(string stageName) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameStageCleared", stageName);
			#endif 
		}


		public static void gameStageCleared(string stageName, Dictionary<string, object> extraAttr) {
			#if UNITY_EDITOR
			Debug.Log("igaworks:Editor mode Connected");
			#elif UNITY_ANDROID              
			_igaworksUnityPluginAosClass.CallStatic("gameStageCleared", stageName, MiniJson_aos.Serialize (extraAttr));
			#endif 
		}

		//end of GAME --

		public static string stringifyCommerceItem(AdBrixRmCommerceProductModel item) {
			
			string jsonString = "null";
			if (item != null) {
				if (item.productId == null) item.productId = "unknown";
				if (item.productName == null) item.productName = "unknown";
				if (item.currency == null) item.currency = "unknown";
				if (item.category == null) item.category = "";
				if (item.extraAttrsDic == null) item.extraAttrsDic = new Dictionary<string, object>();

				Dictionary<string, object> dic = new Dictionary<string, object>();
				dic.Add("productId", item.productId);
				dic.Add("productName", item.productName);
				dic.Add("price", item.price);
				dic.Add("currency", item.currency);
				dic.Add("discount", item.discount);
				dic.Add("quantity", item.quantity);
				dic.Add("category", item.category);
				dic.Add("extra_attrs", item.extraAttrsDic);
				jsonString = MiniJson_aos.Serialize(dic);
			}
			return jsonString;

		}

		[System.Serializable]
		public class PurchaseItemModel	{
			public string orderId = "unknown";
			public string productId = "unknown";
			public string productName = "unknown";
			public double price = 0;
			public int quantity = 1;
			public string currency = "unknown";
			public string category = "unknown";

			public PurchaseItemModel(string orderID, string productID,
				string productName, double price, int quantity, string currency,
				string category) {
				if (orderID != null && orderID.Length > 0) this.orderId = orderID;
				if (productID != null && productID.Length > 0) this.productId = productID;
				if (productName != null && productName.Length > 0) this.productName = productName;
				this.price = price;
				this.quantity = quantity;
				if (currency != null && currency.Length > 0) this.currency = currency;
				if (category != null && category.Length > 0) this.category = category;
			}
		}

		public class AdBrixRmCommerceProductModel {
			public String productId = "";
			public String productName = "";
			public double price = 0.0;
			public double discount = 0.0;
			public int quantity = 1;
			public string currency = "unknown"; //this is set already by enum....
			public String category;
			public Dictionary<string, object> extraAttrsDic;


			public AdBrixRmCommerceProductModel(string productId, string productName,
				double price, int quantity, double discount, AdBrixRmAOS.AdBrixRm.Currency currency,
				AdBrixRmCommerceProductCategoryModel category, AdBrixRmCommerceProductAttrModel attr) {
				extraAttrsDic = null;
				extraAttrsDic = new Dictionary<string, object>();
				if (attr != null) {
					for (int i = 0; i < 5; i++) {
						if (attr.key[i] != null) {
							if (!attr.key[i].Equals("")) {
								extraAttrsDic.Add(attr.key[i], attr.value[i]);
							}
						}
					}
				}

				if (productId != null && productId.Length > 0) this.productId = productId;
				if (productName != null && productName.Length > 0) this.productName = productName;
				this.price = price;
				this.discount = discount;
				this.quantity = quantity;
				if (currency != null ) this.currency = currency.ToString();
				if (category != null)
					this.category = category.getCategoryFullString();
			}


			public static AdBrixRmCommerceProductModel create(string productId, string productName, double price, int quantity, double discount, AdBrixRmAOS.AdBrixRm.Currency currency, AdBrixRmCommerceProductCategoryModel category, AdBrixRmCommerceProductAttrModel extraAttrs) {
				return new AdBrixRmCommerceProductModel(productId, productName, price, quantity, discount, currency, category, extraAttrs);
			}
		}

		public class AdBrixRmCommerceProductCategoryModel {

			String category1;
			String category2;
			String category3;
			String category4;
			String category5;


			private String categoryFullString;

			protected AdBrixRmCommerceProductCategoryModel() { }

			public AdBrixRmCommerceProductCategoryModel(String category1)
			{
				this.category1 = category1;
				this.setCategoryFullString(this.category1);
			}

			public AdBrixRmCommerceProductCategoryModel(String category1, String category2)
			{
				this.category1 = category1;
				this.category2 = category2;
				this.setCategoryFullString(this.category1 + "." + category2);
			}

			public AdBrixRmCommerceProductCategoryModel(String category1, String category2, String category3)
			{
				this.category1 = category1;
				this.category2 = category2;
				this.category3 = category3;
				this.setCategoryFullString(this.category1 + "." + category2 + "." + category3);
			}

			public AdBrixRmCommerceProductCategoryModel(String category1, String category2, String category3, String category4)
			{
				this.category1 = category1;
				this.category2 = category2;
				this.category3 = category3;
				this.category4 = category4;
				this.setCategoryFullString(this.category1 + "." + category2 + "." + category3 + "." + category4);
			}

			public AdBrixRmCommerceProductCategoryModel(String category1, String category2, String category3, String category4, String category5)
			{
				this.category1 = category1;
				this.category2 = category2;
				this.category3 = category3;
				this.category4 = category4;
				this.category5 = category5;
				this.setCategoryFullString(this.category1 + "." + category2 + "." + category3 + "." + category4 + "." + category5);
			}

			public static AdBrixRmCommerceProductCategoryModel create(String category1)
			{
				return new AdBrixRmCommerceProductCategoryModel(category1);
			}

			public static AdBrixRmCommerceProductCategoryModel create(String category1, String category2)
			{
				return new AdBrixRmCommerceProductCategoryModel(category1, category2);
			}

			public static AdBrixRmCommerceProductCategoryModel create(String category1, String category2, String category3)
			{
				return new AdBrixRmCommerceProductCategoryModel(category1, category2, category3);
			}

			public static AdBrixRmCommerceProductCategoryModel create(String category1, String category2, String category3, String category4)
			{
				return new AdBrixRmCommerceProductCategoryModel(category1, category2, category3, category4);
			}

			public static AdBrixRmCommerceProductCategoryModel create(String category1, String category2, String category3, String category4, String category5)
			{
				return new AdBrixRmCommerceProductCategoryModel(category1, category2, category3, category4, category5);
			}

			public String getCategoryFullString()
			{
				return categoryFullString;
			}

			public void setCategoryFullString(String categoryFullString)
			{
				this.categoryFullString = categoryFullString;
			}
		}

		public class AdBrixRmCommerceProductAttrModel {
			public String[] key = new String[5];
			public String[] value = new String[5];

			protected AdBrixRmCommerceProductAttrModel() { }

			public AdBrixRmCommerceProductAttrModel(Dictionary<string, string> attrData) {
				if (attrData != null) {

					int i = 0;
					foreach (KeyValuePair<string, string> pair in attrData) {
                        if (i > 4)
                        {
                            break;
                        }
                        this.key[i] = pair.Key;
						this.value[i] = pair.Value;
						i++;
					}
				}
			}

			public static AdBrixRmCommerceProductAttrModel create(Dictionary<string, string> attrData) {
				return new AdBrixRmCommerceProductAttrModel(attrData);
			}
		}



		// For commerce API:
		private static Dictionary<string, object> PurchaseItem2Dictionary(PurchaseItemModel item) {
			Dictionary<string, object> dic = null;
			if (item != null) {
				dic = new Dictionary<string, object>();
				dic.Add("orderId", item.orderId);
				dic.Add("productId", item.productId);
				dic.Add("productName", item.productName);
				dic.Add("price", item.price);
				dic.Add("currency", item.currency);
				dic.Add("quantity", item.quantity);
				dic.Add("category", item.category);
			}
			return dic;
		}

		public static string stringifyCommerceItem(PurchaseItemModel item) {
			Dictionary<string, object> dic = PurchaseItem2Dictionary(item);
			return MiniJson_aos.Serialize(dic);
		}

		void OnDestroy()
		{

		}


        public static void startSession()
        {
#if UNITY_ANDROID
            Debug.Log("startSession()");
            if (initilizeSdkSucceed) _igaworksUnityPluginAosClass.CallStatic("startSession");

#endif
        }

        public static void endSession()
        {

#if UNITY_ANDROID
            Debug.Log("endSession()");
                _igaworksUnityPluginAosClass.CallStatic ("endSession");
#endif


        }
        public void sdkInitializeSuccess(string state)
        {
            Debug.Log("initializeSuccess : "+state);
            if(state.Equals("true"))
            initilizeSdkSucceed = true;
            startSession();
        }


        public void postDeeplinkOpenEvnet()
        {
#if UNITY_ANDROID
            _igaworksUnityPluginAosClass.CallStatic("postDeeplinkOpenEvnet");
#endif
        }

        public void pushResult(string state)
        {
            Debug.Log("pushEnable state : "+state);
        }

        public static void setPushEnable(bool toEnable)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
              Debug.Log("set Push Enable :: Method Called");
            _igaworksUnityPluginAosClass.CallStatic("setPushEnable",toEnable);
#endif
        }

        public void registrationResult(string state)
        {
            Debug.Log("Set registration-id : " + state);
        }
        public static void setRegistrationId(string token)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("registrationId Method is called !!");
            _igaworksUnityPluginAosClass.CallStatic("setRegistrationId", token);
#endif


        }

        public static void setAppScanEnable(bool enable)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("setAppScanEnable Method is called !!");
            _igaworksUnityPluginAosClass.CallStatic("setAppScanEnable", enable);
#endif


        }


    }

}

