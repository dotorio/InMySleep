package com.inmysleep.backend.user.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class UserLoginDto {
    private int userId;
    private String username;
    private String email;
    private int lastStage;
    private String token;
}
