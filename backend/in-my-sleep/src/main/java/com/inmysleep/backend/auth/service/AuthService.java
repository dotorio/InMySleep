package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.dto.UserInfoDto;

public interface AuthService {
    void registerUser(AuthUserDto user);
    UserInfoDto loginUser(AuthUserDto user);
}
