package com.example.appsyncchatlibrary

interface Callback<T> {
    fun onResponse(response: T)
    fun onFailure(e: Exception)
}