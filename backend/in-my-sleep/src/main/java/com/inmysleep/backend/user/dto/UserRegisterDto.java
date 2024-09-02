package com.inmysleep.backend.user.dto;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class UserRegisterDto {
    private String email;
    private String username;
    private String password;
}
