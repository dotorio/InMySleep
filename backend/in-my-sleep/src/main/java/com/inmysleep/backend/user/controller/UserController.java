package com.inmysleep.backend.user.controller;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.api.response.ApiPageResponse;
import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.api.response.PageInfo;
import com.inmysleep.backend.user.dto.UserInfoDto;
import com.inmysleep.backend.user.dto.UserSearchResultDto;
import com.inmysleep.backend.user.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/user")
public class UserController {

    private final UserService userService;

    @Autowired
    public UserController(UserService userService) {
        this.userService = userService;
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

    @GetMapping("/{id}")
    public ResponseEntity<ApiResponse<UserInfoDto>> getUser(@PathVariable Integer id) {
        ApiResponse<UserInfoDto> apiResponse = new ApiResponse<>();
        UserInfoDto userInfo = userService.getUserInfo(id);

        apiResponse.setResponseTrue(userInfo, "유저 정보 조회");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/search")
    public ResponseEntity<ApiPageResponse<List<UserSearchResultDto>>> searchUser(
            @RequestParam String username,
            @RequestParam int page,
            @RequestParam int size,
            @RequestParam int userId) {
        Pageable pageable = PageRequest.of(page, size);
        Page<UserSearchResultDto> searchResults = userService.searchUsers(username, userId, pageable);

        PageInfo pageInfo = new PageInfo(size, page, searchResults.getTotalElements(), searchResults.getTotalPages(), searchResults.getNumber());

        ApiPageResponse<List<UserSearchResultDto>> apiResponse = new ApiPageResponse<>();
        apiResponse.setResponseTrue(searchResults.stream().toList(), "유저 검색 결과", pageInfo);

        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/check-email-user")
    public ResponseEntity<ApiResponse<Void>> checkEmailUser(@RequestParam String email) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        boolean isUser = userService.isEmailAlreadyInUse(email);

        if (isUser) {
            apiResponse.setResponseTrue(null, "가입된 유저 입니다.");
        } else {
            throw new NotFoundElementException("가입되어 있지 않은 유저입니다.");
        }
        return ResponseEntity.ok(apiResponse);
    }
}
