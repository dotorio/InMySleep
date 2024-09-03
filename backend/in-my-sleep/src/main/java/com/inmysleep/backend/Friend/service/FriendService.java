package com.inmysleep.backend.Friend.service;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.dto.FriendRequestDto;

import java.util.List;

public interface FriendService {
    List<FriendDto> getAllFriends(int userId);
    void requestFriend(FriendRequestDto requestDto);
}
