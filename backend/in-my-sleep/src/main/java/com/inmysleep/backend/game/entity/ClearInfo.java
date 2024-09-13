package com.inmysleep.backend.game.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@ToString
@Table(name = "clear_info")
public class ClearInfo {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int clearInfoId;

    @ManyToOne
    @JoinColumn(name = "room_id", referencedColumnName = "roomId")
    private Room roomId;

    @Column(name = "stage_number", columnDefinition = "TINYINT")
    private int stageNumber;

    private LocalDateTime clearDate;

}
