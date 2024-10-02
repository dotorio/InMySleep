package com.inmysleep.backend.auth.dto;

import lombok.Data;

@Data
public class AuthChangePasswordDto {
    private int userId;
    private String oldPassword;
    private String newPassword;
}
