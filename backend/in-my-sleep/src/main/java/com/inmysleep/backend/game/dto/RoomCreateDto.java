package com.inmysleep.backend.game.dto;

import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class RoomCreateDto {
    private int hostId;
    private int participantId;

    @NotNull(message = "Character host is required")
    private Integer characterHost;

    @NotNull(message = "Character Participant is required")
    private Integer characterParticipant;
}
