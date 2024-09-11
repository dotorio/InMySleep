package com.inmysleep.backend.user.repository;

import com.inmysleep.backend.user.entity.User;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.Optional;

public interface UserRepository extends JpaRepository<User, Integer> {
    boolean existsByEmail(String email);

    boolean existsByUsername(String username);

    Optional<User> findByEmail(String email);

    // username으로 유저 목록을 검색 (대소문자 구분 없이)
    @Query("SELECT u FROM User u WHERE LOWER(u.username) LIKE LOWER(CONCAT('%', :username, '%')) AND u.userId != :currentUserId")
    Page<User> findByUsernameContainingIgnoreCase(@Param("username") String username, int currentUserId, Pageable pageable);
}
