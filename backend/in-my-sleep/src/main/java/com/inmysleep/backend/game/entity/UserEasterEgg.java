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
@Table(name = "user_easter_egg")
public class UserEasterEgg {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int userEasterEggId;

    private int userId;

    private int metadataId;

    private LocalDateTime createdAt;

    @PrePersist
    protected void onCreate() {
        this.createdAt = LocalDateTime.now();
    }
}
