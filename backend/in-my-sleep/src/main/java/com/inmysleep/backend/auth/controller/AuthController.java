package com.inmysleep.backend.auth.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.auth.service.AuthService;
import com.inmysleep.backend.user.dto.UserInfoDto;
import com.inmysleep.backend.user.service.UserService;
import jakarta.servlet.http.HttpSession;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
public class AuthController {

    private final AuthService authService;
    private final UserService userService;

    @Autowired
    public AuthController(AuthService authService, UserService userService) {
        this.authService = authService;
        this.userService = userService;
    }

    @PostMapping("/login")
    public ResponseEntity<ApiResponse<UserInfoDto>> login(@RequestBody AuthUserDto userDto) {
        ApiResponse<UserInfoDto> apiResponse = new ApiResponse<>();
        UserInfoDto userInfo = authService.loginUser(userDto);
        apiResponse.setResponseTrue(userInfo, "로그인 성공");
        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/logout")
    public ResponseEntity<ApiResponse<Void>> logout(HttpSession session) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        if (session != null) {
            session.invalidate();
            apiResponse.setResponseTrue(null, "로그아웃 성공");
        } else {
            apiResponse.setResponseFalse(null, "세션이 존재하지 않습니다.");
        }
        
        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/signup")
    public ResponseEntity<ApiResponse<Void>> signup(@Valid @RequestBody AuthUserDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        if (userService.isEmailAlreadyInUse(dto.getEmail())) {
            apiResponse.setResponseFalse(null, "이미 사용 중인 이메일입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        if (userService.isUsernameAlreadyInUse(dto.getUsername())) {
            apiResponse.setResponseTrue(null, "이미 사용 중인 닉네임입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        authService.registerUser(dto);
        apiResponse.setResponseTrue(null, "회원가입이 성공적으로 완료되었습니다.");

        return ResponseEntity.ok(apiResponse);
    }
}
