package com.inmysleep.backend.auth.service;

import com.inmysleep.backend.api.exception.InvalidDataException;
import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.api.security.JwtTokenProvider;
import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.game.service.EasterEggService;
import com.inmysleep.backend.game.service.EasterEggServiceImpl;
import com.inmysleep.backend.user.dto.UserLoginDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import jakarta.servlet.http.HttpSession;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
public class AuthServiceImpl implements AuthService {

    private final UserRepository userRepository;
    private final HttpSession session;
    private final PasswordEncoder passwordEncoder;
    private final JwtTokenProvider jwtTokenProvider;
    private final EasterEggService easterEggService;

    public AuthServiceImpl(UserRepository userRepository, EasterEggService easterEggService,
                           HttpSession session,
                           PasswordEncoder passwordEncoder,
                           JwtTokenProvider jwtTokenProvider, EasterEggService easterEggService1) {
        this.userRepository = userRepository;
        this.session = session;
        this.passwordEncoder = passwordEncoder;
        this.jwtTokenProvider = jwtTokenProvider;
        this.easterEggService = easterEggService1;
    }

    @Override
    public void registerUser(AuthUserDto dto) {
        User user = new User();
        user.setEmail(dto.getEmail());
        user.setUsername(dto.getUsername());

        // 비밀번호 암호화
        user.setPassword(passwordEncoder.encode(dto.getPassword()));

        user.setCreatedAt(LocalDateTime.now());
        user.setIsActive(true);

        User savedUser = userRepository.save(user);

        // 기본 스킨 정보 추가
        easterEggService.defaultEasterEgg(savedUser.getUserId());
        
        // 기본 스킨 장착 설정
        easterEggService.setDefaultEasterEgg(savedUser.getUserId());
    }

    @Override
    public UserLoginDto loginUser(AuthUserDto dto) {
        Optional<User> user = userRepository.findByEmail(dto.getEmail());

        if (user.isEmpty() || !passwordEncoder.matches(dto.getPassword(), user.get().getPassword())) {
            throw new NotFoundElementException("이메일 또는 비밀번호가 잘못되었습니다.");
        }

        if (!user.get().getIsActive()) {
            throw new InvalidDataException("비활성화된 사용자 입니다.");
        }

        // JWT 토큰 생성
        String token = jwtTokenProvider.createToken(user.get().getEmail());

        session.setAttribute("user", user.get());

        UserLoginDto userInfo = new UserLoginDto();
        userInfo.setUserId(user.get().getUserId());
        userInfo.setUsername(user.get().getUsername());
        userInfo.setEmail(user.get().getEmail());
        userInfo.setLastStage(user.get().getLastStage());
        userInfo.setToken(token);

        return userInfo;
    }
}
