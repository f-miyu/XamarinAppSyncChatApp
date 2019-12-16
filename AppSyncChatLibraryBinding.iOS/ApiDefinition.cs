using System;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace NativeLibrary
{
    // @interface AppSyncChatClient : NSObject
    [BaseType(typeof(NSObject))]
    interface AppSyncChatClient
    {
        // -(void)createMessageWithChatId:(NSString * _Nonnull)chatId text:(NSString * _Nonnull)text userId:(NSString * _Nonnull)userId onResult:(void (^ _Nonnull)(Message * _Nullable))onResult onError:(void (^ _Nonnull)(NSError * _Nonnull))onError;
        [Export("createMessageWithChatId:text:userId:onResult:onError:")]
        void CreateMessageWithChatId(string chatId, string text, string userId, Action<Message> onResult, Action<NSError> onError);

        // -(Subscription * _Nullable)onCreateMessageWithChatId:(NSString * _Nonnull)chatId error:(NSError * _Nullable * _Nullable)error onResult:(void (^ _Nonnull)(Message * _Nullable))onResult onError:(void (^ _Nonnull)(NSError * _Nonnull))onError __attribute__((warn_unused_result));
        [Export("onCreateMessageWithChatId:error:onResult:onError:")]
        [return: NullAllowed]
        Subscription OnCreateMessageWithChatId(string chatId, [NullAllowed] out NSError error, Action<Message> onResult, Action<NSError> onError);
    }

    // @interface Message : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface Message
    {
        // @property (readonly, copy, nonatomic) NSString * _Nonnull id;
        [Export("id")]
        string Id { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nonnull chatId;
        [Export("chatId")]
        string ChatId { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nonnull text;
        [Export("text")]
        string Text { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nonnull userId;
        [Export("userId")]
        string UserId { get; }
    }

    // @interface Subscription : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface Subscription
    {
        // -(void)cancel;
        [Export("cancel")]
        void Cancel();
    }
}

