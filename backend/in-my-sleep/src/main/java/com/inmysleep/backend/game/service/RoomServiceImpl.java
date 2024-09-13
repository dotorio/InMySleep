package com.inmysleep.backend.game.service;

import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.game.dto.RoomClearDto;
import com.inmysleep.backend.game.dto.RoomCreateDto;
import com.inmysleep.backend.game.entity.Room;
import com.inmysleep.backend.game.repository.RoomRepository;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Service
public class RoomServiceImpl implements RoomService {

    private final RoomRepository roomRepository;
    private final UserRepository userRepository;

    public RoomServiceImpl(RoomRepository roomRepository, UserRepository userRepository) {
        this.roomRepository = roomRepository;
        this.userRepository = userRepository;
    }


    @Override
    public Room createRoom(RoomCreateDto roomCreateDto) {
        User host = userRepository.findById(roomCreateDto.getHostId()).orElseThrow(() -> new NotFoundElementException("User not found for host."));
        User participant = userRepository.findById(roomCreateDto.getParticipantId()).orElseThrow(() -> new NotFoundElementException("User not found for participant."));

        Room room = new Room();
        room.setHostId(host);
        room.setParticipantId(participant);
        room.setCharacterHost(roomCreateDto.getCharacterHost());
        room.setCharacterParticipant(roomCreateDto.getCharacterParticipant());
        room.setIsCleared(false);
        room.setStartDate(LocalDateTime.now());
        Room savedRoom = roomRepository.save(room);

        return savedRoom;
    }

    @Override
    public void clearRoom(RoomClearDto roomClearDto) {
        Room room = roomRepository.findById(roomClearDto.getRoomId()).orElseThrow(() -> new NotFoundElementException("Room not found."));
        room.setIsCleared(true);
        room.setClearDate(LocalDateTime.now());
        roomRepository.save(room);
    }
}
