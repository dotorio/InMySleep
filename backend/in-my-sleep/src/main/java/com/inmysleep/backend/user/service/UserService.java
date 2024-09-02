package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.dto.UserRegisterDto;

public interface UserService {
    boolean isEmailAlreadyInUse(String email);
    boolean isUsernameAlreadyInUse(String username);
    void registerUser(UserRegisterDto user);
}
