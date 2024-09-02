package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.stereotype.Service;

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
}
