package com.inmysleep.backend.Friend.controller;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.dto.FriendRequestDto;
import com.inmysleep.backend.Friend.service.FriendService;
import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.user.dto.UserInfoDto;
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

    @GetMapping("/list")
    public ResponseEntity<ApiResponse<List<FriendDto>>> getFriendList(@RequestParam int userId) {
        ApiResponse<List<FriendDto>> apiResponse = new ApiResponse<>();

        List<FriendDto> lst = friendService.getAllFriends(userId);
        apiResponse.setResponseTrue(lst, "친구 목록을 불러왔습니다.");

        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/request")
    public ResponseEntity<ApiResponse<Void>> requestFriend(@RequestBody FriendRequestDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        friendService.requestFriend(dto);

        apiResponse.setResponseTrue(null, "친구 요청 전송 완료");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/receive-list")
    public ResponseEntity<ApiResponse<List<UserInfoDto>>> getFriendReceiveList(@RequestParam int userId) {
        ApiResponse<List<UserInfoDto>> apiResponse = new ApiResponse<>();

        List<UserInfoDto> list = friendService.getReceivedRequests(userId);
        apiResponse.setResponseTrue(list, "받은 친구 요청 리스트");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/request-list")
    public ResponseEntity<ApiResponse<List<UserInfoDto>>> getFriendRequestList(@RequestParam int userId) {
        ApiResponse<List<UserInfoDto>> apiResponse = new ApiResponse<>();

        List<UserInfoDto> list = friendService.getSentRequests(userId);
        apiResponse.setResponseTrue(list, "보낸 친구 요청 리스트");
        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/accept")
    public ResponseEntity<ApiResponse<Void>> acceptFriend(@RequestBody FriendRequestDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        friendService.acceptFriend(dto);    // 친구 추가
        friendService.closeRequest(dto);    // 요청 닫기

        apiResponse.setResponseTrue(null, "친구 추가 완료");
        return ResponseEntity.ok(apiResponse);
    }

    @DeleteMapping("/delete")
    public ResponseEntity<ApiResponse<Void>> deleteFriend(@RequestBody FriendRequestDto dto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        friendService.deleteFriend(dto);
        apiResponse.setResponseTrue(null, "친구 삭제 완료");
        return ResponseEntity.ok(apiResponse);
    }
}
