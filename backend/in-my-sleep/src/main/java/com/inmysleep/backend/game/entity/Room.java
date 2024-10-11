package com.inmysleep.backend.game.entity;

import com.inmysleep.backend.user.entity.User;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@ToString
@Table(name = "room")
public class Room {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int roomId;

    @ManyToOne
    @JoinColumn(name = "host_id", referencedColumnName = "userId")
    private User hostId;

    @ManyToOne
    @JoinColumn(name = "participant_id", referencedColumnName = "userId")
    private User participantId;

    @Column(name = "character_host", columnDefinition = "TINYINT")
    private int characterHost;

    @Column(name = "character_participant", columnDefinition = "TINYINT")
    private int characterParticipant;

    @Column(name = "is_cleared", columnDefinition = "TINYINT(1)")
    private Boolean isCleared;

    private LocalDateTime startDate;

    private LocalDateTime clearDate;

    private LocalDateTime createdAt;

    // 엔티티가 처음 저장되기 전에 호출되는 메서드
    @PrePersist
    protected void onCreate() {
        this.createdAt = LocalDateTime.now();
    }

}
