package com.inmysleep.backend.user.service;

public interface UserService {
    boolean isEmailAlreadyInUse(String email);
    boolean isUsernameAlreadyInUse(String username);
}
