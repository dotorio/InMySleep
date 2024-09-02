package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.auth.dto.AuthRegisterDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;

@Service
public class AuthServiceImpl implements AuthService {

    private final UserRepository userRepository;

    public AuthServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public void registerUser(AuthRegisterDto dto) {
        User user = new User();
        user.setEmail(dto.getEmail());
        user.setUsername(dto.getUsername());
        user.setPassword(dto.getPassword());
        user.setCreatedAt(new Timestamp(System.currentTimeMillis()));
        user.setIsActive(true);
        userRepository.save(user);
    }
}
