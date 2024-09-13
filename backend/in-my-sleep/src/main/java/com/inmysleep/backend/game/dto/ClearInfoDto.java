package com.inmysleep.backend.game.dto;

import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class ClearInfoDto {

    @NotNull(message = "Room Id is required")
    private Integer roomId;

    @NotNull(message = "Stage Number is required")
    private Integer stageNumber;
}
