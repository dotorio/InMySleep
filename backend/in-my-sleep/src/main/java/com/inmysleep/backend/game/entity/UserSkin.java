package com.inmysleep.backend.game.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

@Entity
@Getter
@Setter
@Table(name = "user_skin")
public class UserSkin {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int userSkinId;

    private int userId;
    private int bearSkinMetadata;
    private int rabbitSkinMetadata;
}
