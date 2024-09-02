package com.inmysleep.backend.user.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.sql.Timestamp;

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

    private String password;

    private Timestamp createdAt;

    private Timestamp updatedAt;

    private Timestamp lastLogin;

    @Column(name = "is_active", columnDefinition = "TINYINT(1)")
    private Boolean isActive;

}
