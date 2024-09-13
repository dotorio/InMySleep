package com.inmysleep.backend.game.service;

import com.inmysleep.backend.game.dto.RoomClearDto;
import com.inmysleep.backend.game.dto.RoomCreateDto;
import com.inmysleep.backend.game.entity.Room;

public interface RoomService {
    Room createRoom(RoomCreateDto roomCreateDto);
    void clearRoom(RoomClearDto roomClearDto);
}
