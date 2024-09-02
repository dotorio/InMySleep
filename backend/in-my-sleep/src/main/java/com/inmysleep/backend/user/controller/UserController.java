package com.inmysleep.backend.user.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.user.dto.UserRegisterDto;
import com.inmysleep.backend.user.service.UserService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
public class UserController {

    private final UserService userService;

    @Autowired
    public UserController(UserService userService) {
        this.userService = userService;
    }

    @PostMapping("/login")
    public ResponseEntity<ApiResponse<Void>> login(@RequestBody UserRegisterDto userRegisterDto) {
        return null;
    }

    @PostMapping("/signup")
    public ResponseEntity<ApiResponse<Void>> signup(@Valid @RequestBody UserRegisterDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        if (userService.isEmailAlreadyInUse(dto.getEmail())) {
            apiResponse.setResponseFalse(null, "이미 사용 중인 이메일입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        if (userService.isUsernameAlreadyInUse(dto.getUsername())) {
            apiResponse.setResponseTrue(null, "이미 사용 중인 닉네임입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        userService.registerUser(dto);
        apiResponse.setResponseTrue(null, "회원가입이 성공적으로 완료되었습니다.");

        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/check-email")
    public ResponseEntity<ApiResponse<Void>> checkEmail(@RequestParam String email) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        if (userService.isEmailAlreadyInUse(email)) {
            apiResponse.setResponseFalse(null, "이미 사용 중인 이메일입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        apiResponse.setResponseTrue(null, "사용 가능한 이메일입니다.");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/check-username")
    public ResponseEntity<ApiResponse<Void>> checkUsername(@RequestParam String username) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        if (userService.isUsernameAlreadyInUse(username)) {
            apiResponse.setResponseFalse(null, "이미 사용 중인 닉네임입니다.");
            return ResponseEntity.status(HttpStatus.CONFLICT).body(apiResponse);
        }

        apiResponse.setResponseTrue(null, "사용 가능한 닉네임입니다.");
        return ResponseEntity.ok(apiResponse);
    }
}
