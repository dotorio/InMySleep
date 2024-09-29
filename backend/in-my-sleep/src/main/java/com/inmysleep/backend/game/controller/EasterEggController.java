package com.inmysleep.backend.game.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.game.dto.EasterEggDto;
import com.inmysleep.backend.game.dto.UserSkinDto;
import com.inmysleep.backend.game.repository.UserSkinRepository;
import com.inmysleep.backend.game.service.EasterEggService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

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

    @GetMapping("/skin-list")
    public ResponseEntity<ApiResponse<List<UserSkinDto>>> getUserSkinList(@RequestParam int userId) {
        ApiResponse<List<UserSkinDto>> apiResponse = new ApiResponse<>();
        apiResponse.setResponseTrue(easterEggService.getUserSkinInfo(userId), "유저 스킨 정보");
        return ResponseEntity.ok(apiResponse);
    }
}
