package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.dto.UserRegisterDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;

@Service
public class UserServiceImpl implements UserService {

    private final UserRepository userRepository;

    public UserServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public boolean isEmailAlreadyInUse(String email) {
        return userRepository.existsByEmail(email);
    }

    @Override
    public boolean isUsernameAlreadyInUse(String username) {
        return userRepository.existsByUsername(username);
    }

    @Override
    public void registerUser(UserRegisterDto dto) {
        User user = new User();
        user.setEmail(dto.getEmail());
        user.setUsername(dto.getUsername());
        user.setPassword(dto.getPassword());
        user.setCreatedAt(new Timestamp(System.currentTimeMillis()));
        user.setIsActive(true);
        userRepository.save(user);
    }
}
