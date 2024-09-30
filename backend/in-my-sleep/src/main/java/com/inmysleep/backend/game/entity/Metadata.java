package com.inmysleep.backend.game.entity;

import com.inmysleep.backend.game.converter.JsonAttributesConverter;
import com.inmysleep.backend.game.dto.Attributes;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@ToString
@Table(name = "metadata")
public class Metadata {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    private String metadataUri;
    private String imageUrl;

    @Column(name = "description", columnDefinition = "TEXT")
    private String description;

    @Convert(converter = JsonAttributesConverter.class)
    @Column(name = "attributes", columnDefinition = "JSON")
    private Attributes attributes;

    private LocalDateTime createdAt;
}
