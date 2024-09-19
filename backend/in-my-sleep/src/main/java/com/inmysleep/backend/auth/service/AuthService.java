package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.dto.UserLoginDto;

public interface AuthService {
    void registerUser(AuthUserDto user);
    UserLoginDto loginUser(AuthUserDto user);
}
