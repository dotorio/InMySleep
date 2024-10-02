package com.inmysleep.backend.game.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.game.dto.AddSkinResultDto;
import com.inmysleep.backend.game.dto.EasterEggDto;
import com.inmysleep.backend.game.dto.UserSkinDto;
import com.inmysleep.backend.game.entity.EasterEgg;
import com.inmysleep.backend.game.entity.Metadata;
import com.inmysleep.backend.game.entity.UserEasterEgg;
import com.inmysleep.backend.game.entity.UserSkin;
import com.inmysleep.backend.game.repository.EasterEggRepository;
import com.inmysleep.backend.game.repository.MetadataRepository;
import com.inmysleep.backend.game.repository.UserEasterEggRepository;
import com.inmysleep.backend.game.repository.UserSkinRepository;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@RequiredArgsConstructor
@Service
public class EasterEggServiceImpl implements EasterEggService {

    private final EasterEggRepository easterEggRepository;
    private final UserSkinRepository userSkinRepository;
    private final UserRepository userRepository;
    private final MetadataRepository metadataRepository;
    private final UserEasterEggRepository userEasterEggRepository;

    @Override
    public List<EasterEggDto> getAllEasterEgg() {
        List<EasterEgg> list = easterEggRepository.findAll();
        List<EasterEggDto> dtos = new ArrayList<>();
        for (EasterEgg e : list) {
            EasterEggDto dto = new EasterEggDto();
            dto.setEasterEggId(e.getEasterEggId());
            dto.setStage(e.getStage());
            dto.setPosx(e.getPosx());
            dto.setPosy(e.getPosy());
            dto.setPosz(e.getPosz());
            dtos.add(dto);
        }

        return dtos;
    }

    @Override
    public UserSkinDto getUserSkinInfo(int userId) {
        UserSkin us = userSkinRepository.findByUserId(userId);
        UserSkinDto dto = new UserSkinDto();

        // 기본 정보 추가
        dto.setUserId(us.getUserId());
        dto.setUserSkinId(us.getUserSkinId());
        dto.setBearSkin(us.getBearSkinMetadata());
        dto.setRabbitSkin(us.getRabbitSkinMetadata());

        // metadata 참조
        Metadata bearData = metadataRepository.findById(us.getBearSkinMetadata())
                .orElseThrow(() -> new NotFoundElementException("bear skin metadata not found"));
        Metadata rabbitData = metadataRepository.findById(us.getRabbitSkinMetadata())
                .orElseThrow(() -> new NotFoundElementException("rabbit skin metadata not found"));

        dto.setBear(String.format("%02d", Integer.parseInt(bearData.getAttributes().getColor())));
        dto.setRabbit(String.format("%02d", Integer.parseInt(rabbitData.getAttributes().getColor())));
        return dto;
    }

    @Override
    public AddSkinResultDto addUserSkin(int userId) {
        // 유저가 존재하는지 확인, 없으면 예외 발생
        if (!userRepository.existsById(userId)) {
            throw new NotFoundElementException("사용자를 찾을 수 없습니다");
        }

        // 램덤 스킨 획득
        Metadata data = getRandomMetadata();

        // 중복 체크
        Optional<UserEasterEgg> existingSkin = userEasterEggRepository
                .findByUserIdAndMetadataId(userId, data.getId());

        // 중복 아닐 경우 스킨 추가
        if (existingSkin.isEmpty()) {
            saveUserEasterEgg(userId, data.getId());
        }

        return new AddSkinResultDto(
                data.getImageUrl(),
                data.getDescription(),
                data.getAttributes(),
                existingSkin.isPresent()
        );
    }

    // 기본 스킨 지급
    @Override
    public void defaultEasterEgg(int userId) {
        saveUserEasterEgg(userId, 1);
        saveUserEasterEgg(userId, 4);
    }

    // 기본 스킨 장착
    @Override
    public void setDefaultEasterEgg(int userId) {
        UserSkin uSkin = new UserSkin();
        uSkin.setUserId(userId);
        uSkin.setBearSkinMetadata(1);
        uSkin.setRabbitSkinMetadata(4);
        userSkinRepository.save(uSkin);
    }

    // 유저 스킨 저장
    public void saveUserEasterEgg(int userId, int metadataId) {
        UserEasterEgg bearDefaultSkin = new UserEasterEgg();
        bearDefaultSkin.setUserId(userId);
        bearDefaultSkin.setMetadataId(metadataId);
        userEasterEggRepository.save(bearDefaultSkin);
    }

    // 랜덤 스킨 지급
    public Metadata getRandomMetadata() {
        // 전체 메타데이터의 총 개수
        long qty = metadataRepository.count();

        if (qty == 0) {
            throw new NotFoundElementException("No metadata available.");
        }

        // 무작위로 하나의 인덱스를 선택
        int idx = (int) (Math.random() * qty);

        // 페이징을 사용하여 무작위 메타데이터 획득
        Page<Metadata> metadataPage = metadataRepository.findAll(PageRequest.of(idx, 1));

        // 페이징 결과가 있으면 해당 메타데이터를 반환
        if (metadataPage.hasContent()) {
            return metadataPage.getContent().get(0);
        } else {
            throw new NotFoundElementException("Random metadata not found.");
        }
    }
}
