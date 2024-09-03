package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.api.exception.InvalidDataException;
import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.Optional;

@Service
public class AuthServiceImpl implements AuthService {

    private final UserRepository userRepository;
    private final HttpSession session;

    public AuthServiceImpl(UserRepository userRepository, HttpSession session) {
        this.userRepository = userRepository;
        this.session = session;
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

        if (user.isEmpty() || !user.get().getPassword().equals(dto.getPassword())) {
            throw new NotFoundElementException("이메일 또는 비밀번호가 잘못되었습니다.");
        }

        if (!user.get().getIsActive()) {
            throw new InvalidDataException("비활성화된 사용자 입니다.");
        }

        session.setAttribute("user", user.get());
        System.out.println("사용자 : " + user);
    }
}
