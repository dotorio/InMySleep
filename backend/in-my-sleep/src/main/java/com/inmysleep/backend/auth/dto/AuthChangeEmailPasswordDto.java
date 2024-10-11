package com.inmysleep.backend.auth.dto;

import lombok.Data;

@Data
public class AuthChangeEmailPasswordDto {
    private String email;
    private String password;
}
