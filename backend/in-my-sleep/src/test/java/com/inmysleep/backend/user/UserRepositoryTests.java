package com.inmysleep.backend.user;

import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import com.inmysleep.backend.user.service.UserService;
import jakarta.transaction.Transactional;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.context.SpringBootTest;

import java.time.LocalDateTime;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@AutoConfigureTestDatabase(replace = AutoConfigureTestDatabase.Replace.NONE)
public class UserRepositoryTests {

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private UserService userService;

    @Test
    @Transactional
    public void testSaveAndFindUser() {
        // 새로운 User 객체 생성
        User user = new User();
        user.setEmail("test@example.com");
        user.setUsername("testuser");
        user.setPassword("password123");
        user.setCreatedAt(LocalDateTime.now());
        user.setUpdatedAt(LocalDateTime.now());
        user.setLastLogin(LocalDateTime.now());
        user.setIsActive(true);

        // User 객체를 데이터베이스에 저장
        User savedUser = userRepository.save(user);

        // 저장된 User 객체가 반환된 userId를 가지고 있는지 확인
        assertThat(savedUser.getUserId()).isGreaterThan(0);

        // 데이터베이스에서 User 객체를 조회
        User foundUser = userRepository.findById(savedUser.getUserId()).orElse(null);

        // 콘솔에 출력
        System.out.println("Saved User: " + foundUser);

        // User 객체가 제대로 조회되었는지 확인
        assertThat(foundUser).isNotNull();
        assertThat(foundUser.getEmail()).isEqualTo(user.getEmail());
        assertThat(foundUser.getUsername()).isEqualTo(user.getUsername());
    }

    @Test
    @Transactional
    public void testEmailAlreadyInUse() {
        // given
        User existingUser = new User();
        existingUser.setEmail("test@example.com");
        existingUser.setUsername("existinguser");
        existingUser.setPassword("password123");
        userRepository.save(existingUser);

        AuthUserDto dto = new AuthUserDto();
        dto.setEmail("test@example.com");
        dto.setUsername("newuser");
        dto.setPassword("password123");

        // when
        boolean isEmailInUse = userService.isEmailAlreadyInUse(dto.getEmail());

        // then
        assertThat(isEmailInUse).isTrue();
    }

    @Test
    @Transactional
    public void testUsernameAlreadyInUse() {
        // given
        User existingUser = new User();
        existingUser.setEmail("unique@example.com");
        existingUser.setUsername("testuser");
        existingUser.setPassword("password123");
        userRepository.save(existingUser);

        AuthUserDto dto = new AuthUserDto();
        dto.setEmail("newuser@example.com");
        dto.setUsername("testuser");
        dto.setPassword("password123");

        // when
        boolean isUsernameInUse = userService.isUsernameAlreadyInUse(dto.getUsername());

        // then
        assertThat(isUsernameInUse).isTrue();
    }
}
