package com.inmysleep.backend.game.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.game.dto.RoomClearDto;
import com.inmysleep.backend.game.dto.RoomCreateDto;
import com.inmysleep.backend.game.entity.Room;
import com.inmysleep.backend.game.service.RoomService;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/room")
public class RoomController {

    private final RoomService roomService;

    public RoomController(RoomService roomService) {
        this.roomService = roomService;
    }

    @PostMapping("/create")
    public ResponseEntity<ApiResponse<Room>> createRoom(@Valid @RequestBody RoomCreateDto dto) {
        ApiResponse<Room> apiResponse = new ApiResponse<>();

        Room savedRoom = roomService.createRoom(dto);

        apiResponse.setResponseTrue(savedRoom, "방 생성");
        return ResponseEntity.ok(apiResponse);
    }

    @PutMapping("/clear")
    public ResponseEntity<ApiResponse<Room>> clearRoom(@RequestBody RoomClearDto dto) {
        ApiResponse<Room> apiResponse = new ApiResponse<>();

        roomService.clearRoom(dto);

        apiResponse.setResponseTrue(null, dto.getRoomId() + " Room Game Clear");
        return ResponseEntity.ok(apiResponse);
    }
}
