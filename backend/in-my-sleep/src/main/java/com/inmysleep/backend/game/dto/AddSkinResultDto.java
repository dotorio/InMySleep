package com.inmysleep.backend.game.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.RequiredArgsConstructor;

@Data
@RequiredArgsConstructor
@AllArgsConstructor
public class AddSkinResultDto {
    private String skinImgUrl;
    private String description;
    private Attributes attributes;
    private Boolean duplicated;
}
