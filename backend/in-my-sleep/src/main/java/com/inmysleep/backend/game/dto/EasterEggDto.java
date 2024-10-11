package com.inmysleep.backend.game.dto;

import lombok.Data;
import lombok.RequiredArgsConstructor;

@Data
@RequiredArgsConstructor
public class EasterEggDto {
    private int easterEggId;
    private String name;
    private String description;
    private int stage;
    private double posx;
    private double posy;
    private double posz;
}
