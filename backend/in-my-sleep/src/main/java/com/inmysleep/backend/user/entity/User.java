package com.inmysleep.backend.user.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Getter
@Setter
@ToString
@Entity
@Table(name = "user")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int userId;

    private String email;

    private String username;

    @JsonIgnore
    private String password;

    @Column(name = "last_stage", columnDefinition = "TINYINT")
    private int lastStage;

    private LocalDateTime createdAt;

    private LocalDateTime updatedAt;

    private LocalDateTime lastLogin;

    @Column(name = "is_active", columnDefinition = "TINYINT(1)")
    private Boolean isActive;

}
