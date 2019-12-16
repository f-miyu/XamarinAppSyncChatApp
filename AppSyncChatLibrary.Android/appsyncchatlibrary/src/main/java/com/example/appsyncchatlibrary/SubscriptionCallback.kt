package com.example.appsyncchatlibrary

interface SubscriptionCallback<T> {
    fun onResponse(response: T)
    fun onFailure(e: Exception)
    fun onCompleted();
}