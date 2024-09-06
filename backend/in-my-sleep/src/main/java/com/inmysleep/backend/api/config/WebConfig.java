package com.inmysleep.backend.api.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class WebConfig implements WebMvcConfigurer {

    @Override
    public void addCorsMappings(CorsRegistry registry) {
        registry.addMapping("/**") // 모든 경로에 대해 CORS 허용
                .allowedOrigins("http://localhost") // 모든 도메인 허용 (특정 도메인만 허용하려면 도메인 주소를 명시)
                .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS") // 허용할 HTTP 메서드 명시
                .allowedHeaders("*") // 모든 헤더 허용
                .allowCredentials(true) // 자격 증명 허용 (예: 쿠키)
                .maxAge(3600); // Pre-flight 요청 캐시 시간 설정 (초 단위)
    }
}
