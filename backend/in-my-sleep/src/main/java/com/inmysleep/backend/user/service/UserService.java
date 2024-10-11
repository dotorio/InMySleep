package com.inmysleep.backend.user.service;

import com.inmysleep.backend.user.dto.UserInfoDto;
import com.inmysleep.backend.user.dto.UserSearchResultDto;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface UserService {
    boolean isEmailAlreadyInUse(String email);
    boolean isUsernameAlreadyInUse(String username);
    UserInfoDto getUserInfo(int id);
    Page<UserSearchResultDto> searchUsers(String username, int currentUserId, Pageable pageable);
}
