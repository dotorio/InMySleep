package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthUserDto;

public interface AuthService {
    void registerUser(AuthUserDto user);
    void loginUser(AuthUserDto user);
}
