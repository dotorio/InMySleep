package com.inmysleep.backend.api.response;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
public class ApiPageResponse<T> {
    private boolean success;
    private T data;
    private String message;
    private PageInfo pageInfo; // 페이지 정보 추가

    public void setResponseTrue(T data, String message, PageInfo pageInfo) {
        this.success = true;
        this.data = data;
        this.message = message;
        this.pageInfo = pageInfo;
    }

    public void setResponseFalse(T data, String message, PageInfo pageInfo) {
        this.success = false;
        this.data = data;
        this.message = message;
        this.pageInfo = pageInfo;
    }
}
