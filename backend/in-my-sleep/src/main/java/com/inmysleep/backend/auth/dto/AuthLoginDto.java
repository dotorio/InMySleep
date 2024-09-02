package com.inmysleep.backend.auth.dto;

import lombok.Data;

@Data
public class AuthLoginDto {
    private String email;
    private String password;
}
