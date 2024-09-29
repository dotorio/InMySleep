package com.inmysleep.backend.game.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Entity
@Getter
@Setter
@ToString
@Table(name = "easter_egg")
public class EasterEgg {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int easterEggId;

    private String name;
    private String description;

    @Column(name = "stage", columnDefinition = "TINYINT")
    private int stage;
    private double posx;
    private double posy;
    private double posz;
}
