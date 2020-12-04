//
//  AdBrixRmBridge.mm
//
//  Created by freddy on 2018.
//  Copyright © 2018년 igaworks. All rights reserved.
//
//다른 cs  클래스에서 본 브릿지의 static 함수를 호출하여 사용하며,
//각 함수별로 AdBrix API를 사용하는 AdBrixRmBridge.mm 클래스 파일내의 함수들을 호출합니다.
//본 브릿지 함수들 구조는 연동 개발간 수정하지 않도록 유의해주십시오. 본 플러그인 수정에 따른 데이터 분석 불가 현상이 발생하는 경우, 이에 대한 귀책사유는 연동 개발자에게 귀속됩니다.

#import <Foundation/Foundation.h>
//#import "unityswift-Swift.h"
#import "AdBrixRM-Swift.h"
#import <AdSupport/AdSupport.h>
#import "AdBrixRmBridge.h"

UIViewController *UnityGetGLViewController();

static AdBrixRmBridge *_sharedInstance = nil; //To make IgaworksCorePlugin Singleton

@implementation AdBrixRmBridge

@synthesize callbackHandlerName = _callbackHandlerName;

- (void)setAdBrixDeeplinkDelegate
{
    [[AdBrixRM sharedInstance] setDeeplinkDelegateWithDelegate:self];
}
- (void)setAdBrixDeferredDeeplinkDelegate
{
    [[AdBrixRM sharedInstance] setDeferredDeeplinkDelegateWithDelegate:self];
}

- (void)didReceiveDeeplinkWithDeeplink:(NSString *)deeplink {
    
    if(deeplink != nil && _callbackHandlerName != nil) {
        NSLog(@"- (void)didReceiveDeeplinkWithDeeplink:(NSString *)deepLink : %@", deeplink);
        UnitySendMessage([_callbackHandlerName UTF8String], "DidReceiveDeeplink", [deeplink UTF8String]);
    }
    else {
        NSLog(@"- (void)didReceiveDeferredDeeplinkWithDeeplink: nil");
    }
}

- (void)didReceiveDeferredDeeplinkWithDeeplink:(NSString *)deeplink {
    
    if(deeplink != nil && _callbackHandlerName != nil) {
        NSLog(@"- (void)didReceiveDeferredDeeplinkWithDeeplink:(NSString *)deepLink : %@", deeplink);
        UnitySendMessage([_callbackHandlerName UTF8String], "DidReceiveDeferredDeeplink", [deeplink UTF8String]);
    }
    else {
        NSLog(@"- (void)didReceiveDeferredDeeplinkWithDeeplink: nil");
    }
}


+ (void)initialize
{
    if (self == [AdBrixRmBridge class])
    {
        _sharedInstance = [[self alloc] init];
    }
}


+ (AdBrixRmBridge *)sharedAdBrixRmBridge
{
    return _sharedInstance;
}

- (id)init
{
    self = [super init];
    
    if (self)
    {
        
    }
    return self;
}

+ (NSString *)checkNilToBlankString:(id)target
{
    NSString *returnString = @"";
    if (!([target isEqual:[NSNull null]] || target == nil))
    {
        returnString = target;
    }
    
    return returnString;
}

+ (double)checkDoubleNilToZero:(id)target
{
    double returnDouble = 0.0f;
    if (!([target isEqual:[NSNull null]] || target == nil))
    {
        returnDouble = (double)[target doubleValue];
    }
    
    return returnDouble;
}

+ (NSInteger)checkIntegerNilToZero:(id)target
{
    NSInteger returnInteger = 0;
    if (!([target isEqual:[NSNull null]] || target == nil))
    {
        returnInteger = [target integerValue];
    }
    
    return returnInteger;
}

