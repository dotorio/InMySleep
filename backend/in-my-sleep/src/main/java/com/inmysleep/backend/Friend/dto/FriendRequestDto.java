package com.inmysleep.backend.Friend.dto;

import lombok.Data;

@Data
public class FriendRequestDto {
    private int requestUserId;
    private int receiveUserId;
}
