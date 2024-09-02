package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.Optional;

@Service
public class AuthServiceImpl implements AuthService {

    private final UserRepository userRepository;

    public AuthServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public void registerUser(AuthUserDto dto) {
        User user = new User();
        user.setEmail(dto.getEmail());
        user.setUsername(dto.getUsername());
        user.setPassword(dto.getPassword());
        user.setCreatedAt(new Timestamp(System.currentTimeMillis()));
        user.setIsActive(true);
        userRepository.save(user);
    }

    @Override
    public void loginUser(AuthUserDto dto) {
        Optional<User> user = userRepository.findByEmail(dto.getEmail());
        if (user.isEmpty()) {
            throw new NotFoundElementException("이메일의 사용자를 찾을 수 없습니다");
        } else {
            System.out.println("사용자 : " + user);
            if (dto.getPassword().equals(user.get().getPassword())) {
                // 로그인 성공
            } else {
                throw new IllegalArgumentException("잘못된 패스워드 입니다.");
            }
        }
    }
}
