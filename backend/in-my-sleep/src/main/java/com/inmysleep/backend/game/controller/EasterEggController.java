package com.inmysleep.backend.game.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.game.dto.AddSkinResultDto;
import com.inmysleep.backend.game.dto.EasterEggDto;
import com.inmysleep.backend.game.dto.UserSkinDto;
import com.inmysleep.backend.game.service.EasterEggService;
import com.inmysleep.backend.user.dto.UserIdRequestDto;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/easter")
public class EasterEggController {

    private final EasterEggService easterEggService;

    @GetMapping("/list")
    public ResponseEntity<ApiResponse<List<EasterEggDto>>> easterEggList() {
        ApiResponse<List<EasterEggDto>> apiResponse = new ApiResponse<>();
        apiResponse.setResponseTrue(easterEggService.getAllEasterEgg(), "이스터에그 정보 리스트");
        return ResponseEntity.ok(apiResponse);
    }

    @GetMapping("/user-skin-info")
    public ResponseEntity<ApiResponse<UserSkinDto>> getUserSkinList(@RequestParam int userId) {
        ApiResponse<UserSkinDto> apiResponse = new ApiResponse<>();
        apiResponse.setResponseTrue(easterEggService.getUserSkinInfo(userId), "유저 스킨 정보");
        return ResponseEntity.ok(apiResponse);
    }

    @PostMapping("/add-skin")
    public ResponseEntity<ApiResponse<AddSkinResultDto>> addUserSkin(@RequestBody UserIdRequestDto userIdDto) {
        AddSkinResultDto result = easterEggService.addUserSkin(userIdDto.getUserId());
        String message = (result.getDuplicated()) ? "중복 획득" : "새로운 스킨 획득";

        ApiResponse<AddSkinResultDto> apiResponse = new ApiResponse<>();
        apiResponse.setResponseTrue(result, message);

        return ResponseEntity.ok(apiResponse);
    }
}
