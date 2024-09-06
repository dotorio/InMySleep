package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.dto.UserInfoDto;

public interface UserService {
    boolean isEmailAlreadyInUse(String email);
    boolean isUsernameAlreadyInUse(String username);
    UserInfoDto getUserInfo(int id);
}
