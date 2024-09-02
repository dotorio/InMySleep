package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthRegisterDto;

public interface AuthService {
    void registerUser(AuthRegisterDto user);
}
