package com.inmysleep.backend.user.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class UserSearchResultDto {
    private int userId;
    private String username;
    private String email;
    private int lastStage;
    private boolean isFriend;  // 친구 여부
}
