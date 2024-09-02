package com.inmysleep.backend.api.response;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
public class ApiResponse<T> {
    private boolean success;
    private T data;
    private String message;

    public void setResponseTrue(T data, String message) {
        this.success = true;
        this.data = data;
        this.message = message;
    }

    public void setResponseFalse(T data, String message) {
        this.success = false;
        this.data = data;
        this.message = message;
    }
}
