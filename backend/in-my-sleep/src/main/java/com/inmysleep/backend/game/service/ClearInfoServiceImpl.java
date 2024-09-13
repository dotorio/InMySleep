package com.inmysleep.backend.game.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.game.dto.ClearInfoDto;
import com.inmysleep.backend.game.entity.ClearInfo;
import com.inmysleep.backend.game.entity.Room;
import com.inmysleep.backend.game.repository.ClearInfoRepository;
import com.inmysleep.backend.game.repository.RoomRepository;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Service
public class ClearInfoServiceImpl implements ClearInfoService {

    private final ClearInfoRepository clearInfoRepository;
    private final RoomRepository roomRepository;

    public ClearInfoServiceImpl(ClearInfoRepository clearInfoRepository, RoomRepository roomRepository) {
        this.clearInfoRepository = clearInfoRepository;

        this.roomRepository = roomRepository;
    }

    @Override
    public void clearStage(ClearInfoDto clearInfoDto) {
        Room room = roomRepository.findById(clearInfoDto.getRoomId()).orElseThrow(() -> new NotFoundElementException("Room not found"));

        ClearInfo clearInfo = new ClearInfo();
        clearInfo.setRoomId(room);
        clearInfo.setStageNumber(clearInfoDto.getStageNumber());
        clearInfo.setClearDate(LocalDateTime.now());
        clearInfoRepository.save(clearInfo);
    }
}
