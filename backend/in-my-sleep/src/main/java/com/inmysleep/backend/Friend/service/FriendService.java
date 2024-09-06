package com.inmysleep.backend.Friend.service;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.dto.FriendRequestDto;
import com.inmysleep.backend.user.dto.UserInfoDto;

import java.util.List;

public interface FriendService {
    List<FriendDto> getAllFriends(int userId);
    void requestFriend(FriendRequestDto requestDto);
    List<UserInfoDto> getReceivedRequests(int userId);
    List<UserInfoDto> getSentRequests(int userId);
    void acceptFriend(FriendRequestDto requestDto);
    void closeRequest(FriendRequestDto requestDto);
    void deleteFriend(FriendRequestDto requestDto);
    void refuseFriend(FriendRequestDto dto);
}
