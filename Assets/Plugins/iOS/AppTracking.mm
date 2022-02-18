#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import "UnityInterface.h"

extern "C" {
     void _RequestTrackingAuthorizationWithCompletionHandler() {
         if (@available(iOS 14, *)) {
             [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                 NSString *stringInt = [NSString stringWithFormat:@"%lu",(unsigned long)status];
                 const char* charStatus = [stringInt UTF8String];
                 UnitySendMessage("IOSMethod", "GetAuthorizationStatus", charStatus);
             }];
         } else {
             UnitySendMessage("IOSMethod", "GetAuthorizationStatus", "-1");
         }
     }
    
     int _GetAppTrackingAuthorizationStatus() {
         if (@available(iOS 14, *)) {
             return (int)[ATTrackingManager trackingAuthorizationStatus];
         } else {
             return -1;
         }
     }
}

