package com.inmysleep.backend.Friend.dto;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class FriendDto {
    private int userId;
    private String username;
    private String email;
}
