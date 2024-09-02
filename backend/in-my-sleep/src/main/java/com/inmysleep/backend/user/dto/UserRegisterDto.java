package com.inmysleep.backend.user.dto;

import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class UserRegisterDto {
    @NotNull
    private String email;

    @NotNull
    private String username;

    @NotNull
    private String password;
}
