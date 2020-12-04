//
//  AdPopcornSDKPlugin.h
//  IgaworksAd
//
//  Created by wonje,song on 2014. 1. 21..
//  Copyright (c) 2014년 wonje,song. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AdBrixRmBridge : NSObject <AdBrixRMDeeplinkDelegate, AdBrixRMDeferredDeeplinkDelegate>


@property (nonatomic, copy) NSString *callbackHandlerName;

+ (AdBrixRmBridge *)sharedAdBrixRmBridge;
- (void)setAdBrixDeeplinkDelegate;
- (void)setAdBrixDeferredDeeplinkDelegate;


@end
