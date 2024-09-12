package com.inmysleep.backend.game.controller;

import com.inmysleep.backend.api.response.ApiResponse;
import com.inmysleep.backend.game.dto.ClearInfoDto;
import com.inmysleep.backend.game.service.ClearInfoService;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/game-info")
public class ClearInfoController {

    private final ClearInfoService clearInfoService;

    public ClearInfoController(ClearInfoService clearInfoService) {
        this.clearInfoService = clearInfoService;
    }

    @PostMapping("/clear-stage")
    public ResponseEntity<ApiResponse<Void>> clearStage(@RequestBody @Valid ClearInfoDto clearInfoDto) {
        ApiResponse<Void> apiResponse = new ApiResponse<>();

        clearInfoService.clearStage(clearInfoDto);

        apiResponse.setResponseTrue(null, "스테이지 클리어");
        return ResponseEntity.ok(apiResponse);
    }
}
