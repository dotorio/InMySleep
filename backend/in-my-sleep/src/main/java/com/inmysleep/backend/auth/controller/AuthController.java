package com.inmysleep.backend.auth.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.auth.dto.AuthChangeEmailPasswordDto;
import com.inmysleep.backend.auth.dto.AuthChangePasswordDto;
import com.inmysleep.backend.auth.dto.AuthUserDto;
import com.inmysleep.backend.auth.service.AuthService;
import com.inmysleep.backend.email.config.CustomEmail;
import com.inmysleep.backend.email.service.MailAuthService;
import com.inmysleep.backend.user.dto.UserLoginDto;
import com.inmysleep.backend.user.service.UserService;
import jakarta.servlet.http.HttpSession;
import jakarta.transaction.Transactional;
import jakarta.validation.Valid;
import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@AllArgsConstructor
@RestController
@RequestMapping("/auth")
public class AuthController {

    private final AuthService authService;
    private final UserService userService;
    private final MailAuthService mailAuthService;

    @PostMapping("/login")
    public ResponseEntity<ApiResponse<UserLoginDto>> login(@RequestBody AuthUserDto userDto) {
        ApiResponse<UserLoginDto> apiResponse = new ApiResponse<>();
        UserLoginDto userInfo = authService.loginUser(userDto);
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

    @Transactional
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

    @PostMapping("/email/verification-request")
    public ResponseEntity<ApiResponse<Void>> sendMessage(@RequestParam("email") @Valid @CustomEmail String email) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        mailAuthService.sendCodeToEmail(email);

        apiResponse.setResponseTrue(null, "이메일 전송");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/emails/verifications")
    public ResponseEntity<ApiResponse<Void>> verificationEmail(@RequestParam("email") @Valid @CustomEmail String email,
                                                               @RequestParam("code") String authCode) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        boolean response = mailAuthService.verifiedCode(email, authCode);

        if (response) {
            apiResponse.setResponseTrue(null, "인증 성공");
        } else {
            apiResponse.setResponseFalse(null, "인증 실패");
        }
        return ResponseEntity.ok(apiResponse);
    }

    @PatchMapping("/change-password")
    public ResponseEntity<ApiResponse<Void>> changePassword(@RequestBody AuthChangePasswordDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        authService.changePassword(dto);
        apiResponse.setResponseTrue(null, "비밀번호 변경 완료");
        return ResponseEntity.ok(apiResponse);
    }

    // 이메일 인증 후 변경
    @PatchMapping("/change-email-password")
    public ResponseEntity<ApiResponse<Void>> changeEmailPassword(@RequestBody AuthChangeEmailPasswordDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        authService.changeEmailPassword(dto);
        apiResponse.setResponseTrue(null, "비밀번호 변경 완료");
        return ResponseEntity.ok(apiResponse);
    }
}
