package com.example.appsyncchatlibrary

import android.content.Context
import com.amazonaws.amplify.generated.graphql.CreateMessageMutation
import com.amazonaws.amplify.generated.graphql.OnCreateMessageSubscription
import com.amazonaws.mobile.config.AWSConfiguration
import com.amazonaws.mobileconnectors.appsync.AWSAppSyncClient
import com.amazonaws.mobileconnectors.appsync.AppSyncSubscriptionCall
import com.apollographql.apollo.GraphQLCall
import com.apollographql.apollo.api.Response
import com.apollographql.apollo.exception.ApolloException
import type.CreateMessageInput

class AppSyncChatClient(context: Context) {
    private val client: AWSAppSyncClient = AWSAppSyncClient.builder()
        .context(context)
        .awsConfiguration(AWSConfiguration(context))
        .build()

    fun createMessage(chatId: String, text: String, userId: String, callback: Callback<Message?>?) {
        val input = CreateMessageInput.builder()
            .chatId(chatId)
            .text(text)
            .userId(userId)
            .build()
        val mutation = CreateMessageMutation.builder()
            .input(input)
            .build()

        client.mutate(mutation)
            .enqueue(object : GraphQLCall.Callback<CreateMessageMutation.Data>() {
                override fun onFailure(e: ApolloException) {
                    callback?.onFailure(e)
                }

                override fun onResponse(response: Response<CreateMessageMutation.Data>) {
                    val message = response.data()?.createMessage()?.let {
                        Message(
                            id = it.id(),
                            chatId = it.chatId(),
                            text = it.text(),
                            userId = it.userId()
                        )
                    }
                    callback?.onResponse(message)
                }
            })
    }

    fun onCreateMessage(chatId: String, callback: SubscriptionCallback<Message?>?): Cancelable {
        val subscription = OnCreateMessageSubscription.builder()
            .chatId(chatId)
            .build()
        val subscriptionWatcher = client.subscribe(subscription)
        subscriptionWatcher.execute(object :
            AppSyncSubscriptionCall.Callback<OnCreateMessageSubscription.Data> {
            override fun onFailure(e: ApolloException) {
                callback?.onFailure(e)
            }

            override fun onResponse(response: Response<OnCreateMessageSubscription.Data>) {
                val message = response.data()?.onCreateMessage()?.let {
                    Message(
                        id = it.id(),
                        chatId = it.chatId(),
                        text = it.text(),
                        userId = it.userId()
                    )
                }
                callback?.onResponse(message)
            }

            override fun onCompleted() {
                callback?.onCompleted()
            }
        })

        return object : Cancelable {
            override fun cancel() {
                subscriptionWatcher.cancel()
            }
        }
    }
}