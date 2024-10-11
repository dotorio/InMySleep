package com.inmysleep.backend.api.response;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
public class PageInfo {
    private int size;        // 한 페이지의 항목 수
    private int page;        // 현재 페이지 번호
    private long total;      // 전체 항목 수
    private int totalPages;  // 전체 페이지 수
    private int currentPage; // 현재 페이지 (page와 동일)
}
