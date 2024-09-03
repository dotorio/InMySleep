package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.dto.UserInfo;

public interface UserService {
    boolean isEmailAlreadyInUse(String email);
    boolean isUsernameAlreadyInUse(String username);
    UserInfo getUserInfo(int id);
}
