package com.inmysleep.backend.auth.dto;

import jakarta.validation.constraints.NotNull;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class AuthUserDto {
    @NotNull
    private String email;

    @NotNull
    private String username;

    @NotNull
    private String password;
}
