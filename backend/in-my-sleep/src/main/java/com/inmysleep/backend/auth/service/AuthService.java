package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.dto.UserInfo;

public interface AuthService {
    void registerUser(AuthUserDto user);
    UserInfo loginUser(AuthUserDto user);
}
