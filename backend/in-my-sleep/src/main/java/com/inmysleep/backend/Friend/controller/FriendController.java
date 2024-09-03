package com.inmysleep.backend.Friend.controller;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.dto.FriendRequestDto;
import com.inmysleep.backend.Friend.service.FriendService;
import com.inmysleep.backend.api.response.ApiResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/friend")
public class FriendController {

    private final FriendService friendService;

    @Autowired
    public FriendController(FriendService friendService) {
        this.friendService = friendService;
    }

    @GetMapping("/friend-list")
    public ResponseEntity<ApiResponse<List<FriendDto>>> getFriendList(@RequestParam int userId) {
        ApiResponse<List<FriendDto>> apiResponse = new ApiResponse<>();

        List<FriendDto> lst = friendService.getAllFriends(userId);
        apiResponse.setResponseTrue(lst, "친구 목록을 불러왔습니다.");

        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/friend-request")
    public ResponseEntity<ApiResponse<Void>> requestFriend(@RequestBody FriendRequestDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        friendService.requestFriend(dto);

        apiResponse.setResponseTrue(null, "친구 요청 전송 완료");
        return ResponseEntity.ok(apiResponse);
    }
}
