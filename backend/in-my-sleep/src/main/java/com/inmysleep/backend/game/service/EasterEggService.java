package com.inmysleep.backend.game.service;

import com.inmysleep.backend.game.dto.AddSkinResultDto;
import com.inmysleep.backend.game.dto.EasterEggDto;
import com.inmysleep.backend.game.dto.UserSkinDto;

import java.util.List;

public interface EasterEggService {
    List<EasterEggDto> getAllEasterEgg();
    List<UserSkinDto> getUserSkinInfo(int userId);
    AddSkinResultDto addUserSkin(int userId);
    void defaultEasterEgg(int userId);
    void setDefaultEasterEgg(int userId);

    void SetAllUserSkinInfo();
}
