package com.inmysleep.backend.game.converter;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import com.inmysleep.backend.game.dto.Attributes;
import jakarta.persistence.AttributeConverter;
import jakarta.persistence.Converter;

@Converter
public class JsonAttributesConverter implements AttributeConverter<Attributes, String> {

    private final ObjectMapper objectMapper = new ObjectMapper();

    @Override
    public String convertToDatabaseColumn(Attributes attributes) {
        try {
            return objectMapper.writeValueAsString(attributes);
        } catch (JsonProcessingException e) {
            throw new IllegalArgumentException("Error converting attributes to JSON", e);
        }
    }

    @Override
    public Attributes convertToEntityAttribute(String json) {
        try {
            return objectMapper.readValue(json, Attributes.class);
        } catch (JsonProcessingException e) {
            throw new IllegalArgumentException("Error reading JSON as attributes", e);
        }
    }
}