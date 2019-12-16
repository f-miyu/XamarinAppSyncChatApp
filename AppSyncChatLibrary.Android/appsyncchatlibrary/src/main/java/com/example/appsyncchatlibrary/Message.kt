package com.example.appsyncchatlibrary

data class Message(
    val id: String,
    val chatId: String,
    val text: String,
    val userId: String
)