package com.inmysleep.backend.game.service;

import com.inmysleep.backend.game.dto.EasterEggDto;
import com.inmysleep.backend.game.dto.UserSkinDto;
import com.inmysleep.backend.game.entity.EasterEgg;
import com.inmysleep.backend.game.entity.UserSkin;
import com.inmysleep.backend.game.repository.EasterEggRepository;
import com.inmysleep.backend.game.repository.UserSkinRepository;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@RequiredArgsConstructor
@Service
public class EasterEggServiceImpl implements EasterEggService {

    private final EasterEggRepository easterEggRepository;
    private final UserSkinRepository userSkinRepository;
    private final UserRepository userRepository;

    @Override
    public List<EasterEggDto> getAllEasterEgg() {
        List<EasterEgg> list = easterEggRepository.findAll();
        List<EasterEggDto> dtos = new ArrayList<>();
        for (EasterEgg e : list) {
            EasterEggDto dto = new EasterEggDto();
            dto.setEasterEggId(e.getEasterEggId());
            dto.setDescription(e.getDescription());
            dto.setName(e.getName());
            dto.setStage(e.getStage());
            dto.setPosx(e.getPosx());
            dto.setPosy(e.getPosy());
            dto.setPosz(e.getPosz());
            dtos.add(dto);
        }

        return dtos;
    }

    @Override
    public List<UserSkinDto> getUserSkinInfo(int userId) {
        List<UserSkin> list = userSkinRepository.findByUserId(userId);
        List<UserSkinDto> dtos = new ArrayList<>();
        for (UserSkin us : list) {
            UserSkinDto dto = new UserSkinDto();
            dto.setUserId(us.getUserId());
            dto.setUserSkinId(us.getUserSkinId());
            dto.setBearSkin(us.getBearSkin());
            dto.setRabbitSkin(us.getRabbitSkin());
            dtos.add(dto);
        }
        return dtos;
    }

    @Transactional
    void SetAllUserSkinInfo() {
        List<User> list = userRepository.findAll();
        for (User u : list) {
            List<UserSkin> us = userSkinRepository.findByUserId(u.getUserId());
            if (us.isEmpty()) {
                UserSkin uSkin = new UserSkin();
                uSkin.setUserId(u.getUserId());
                uSkin.setBearSkin(1);
                uSkin.setRabbitSkin(4);
                userSkinRepository.save(uSkin);
            }
        }
    }
}