+ (AdBrixRmCommerceProductModel *)makeProductFromJsonForCommerce:(NSString *)purchaseDataJsonString
{
    try {
        
        NSString *_productId = @"";
        NSString *_productName = @"";
        double _price = 0.0;
        double _discount = 0.0;
        NSUInteger _quantity = 1;
        NSString *_currency = @"";
        
        NSMutableDictionary *_extraAttrs;
        
        id dict=[NSJSONSerialization JSONObjectWithData:[purchaseDataJsonString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
        AdBrixRmCommerceProductCategoryModel * _cate;
        for (id element in dict)
        {
            for(NSString* key in element)
            {
                
                
                if(![key isKindOfClass:[NSNull class]])
                {
                    if ([key isEqualToString:@"productId"])
                    {
                        _productId = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"productName"])
                    {
                        _productName = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"price"])
                    {
                        _price = [self checkDoubleNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"discount"])
                    {
                        _discount = [self checkDoubleNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"quantity"])
                    {
                        _quantity = [self checkIntegerNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"currency"])
                    {
                        _currency = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"category"])
                    {
                        NSString *categories[5];
                        NSString *pCategories = [self checkNilToBlankString : [element objectForKey:key]];
                        if (pCategories) {
                            NSArray* categoryList = [pCategories componentsSeparatedByString:@"."];
                            for (int i=0; i<categoryList.count; ++i)
                            {
                                categories[i] = categoryList[i];
                            }
                        }
                        _cate = [[AdBrixRM sharedInstance] createCommerceProductCategoryDataWithCategory:categories[0] category2:categories[1] category3:categories[2] category4:categories[3] category5:categories[4]];
                    }
                    if ([key isEqualToString:@"extra_attrs"])
                    {
                        _extraAttrs = [element objectForKey:key];
                    }
                }
            }
        }
        return [[AdBrixRM sharedInstance] createCommerceProductDataWithProductId:_productId
                                                                     productName:_productName
                                                                           price:_price
                                                                        quantity:_quantity
                                                                        discount:_discount
                                                                  currencyString:_currency
                                                                        category:_cate
                                                                 productAttrsMap:[[AdBrixRM sharedInstance] createCommerceProductAttrDataWithDictionary:_extraAttrs]];
    }
    catch (NSException *exception)
    {
        NSLog(@"fail to make product for iOS native : %@", exception);
    }
    return nil;
}

+ (NSArray<AdBrixRmCommerceProductModel *> *)makeProductsFromJsonForCommerce:(NSString *)purchaseDataJsonString
{
    try {
        
        NSString *_productId = @"";
        NSString *_productName = @"";
        double _price = 0.0;
        double _discount = 0.0;
        NSUInteger _quantity = 1;
        NSString *_currency = @"";
        
        NSMutableDictionary *_extraAttrs;
        NSMutableArray<AdBrixRmCommerceProductModel *> *productArray = [NSMutableArray array];
        AdBrixRmCommerceProductCategoryModel * _cate;
        
        id dict=[NSJSONSerialization JSONObjectWithData:[purchaseDataJsonString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
        
        for (id element in dict)
        {
            for(NSString* key in element)
            {
                
                if(![key isKindOfClass:[NSNull class]])
                {
                    if ([key isEqualToString:@"productId"])
                    {
                        _productId = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"productName"])
                    {
                        _productName = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"price"])
                    {
                        _price = [self checkDoubleNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"discount"])
                    {
                        _discount = [self checkDoubleNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"quantity"])
                    {
                        _quantity = [self checkIntegerNilToZero : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"currency"])
                    {
                        _currency = [self checkNilToBlankString : [element objectForKey:key]];
                    }
                    if ([key isEqualToString:@"category"])
                    {
                        NSString *categories[5];
                        NSString *pCategories = [self checkNilToBlankString : [element objectForKey:key]];
                        if (pCategories) {
                            NSArray* categoryList = [pCategories componentsSeparatedByString:@"."];
                            for (int i=0; i<categoryList.count; ++i)
                            {
                                categories[i] = categoryList[i];
                            }
                        }
                        _cate = [[AdBrixRM sharedInstance] createCommerceProductCategoryDataWithCategory:categories[0] category2:categories[1] category3:categories[2] category4:categories[3] category5:categories[4]];
                    }
                    if ([key isEqualToString:@"extra_attrs"])
                    {
                        _extraAttrs = [element objectForKey:key];
                    }
                }
            }
            
            
            AdBrixRmCommerceProductModel *productModel = [[AdBrixRM sharedInstance] createCommerceProductDataWithProductId:_productId
                                                                                                               productName:_productName
                                                                                                                     price:_price
                                                                                                                  quantity:_quantity
                                                                                                                  discount:_discount
                                                                                                            currencyString:_currency
                                                                                                                  category:_cate
                                                                                                           productAttrsMap:[[AdBrixRM sharedInstance] createCommerceProductAttrDataWithDictionary:_extraAttrs]
                                                          ];
            
            [productArray addObject: productModel];
        }
        return productArray;
    }
    catch (NSException *exception)
    {
        NSLog(@"fail to make product for iOS native : %@", exception);
    }
    return nil;
}

+(AdBrixRmCommerceProductCategoryModel *)makeCategoryFromStringForCommerce: (NSString *)categoryString
{
    NSString *categories[5];
    if (categoryString) {
        NSArray* categoryList = [categoryString componentsSeparatedByString:@"."];
        for (int i=0; i<categoryList.count; ++i)
        {
            categories[i] = categoryList[i];
        }
    }
    
    return [[AdBrixRM sharedInstance] createCommerceProductCategoryDataWithCategory:categories[0] category2:categories[1] category3:categories[2] category4:categories[3] category5:categories[4]];
}

