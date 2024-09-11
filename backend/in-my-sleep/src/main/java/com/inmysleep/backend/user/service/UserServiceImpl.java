package com.inmysleep.backend.user.service;

import com.inmysleep.backend.Friend.repository.FriendRepository;
import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.user.dto.UserInfoDto;
import com.inmysleep.backend.user.dto.UserSearchResultDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class UserServiceImpl implements UserService {

    private final UserRepository userRepository;
    private final FriendRepository friendRepository;

    public UserServiceImpl(UserRepository userRepository, FriendRepository friendRepository) {
        this.userRepository = userRepository;
        this.friendRepository = friendRepository;
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
    public UserInfoDto getUserInfo(int id) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new NotFoundElementException("User not found"));

        UserInfoDto userInfo = new UserInfoDto();
        userInfo.setUserId(user.getUserId());
        userInfo.setUsername(user.getUsername());
        userInfo.setEmail(user.getEmail());
        userInfo.setLastStage(user.getLastStage());

        return userInfo;
    }

    @Override
    public Page<UserSearchResultDto> searchUsers(String username, int currentUserId, Pageable pageable) {
        Page<User> users = userRepository.findByUsernameContainingIgnoreCase(username, currentUserId, pageable);

        // Page<User>를 Page<UserSearchResultDto>로 변환
        Page<UserSearchResultDto> result = users.map(user -> {
            boolean isFriend = friendRepository.existsFriendRelation(currentUserId, user.getUserId());
            return new UserSearchResultDto(user.getUserId(), user.getUsername(), user.getEmail(), user.getLastStage(), isFriend);
        });
        return result;
    }
}
