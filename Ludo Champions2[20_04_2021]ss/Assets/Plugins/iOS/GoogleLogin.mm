/*
 * Copyright (C) 2011 Keijiro Takahashi
 * Copyright (C) 2012 GREE, Inc.
 *
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

#import <UIKit/UIKit.h>
#import <GoogleSignIn/GoogleSignIn.h>

// NOTE: we need extern without "C" before unity 4.5
//extern UIViewController *UnityGetGLViewController();
extern "C" UIViewController *UnityGetGLViewController();
extern "C" void UnitySendMessage(const char *, const char *, const char *);


@interface DCGoogleSignIn : NSObject <GIDSignInUIDelegate, GIDSignInDelegate>

@property (retain, nonatomic) GIDGoogleUser *cUser;
@property (retain, nonatomic) NSString *gameObjectName;

+(DCGoogleSignIn*)manager:(const char *)gameObjectName_;

-(void)login;
-(void)logout;

@end

@implementation DCGoogleSignIn

static DCGoogleSignIn *commonManager = nil;

+(DCGoogleSignIn*)manager:(const char *)gameObjectName_ {
    
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        
        commonManager = [[self alloc] init];
        
        NSDictionary *credentials = [NSDictionary dictionaryWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"credentials" ofType:@"plist"]];
        
        [GIDSignIn sharedInstance].clientID = credentials[@"CLIENT_ID"];
        
        //[GIDSignIn sharedInstance].clientID = @"649685272055-0ciuiavj4g8okgfq0jpec26ftbq32d26.apps.googleusercontent.com";
        
        [GIDSignIn sharedInstance].uiDelegate = commonManager;
        [GIDSignIn sharedInstance].delegate = commonManager;
    });
    
    commonManager.gameObjectName = [NSString stringWithUTF8String:gameObjectName_];
    
    return commonManager;
}


- (void)signIn:(GIDSignIn *)signIn didSignInForUser:(GIDGoogleUser *)user withError:(NSError *)error {
    
    if (error) {
        
        NSDictionary *eDict = @{@"error" : error.description,
                                @"event_name" : @"g_signup"};
        
        [commonManager callBackWithData:eDict];
        return;
    }
    
    
    commonManager.cUser = user;
    
    NSString *userId = user.userID;
    NSString *displyName = user.profile.name;
    NSString *givenName = user.profile.givenName;
    NSString *familyName = user.profile.familyName;
    NSString *email = user.profile.email;
    NSString *photoUrl = [user.profile imageURLWithDimension:600].absoluteString;
    NSArray *grantedScopes = user.grantedScopes;
    
    NSMutableDictionary *dict = [NSMutableDictionary new];
    dict[@"email"] = email;
    dict[@"displyName"] = displyName;
    dict[@"givenName"] = givenName;
    dict[@"familyName"] = familyName;
    dict[@"photoUrl"] = photoUrl;
    dict[@"id"] = userId;
    dict[@"grantedScopes"] = grantedScopes;
    dict[@"event_name"] = @"g_signup";
    
    [commonManager callBackWithData:dict];
    
    NSLog(@"%@", dict);
}


- (void)signIn:(GIDSignIn *)signIn didDisconnectWithUser:(GIDGoogleUser *)user withError:(NSError *)error {
    
    if (error) {
        
        NSDictionary *eDict = @{@"error" : error.description,
                                @"status" : @(NO),
                                @"event_name" : @"g_signout"};
        
        [commonManager callBackWithData:eDict];
        
        return;
    }
    
    NSDictionary *dict = @{@"status" : @(YES),
                           @"event_name" : @"g_signout"};
    
    [commonManager callBackWithData:dict];
}


-(void)login {
    
    [[GIDSignIn sharedInstance] signIn];
}


-(void)logout {
    
    [[GIDSignIn sharedInstance] disconnect];
}

-(void)signInWillDispatch:(GIDSignIn *)signIn error:(NSError *)error {
    
}

// Present a view that prompts the user to sign in with Google
- (void)signIn:(GIDSignIn *)signIn presentViewController:(UIViewController *)viewController {
    
    UIWindow *appD = [UIApplication sharedApplication].keyWindow;
    [appD.rootViewController presentViewController:viewController animated:YES completion:nil];
}

// Dismiss the "Sign in with Google" view
- (void)signIn:(GIDSignIn *)signIn dismissViewController:(UIViewController *)viewController {
    NSDictionary *dict = @{@"event_name" : @"dismiss"};
	[commonManager callBackWithData:dict];

    UIWindow *appD = [UIApplication sharedApplication].keyWindow;
    [appD.window.rootViewController dismissViewControllerAnimated:YES completion:nil];
}

-(void)callBackWithData:(NSDictionary*)dict {
    
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:nil];
    NSString *jText = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    UnitySendMessage([commonManager.gameObjectName UTF8String], "IOSCallback", [jText UTF8String]);
}

@end

extern "C" {
    void _LoginGoogle(const char *gameObjectName);
    void _LogoutGoogle(const char *gameObjectName);
}


void _LoginGoogle(const char *gameObjectName)
{
    DCGoogleSignIn *instance = [DCGoogleSignIn manager:gameObjectName];
    [instance login];
}

void _LogoutGoogle(const char *gameObjectName)
{
    DCGoogleSignIn *instance = [DCGoogleSignIn manager:gameObjectName];
    [instance logout];
}

