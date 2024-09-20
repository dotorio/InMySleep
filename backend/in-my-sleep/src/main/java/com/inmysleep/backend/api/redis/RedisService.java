package com.inmysleep.backend.api.redis;

import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Service;

import java.time.Duration;
import java.util.concurrent.TimeUnit;

@Service
@RequiredArgsConstructor
public class RedisService {

    private final RedisTemplate<String, String> redisTemplate;

    // Redis에 값을 저장하고 만료 시간 설정
    public void setValues(String key, String value, Duration expiration) {
        redisTemplate.opsForValue().set(key, value, expiration.getSeconds(), TimeUnit.SECONDS);
    }

    // Redis에서 값을 조회
    public String getValues(String key) {
        return redisTemplate.opsForValue().get(key);
    }

    // 값이 존재하는지 확인
    public boolean checkExistsValue(String value) {
        return value != null;
    }

    // Redis에서 키 삭제 (선택적 기능)
    public void deleteValues(String key) {
        redisTemplate.delete(key);
    }
}

