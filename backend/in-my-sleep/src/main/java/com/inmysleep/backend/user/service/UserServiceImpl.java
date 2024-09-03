package com.inmysleep.backend.user.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.user.dto.UserInfo;
import com.inmysleep.backend.user.entity.User;
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

    @Override
    public UserInfo getUserInfo(int id) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new NotFoundElementException("User not found"));

        UserInfo userInfo = new UserInfo();
        userInfo.setUsername(user.getUsername());
        userInfo.setEmail(user.getEmail());

        return userInfo;
    }

}
