package com.inmysleep.backend.game.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.game.dto.ClearInfoDto;
import com.inmysleep.backend.game.entity.ClearInfo;
import com.inmysleep.backend.game.entity.Room;
import com.inmysleep.backend.game.repository.ClearInfoRepository;
import com.inmysleep.backend.game.repository.RoomRepository;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@RequiredArgsConstructor
@Service
public class ClearInfoServiceImpl implements ClearInfoService {

    private final ClearInfoRepository clearInfoRepository;
    private final RoomRepository roomRepository;
    private final UserRepository userRepository;

    @Override
    public void clearStage(ClearInfoDto clearInfoDto) {
        Room room = roomRepository.findById(clearInfoDto.getRoomId()).orElseThrow(() -> new NotFoundElementException("Room not found"));

        ClearInfo clearInfo = new ClearInfo();
        clearInfo.setRoomId(room);
        clearInfo.setStageNumber(clearInfoDto.getStageNumber());
        clearInfo.setClearDate(LocalDateTime.now());
        clearInfoRepository.save(clearInfo);

        setLastClearStage(clearInfoDto);
    }

    public void setLastClearStage(ClearInfoDto clearInfoDto) {
        int clearedStage = clearInfoDto.getStageNumber();
        Room roomInfo = roomRepository.findById(clearInfoDto.getRoomId()).orElseThrow(() -> new NotFoundElementException("Room not found"));

        updateLastStage(roomInfo.getHostId(), clearedStage);
        updateLastStage(roomInfo.getParticipantId(), clearedStage);
    }

    private void updateLastStage(User user, int clearedStage) {
        int currentLastStage = user.getLastStage();
        if (clearedStage > currentLastStage) {
            user.setLastStage(clearedStage);
            userRepository.save(user); // 변경이 있을 때만 DB 업데이트 호출
        }
    }
}
