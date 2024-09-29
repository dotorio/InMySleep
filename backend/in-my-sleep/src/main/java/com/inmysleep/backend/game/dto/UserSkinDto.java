package com.inmysleep.backend.game.dto;

import lombok.Data;
import lombok.RequiredArgsConstructor;

@Data
@RequiredArgsConstructor
public class UserSkinDto {
    private int userSkinId;
    private int userId;
    private int bearSkin;
    private int rabbitSkin;
}