+ (NSMutableDictionary* )makeExtraAttrDictionaryFromJson:(NSString *)jsonString
{
    try {
        
        NSMutableDictionary *_extraAttrs = [NSMutableDictionary dictionary];
        
        id dict = [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
        
        for(NSString* key in dict)
        {
            if(![key isKindOfClass:[NSNull class]])
            {
                [_extraAttrs setValue:[dict objectForKey:key] forKey:key];
            }
        }
        
        return _extraAttrs;
    }
    catch (NSException *exception)
    {
        NSLog(@"fail to make product for iOS native : %@", exception);
    }
    return nil;
}

extern "C" {
    
    void _SetCallbackHandlerRm(const char* handlerName) {
        [[AdBrixRmBridge sharedAdBrixRmBridge] setCallbackHandlerName:[NSString stringWithUTF8String:handlerName]];
        NSLog(@"callbackHandlerName: %@", [[AdBrixRmBridge sharedAdBrixRmBridge] callbackHandlerName]);
    }
    
    void _SetAdBrixDeeplinkDelegate()
    {
         NSLog(@"AdBrixRM: _SetAdBrixDeeplinkDelegate");
        [[AdBrixRmBridge sharedAdBrixRmBridge] setAdBrixDeeplinkDelegate];
       
    }
    
    void _SetAdBrixDeferredDeeplinkDelegate()
    {
        NSLog(@"AdBrixRM: _SetAdBrixDeferredDeeplinkDelegate");
        [[AdBrixRmBridge sharedAdBrixRmBridge] setAdBrixDeferredDeeplinkDelegate];
        
    }
    
    void _initAdBrix(const char* appKey, const char* secretKey) {
         NSLog(@"AdBrixRM: _initAdBrix");
        [[AdBrixRM sharedInstance] initAdBrixWithAppKey:[NSString stringWithUTF8String:appKey] secretKey:[NSString stringWithUTF8String:secretKey]];
    }
    
    void _setPushEnable(BOOL toEnable) {
        NSLog(@"AdBrixRM: _setPushEnable");
        [[AdBrixRM sharedInstance] setPushEnableToPushEnable:toEnable];
    }
    
    void _setRegistrationId(const char* deviceToken) {
        NSLog(@"AdBrixRM: _setRegistrationId");
        
        NSString *pToken =  [NSString stringWithUTF8String:deviceToken];
        pToken = [pToken stringByReplacingOccurrencesOfString: @ "-" withString: @ ""];
        pToken = [pToken lowercaseString];
        
        
        [[AdBrixRM sharedInstance] setRegistrationIdForUnityWithDeviceToken:pToken];
        
    }
    
    void _gdprForgetMe() {
         NSLog(@"AdBrixRM: _gdprForgetMe");
        [[AdBrixRM sharedInstance] gdprForgetMe];
    }
    
    void _setLogLevel(int logLevel) {
         NSLog(@"AdBrixRM: _setLogLevel");
        [[AdBrixRM sharedInstance] setLogLevel:[[AdBrixRM sharedInstance] convertLogLevel:logLevel]];
    }
    
    void _setEventUploadCountInterval(int countInterval) {
        [[AdBrixRM sharedInstance] setEventUploadCountInterval:[[AdBrixRM sharedInstance] convertCountInterval:countInterval]];
        
    }
    
    void _setEventUploadTimeInterval(int timeInterval) {
        [[AdBrixRM sharedInstance] setEventUploadTimeInterval:[[AdBrixRM sharedInstance] convertTimeInterval:timeInterval]];
    }
    
    void _deepLinkOpenWithUrl(const char* url) {
        NSLog(@"AdBrixRM: _deepLinkOpenWithUrl :: %@", url);
        [[AdBrixRM sharedInstance] deepLinkOpenWithUrl:[NSURL URLWithString: [NSString stringWithUTF8String:url]]];
    }
    
    void _deepLinkOpenWithUrlAndDateStr(const char* url, const char* eventDateStr) {
        NSLog(@"AdBrixRM: _deepLinkOpenWithUrlAndDateStr");
        [[AdBrixRM sharedInstance] deepLinkOpenWithUrl:[NSURL URLWithString: [NSString stringWithUTF8String:url]] eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    void _setLocationWithLatitude(double latitude, double longitude) {
        [[AdBrixRM sharedInstance] setLocationWithLatitude:latitude longitude:longitude];
    }
    
    void _setAgeWithInt(int age) {
        [[AdBrixRM sharedInstance] setAgeWithInt:age];
    }
    
    void _setGenderWithAdBrixGenderType(int adBrixGenderType) {
        [[AdBrixRM sharedInstance] setGenderWithAdBrixGenderType:[[AdBrixRM sharedInstance] convertGender:adBrixGenderType]];
    }
    
    void _setUserPropertiesWithDictionary(const char* key[], const char* value[], int count) {
        
        if (key != nil && value != nil && count > 0) {
            NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
            for(int i=0; i < count; i++) {
                [dictionary setObject:[NSString stringWithUTF8String:value[i]] forKey:[NSString stringWithUTF8String:key[i]]];
            }
            [[AdBrixRM sharedInstance] setUserPropertiesWithDictionary:dictionary];
        }
        
    }
    void _clearUserProperties() {
        [[AdBrixRM sharedInstance] clearUserProperties];
    }
    
    NSDictionary<NSString *, NSString *> * _getUserProperties() {
        return [[AdBrixRM sharedInstance] getUserProperties];
    }
    
    void _setAppleAdvertisingIdentifierRm(const char* appleAdvertisingIdentifier) {
        [[AdBrixRM sharedInstance] setAppleAdvertisingIdentifier:[NSString stringWithUTF8String:appleAdvertisingIdentifier]];
    }
    
    void _logout(){
        [[AdBrixRM sharedInstance] logout];
    }
    void _loginWithUserId(const char* userId) {
        [[AdBrixRM sharedInstance] loginWithUserId:[NSString stringWithUTF8String:userId]];
    }
    
    void _loginWithUserIdAndDateStr(const char* userId, const char* eventDateStr) {
        [[AdBrixRM sharedInstance] loginWithUserId:[NSString stringWithUTF8String:userId] eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    void _eventWithName(const char* eventName) {
        [[AdBrixRM sharedInstance] eventWithEventName:[NSString stringWithUTF8String:eventName]];
    }
    
    void _eventWithNameAndDateStr(const char* eventName, const char* eventDateStr) {
        [[AdBrixRM sharedInstance] eventWithEventName:[NSString stringWithUTF8String:eventName] eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    void _eventWithEventNameAndValue(const char* eventName, const char* key[], const char* value[], int count) {
        
        
        if (key != nil && value != nil && count > 0) {
            NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
            for(int i=0; i < count; i++) {
                [dictionary setObject:[NSString stringWithUTF8String:value[i]] forKey:[NSString stringWithUTF8String:key[i]]];
            }
            [[AdBrixRM sharedInstance] eventWithEventName:[NSString stringWithUTF8String:eventName] value:dictionary];
        }
        
        
        
    }
    
    void _eventWithEventNameAndValueAndDateStr(const char* eventName, const char* key[], const char* value[], const char* eventDateStr, int count) {
        
        if (key != nil && value != nil && count > 0) {
            NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
            for(int i=0; i < count; i++) {
                [dictionary setObject:[NSString stringWithUTF8String:value[i]] forKey:[NSString stringWithUTF8String:key[i]]];
            }
            [[AdBrixRM sharedInstance] eventWithEventName:[NSString stringWithUTF8String:eventName] value:dictionary eventDateStr:[NSString  stringWithUTF8String:eventDateStr]];
        }
        
        
    }
    
    // MARK: - Commerce Event
    
    void _commerceViewHome() {
        [[AdBrixRM sharedInstance] commerceViewHome];
    }
    
    
    void _commerceViewHomeWithDateStr(const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commerceViewHomeWithEventDateStr:date];
    }
    
    
    void _commerceViewHomeWithExtraAttr(const char* jsonCommerceExtraAttrString) {
        printf("=== commerceExtraAttr json:: %s", jsonCommerceExtraAttrString);
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        //[NSString stringWithUTF8String:eventDateStr]
        [[AdBrixRM sharedInstance] commerceViewHomeWithOrderAttr:commerceExtraAttr];
    }
    
    
    void _commerceViewHomeWithExtraAttrAndDateStr(const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceViewHomeWithOrderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    void _commerceCategoryViewWithCategoryAndProduct(const char* categoryString, const char* jsonDataString) {
        
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray];
    }
    
    void _commerceCategoryViewBulkWithCategoryAndProduct(const char* categoryString, const char* jsonDataString) {
        
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray];
    }
    
    void _commerceCategoryViewWithCategoryAndProductAndDateStr(const char* categoryString, const char* jsonDataString, const char* eventDateStr) {
        
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray eventDateStr: date];
        
    }
    
    void _commerceCategoryViewBulkWithCategoryAndProductAndDateStr(const char* categoryString, const char* jsonDataString, const char* eventDateStr) {
        
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray eventDateStr: date];
        
    }
    
    
    void _commerceCategoryViewWithCategoryAndProductAndExtraAttr(const char* categoryString, const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray orderAttr:commerceExtraAttr];
    }
    
    void _commerceCategoryViewBulkWithCategoryAndProductAndExtraAttr(const char* categoryString, const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray orderAttr:commerceExtraAttr];
    }
    
    void _commerceCategoryViewWithCategoryAndProductAndExtraAttrAndDateStr(const char* categoryString, const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray orderAttr:commerceExtraAttr eventDateStr: date];
    }
    
    void _commerceCategoryViewBulkWithCategoryAndProductAndExtraAttrAndDateStr(const char* categoryString, const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductCategoryModel *cate = [AdBrixRmBridge makeCategoryFromStringForCommerce:[NSString stringWithUTF8String:categoryString]];
        
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCategoryViewWithCategory:cate productInfo:productArray orderAttr:commerceExtraAttr eventDateStr: date];
    }
    
    
    void _commerceProductViewWithProduct(const char* jsonDataString) {
        [[AdBrixRM sharedInstance] commerceProductViewWithProductInfo:[AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]]];
    }
    
    void _commerceProductViewWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceProductViewWithProductInfo:productModel eventDateStr:date];
        
    }
    
    void _commerceProductViewWithProductAndExtraAttr(const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceProductViewWithProductInfo:productModel orderAttr:commerceExtraAttr];
        
    }
    
    
    void _commerceProductViewWithProductAndExtraAttrAndDateStr(const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceProductViewWithProductInfo:productModel orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    
    
    void _commerceAddToCartWithProduct(const char* jsonDataString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray];
    }
    
    void _commerceAddToCartBulkWithProduct(const char* jsonDataString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray];
    }
    
    
    
    void _commerceAddToCartWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray eventDateStr:date];
    }
    
    void _commerceAddToCartBulkWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray eventDateStr:date];
    }
    
    
    
    void _commerceAddToCartWithProductAndExtraAttr(const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray orderAttr:commerceExtraAttr];
    }
    
    void _commerceAddToCartBulkWithProductAndExtraAttr(const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray orderAttr:commerceExtraAttr];
    }
    
    
    
    
    void _commerceAddToCartWithProductAndExtraAttrAndDateStr(const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    void _commerceAddToCartBulkWithProductAndExtraAttrAndDateStr(const char* jsonDataString,  const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceAddToCartWithProductInfo:productArray orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    
    void _commerceAddToWishListWithProduct(const char* jsonDataString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceAddToWishListWithProductInfo:productModel];
    }
    
    void _commerceAddToWishListWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceAddToWishListWithProductInfo:productModel eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    
    void _commerceAddToWishListWithProductAndExtraAttr(const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceAddToWishListWithProductInfo:productModel orderAttr:commerceExtraAttr];
    }
    
    void _commerceAddToWishListWithProductAndExtraAttrAndDateStr(const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceAddToWishListWithProductInfo:productModel orderAttr: commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    void _commerceReviewOrderWithOrderId(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge];
    }
    
    void _commerceReviewOrderBulkWithOrderId(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge];
    }
    
    void _commerceReviewOrderWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge eventDateStr:date];
    }
    
    
    void _commerceReviewOrderBulkWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge eventDateStr:date];
    }
    
    
    
    void _commerceReviewOrderWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge orderAttr:commerceExtraAttr];
    }
    
    
    void _commerceReviewOrderBulkWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* jsonCommerceExtraAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge orderAttr:commerceExtraAttr];
    }
    
    
    
    void _commerceReviewOrderWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge orderAttr: commerceExtraAttr eventDateStr:date];
    }
    
    
    void _commerceReviewOrderBulkWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceReviewOrderWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    void _commerceRefundWithOrderId(const char* orderId, const char* jsonDataString, double penaltyCharge) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge];
    }
    
    void _commerceRefundBulkWithOrderId(const char* orderId, const char* jsonDataString, double penaltyCharge) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge];
        
    }
    
    
    void _commerceRefundWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    
    void _commerceRefundBulkWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    //
    void _commerceRefundWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge orderAttr:commerceExtraAttr];
    }
    
    
    void _commerceRefundBulkWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* jsonCommerceExtraAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge orderAttr:commerceExtraAttr];
        
    }
    
    void _commerceRefundWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    void _commerceRefundBulkWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double penaltyCharge, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceRefundWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray penaltyCharge:penaltyCharge orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    void _commerceSearchWithProduct(const char* jsonDataString, const char* keyword) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword]];
    }
    
    void _commerceSearchBulkWithProduct(const char* jsonDataString, const char* keyword) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword]];
    }
    
    void _commerceSearchWithProductAndDateStr(const char* jsonDataString, const char* keyword, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] eventDateStr:date];
    }
    
    
    void _commerceSearchBulkWithProductAndDateStr(const char* jsonDataString, const char* keyword, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] eventDateStr:date];
    }
    
    
    void _commerceSearchWithProductAndExtraAttr(const char* jsonDataString, const char* keyword, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] orderAttr: commerceExtraAttr];
    }
    
    
    void _commerceSearchBulkWithProductAndExtraAttr(const char* jsonDataString, const char* keyword, const char* jsonCommerceExtraAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] orderAttr:commerceExtraAttr];
    }
    
    
    void _commerceSearchWithProductAndExtraAttrAndDateStr(const char* jsonDataString, const char* keyword, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    void _commerceSearchBulkWithProductAndExtraAttrAndDateStr(const char* jsonDataString, const char* keyword, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceSearchWithProductInfo:productArray keyword:[NSString stringWithUTF8String:keyword] orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    void _commerceShareWithChannel(int channel, const char* jsonDataString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        
        [[AdBrixRM sharedInstance] commerceShareWithChannel:[[AdBrixRM sharedInstance] convertChannel:channel] productInfo:productModel];
    }
    
    void _commerceShareWithChannelAndDateStr(int channel, const char* jsonDataString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceShareWithChannel:[[AdBrixRM sharedInstance] convertChannel:channel] productInfo:productModel eventDateStr:date];
    }
    
    
    
    void _commerceShareWithChannelAndExtraAttr(int channel, const char* jsonDataString, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commerceShareWithChannel:[[AdBrixRM sharedInstance] convertChannel:channel] productInfo:productModel orderAttr:commerceExtraAttr];
    }
    
    void _commerceShareWithChannelAndExtraAttrAndDateStr(int channel, const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceShareWithChannel:[[AdBrixRM sharedInstance] convertChannel:channel] productInfo:productModel orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    
    
    
    
    
    
    void _commerceListViewWithProduct(const char* jsonDataString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray];
    }
    
    void _commerceListViewBulkWithProduct(const char* jsonDataString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray];
    }
    
    void _commerceListViewProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray eventDateStr:date];
    }
    
    
    void _commerceListViewBulkWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray eventDateStr:date];
    }
    
    
    void _commerceListViewProductAndOrderAttr(const char* jsonDataString, const char* jsonOrderAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray orderAttr:orderAttr];
    }
    
    
    void _commerceListViewBulkWithProductAndOrderAttr(const char* jsonDataString, const char* jsonOrderAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray orderAttr:orderAttr];
    }
    
    
    void _commerceListViewProductAndOrderAttrAndDateStr(const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    void _commerceListViewBulkWithProductAndOrderAttrAndDateStr(const char* jsonDataString, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceListViewWithProductInfo:productArray orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    
    void _commerceCartViewWithProduct(const char* jsonDataString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray];
    }
    
    void _commerceCartViewBulkWithProduct(const char* jsonDataString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray];
    }
    
    void _commerceCartViewProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray eventDateStr:date];
    }
    
    
    void _commerceCartViewBulkWithProductAndDateStr(const char* jsonDataString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray eventDateStr:date];
    }
    
    
    void _commerceCartViewProductAndOrderAttr(const char* jsonDataString, const char* jsonOrderAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray orderAttr:orderAttr];
    }
    
    
    void _commerceCartViewBulkWithProductAndOrderAttr(const char* jsonDataString, const char* jsonOrderAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray orderAttr:orderAttr];
    }
    
    
    void _commerceCartViewProductAndOrderAttrAndDateStr(const char* jsonDataString, const char* jsonOrderAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray orderAttr:orderAttr eventDateStr:date];
    }
    
    
    void _commerceCartViewBulkWithProductAndOrderAttrAndDateStr(const char* jsonDataString, const char* jsonOrderAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *orderAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonOrderAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commerceCartViewWithProductInfo:productArray orderAttr:orderAttr eventDateStr:date];
    }
    
    
    
    void _commercePaymentInfoAddedWithExtraAttr(const char* jsonCommerceExtraAttrString) {
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        [[AdBrixRM sharedInstance] commercePaymentInfoAddedWithPaymentInfoAttr:commerceExtraAttr];
    }
    
    
    void _commercePaymentInfoAddedWithExtraAttrAndDateStr(const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commercePaymentInfoAddedWithPaymentInfoAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    
    void _gameLevelAchievedWithLevel(int level) {
        [[AdBrixRM sharedInstance] gameLevelAchievedWithLevel:level];
    }
    
    void _gameLevelAchievedWithLevelAndDateStr(int level, const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameLevelAchedvedWithLevel:level eventDateStr:date];
    }
    
    void _gameLevelAchievedWithLevelWithGameAttr(int level, const char* jsonGameAttrString) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        [[AdBrixRM sharedInstance] gameLevelAchievedWithLevel:level gameInfoAttr:gameAttr];
    }
    
    void _gameLevelAchievedWithLevelWithGameAttrAndDateStr(int level, const char* jsonGameAttrString, const char* eventDateStr) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameLevelAchievedWithLevel:level gameInfoAttr:gameAttr eventDateStr:date];
    }
    
    
    void _gameTutorialCompletedWithIsSkip(BOOL isSkip) {
        [[AdBrixRM sharedInstance] gameTutorialCompletedWithIsSkip:isSkip];
    }
    
    void _gameTutorialCompletedWithIsSkipAndDateStr(BOOL isSkip, const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameTutorialCompletedWithIsSkip:isSkip eventDateStr:date];
    }
    
    void _gameTutorialCompletedWithIsSkipAndGameAttr(BOOL isSkip, const char* jsonGameAttrString) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        [[AdBrixRM sharedInstance] gameTutorialCompletedWithIsSkip:isSkip gameInfoAttr:gameAttr];
    }
    
    void _gameTutorialCompletedWithIsSkipAndGameAttrAndDateStr(BOOL isSkip, const char* jsonGameAttrString, const char* eventDateStr) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameTutorialCompletedWithIsSkip:isSkip gameInfoAttr:gameAttr eventDateStr:date];
    }
    
    
    
    void _gameCharacterCreated() {
        [[AdBrixRM sharedInstance] gameCharacterCreated];
    }
    
    void _gameCharacterCreatedWithDateStr(const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameCharacterCreatedWithEventDateStr:date];
    }
    
    void _gameCharacterCreatedWithGameAttr(const char* jsonGameAttrString) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        [[AdBrixRM sharedInstance] gameCharacterCreatedWithGameInfoAttr:gameAttr];
    }
    
    void _gameCharacterCreatedWithGameAttrAndDateStr(const char* jsonGameAttrString, const char* eventDateStr) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameCharacterCreatedWithGameInfoAttr:gameAttr eventDateStr:date];
    }
    
    
    
    
    void _gameStageClearedWithStageName(const char* stageName) {
        [[AdBrixRM sharedInstance] gameStageClearedWithStageName:[NSString stringWithUTF8String:stageName]];
    }
    
    void _gameStageClearedWithStageNameAndDateStr(const char* stageName, const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameStageClearedWithStageName:[NSString stringWithUTF8String:stageName] eventDateStr:date];
    }
    
    void _gameStageClearedWithStageNameAndGameAttr(const char* stageName, const char* jsonGameAttrString) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        [[AdBrixRM sharedInstance] gameStageClearedWithStageName:[NSString stringWithUTF8String:stageName] gameInfoAttr:gameAttr];
    }
    
    void _gameStageClearedWithStageNameAndGameAttrAndDateStr(const char* stageName, const char* jsonGameAttrString, const char* eventDateStr) {
        NSMutableDictionary *gameAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonGameAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] gameStageClearedWithStageName:[NSString stringWithUTF8String:stageName] gameInfoAttr:gameAttr eventDateStr:date];
    }
    
    
    
    void _commonPurchaseWithOrderId(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod]];
    }
    
    void _commonPurchaseBulkWithOrderId(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge,int paymentMethod) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod]];
    }
    
    
    void _commonPurchaseWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod] eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    
    void _commonPurchaseBulkWithOrderIdAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod] eventDateStr:[NSString stringWithUTF8String:eventDateStr]];
    }
    
    
    void _commonPurchaseWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* jsonCommerceExtraAttrString) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod] orderAttr:commerceExtraAttr];
    }
    
    void _commonPurchaseBulkWithOrderIdAndExtraAttr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* jsonCommerceExtraAttrString) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod]  orderAttr:commerceExtraAttr];
    }
    
    
    void _commonPurchaseWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        AdBrixRmCommerceProductModel *productModel = [AdBrixRmBridge makeProductFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [[NSArray alloc] initWithObjects:productModel, nil];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod] orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    void _commonPurchaseBulkWithOrderIdAndExtraAttrAndDateStr(const char* orderId, const char* jsonDataString, double discount, double deliveryCharge, int paymentMethod, const char* jsonCommerceExtraAttrString, const char* eventDateStr) {
        NSArray<AdBrixRmCommerceProductModel *> *productArray = [AdBrixRmBridge makeProductsFromJsonForCommerce:[NSString stringWithUTF8String:jsonDataString]];
        NSMutableDictionary *commerceExtraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:jsonCommerceExtraAttrString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commonPurchaseWithOrderId:[NSString stringWithUTF8String:orderId] productInfo:productArray discount:discount deliveryCharge:deliveryCharge paymentMethod:[[AdBrixRM sharedInstance] convertPayment:paymentMethod] orderAttr:commerceExtraAttr eventDateStr:date];
    }
    
    
    //sign_up
    void _commonSignUp(int channel) {
        [[AdBrixRM sharedInstance] commonSignUpWithChannel:[[AdBrixRM sharedInstance] convertSignUpChannel:channel]];
    }
    
    void _commonSignUpWithDateStr(int channel, const char* eventDateStr) {
        
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commonSignUpWithChannel:[[AdBrixRM sharedInstance] convertSignUpChannel:channel] eventDateStr:date];
    }
    
    void _commonSignUpWithAttr(int channel, const char* extraAttrJsonString) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        [[AdBrixRM sharedInstance] commonSignUpWithChannel:[[AdBrixRM sharedInstance] convertSignUpChannel:channel] commonAttr:extraAttr];
    }
    
    
    void _commonSignUpWithAttrAndDateStr(int channel, const char* extraAttrJsonString, const char* eventDateStr) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commonSignUpWithChannel:[[AdBrixRM sharedInstance] convertSignUpChannel:channel] commonAttr:extraAttr eventDateStr:date];
    }
    
    
    
    //use_credit
    void _commonUseCredit() {
        [[AdBrixRM sharedInstance] commonUseCredit];
    }
    
    void _commonUseCreditWithDateStr(const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commonUseCreditWithEventDateStr:date];
    }
    
    void _commonUseCreditWithAttr(const char* extraAttrJsonString) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        [[AdBrixRM sharedInstance] commonUseCreditWithCommonAttr:extraAttr];
    }
    
    void _commonUseCreditWithAttrAndDateStr(const char* extraAttrJsonString, const char* eventDateStr) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance] commonUseCreditWithCommonAttr:extraAttr eventDateStr:date];
        
    }
    
    
    
    //app_update
    void _commonAppUpdate(const char* prev_ver, const char* curr_ver) {
        [[AdBrixRM sharedInstance] commonAppUpdateWithPrev_ver:[NSString stringWithUTF8String:prev_ver] curr_ver:[NSString stringWithUTF8String:curr_ver]];
    }
    
    void _commonAppUpdateWithDateStr(const char* prev_ver, const char* curr_ver, const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        
        [[AdBrixRM sharedInstance]
         commonAppUpdateWithPrev_ver:[NSString stringWithUTF8String:prev_ver]
         curr_ver:[NSString stringWithUTF8String:curr_ver]
         eventDateStr:date];
    }
    
    void _commonAppUpdateWithAttr(const char* prev_ver, const char* curr_ver, const char* extraAttrJsonString) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        [[AdBrixRM sharedInstance]
         commonAppUpdateWithPrev_ver:[NSString stringWithUTF8String:prev_ver]
         curr_ver:[NSString stringWithUTF8String:curr_ver]
         commonAttr:extraAttr];
    }
    
    
    void _commonAppUpdateWithAttrAndDateStr(const char* prev_ver, const char* curr_ver, const char*extraAttrJsonString, const char* eventDateStr) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance]
         commonAppUpdateWithPrev_ver:[NSString stringWithUTF8String:prev_ver]
         curr_ver:[NSString stringWithUTF8String:curr_ver]
         commonAttr:extraAttr
         eventDateStr:date];
    }
    
    
    //invite
    void _commonInvite(int channel) {
        [[AdBrixRM sharedInstance] commonInviteWithChannel:[[AdBrixRM sharedInstance] convertInviteChannel:channel]];
    }
    
    void _commonInviteWithDateStr(int channel, const char* eventDateStr) {
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commonInviteWithChannel:[[AdBrixRM sharedInstance] convertInviteChannel:channel] eventDateStr:date];
    }
    
    void _commonInviteWithAttr(int channel, const char* extraAttrJsonString) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        [[AdBrixRM sharedInstance] commonInviteWithChannel:[[AdBrixRM sharedInstance] convertInviteChannel:channel] commonAttr:extraAttr];
    }
    
    void _commonInviteWithAttrAndDateStr(int channel, const char* extraAttrJsonString, const char* eventDateStr) {
        NSMutableDictionary *extraAttr = [AdBrixRmBridge makeExtraAttrDictionaryFromJson:[NSString stringWithUTF8String:extraAttrJsonString]];
        NSString *date = [NSString stringWithUTF8String:eventDateStr];
        [[AdBrixRM sharedInstance] commonInviteWithChannel:[[AdBrixRM sharedInstance] convertInviteChannel:channel] commonAttr:extraAttr eventDateStr:date];
    }
    
    
    const char* _AdBrixCurrencyNameRm (int currency)
    {
        NSString* str = [[AdBrixRM sharedInstance] getCurrencyString:currency];
        
        char* res = (char*)malloc(str.length+1);
        strcpy(res, [str UTF8String]);
        
        return res;
    }
    
    const char* _AdBrixPaymentMethodNameRm (int method)
    {
        NSString* str = [[AdBrixRM sharedInstance] getPaymentMethod:method];
        
        char* res = (char*)malloc(str.length+1);
        strcpy(res, [str UTF8String]);
        
        return res;
    }
    
    const char* _AdBrixSharingChannelNameRm (int channel)
    {
        NSString* str = [[AdBrixRM sharedInstance] getSharingChannel:channel];
        
        char* res = (char*)malloc(str.length+1);
        strcpy(res, [str UTF8String]);
        
        return res;
    }
    
    const char* _AdBrixSignUpChannelName (int channel)
    {
        NSString* str = [[AdBrixRM sharedInstance] getSignUpChannel:channel];
        
        char* res = (char*)malloc(str.length+1);
        strcpy(res, [str UTF8String]);
        
        return res;
    }
    
    const char* _AdBrixInviteChannelName (int channel)
    {
        NSString* str = [[AdBrixRM sharedInstance] getInviteChannel:channel];
        
        char* res = (char*)malloc(str.length+1);
        strcpy(res, [str UTF8String]);
        
        return res;
    }
    
    
}


@end
