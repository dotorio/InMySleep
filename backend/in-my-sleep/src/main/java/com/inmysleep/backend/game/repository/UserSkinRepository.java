package com.inmysleep.backend.game.repository;

import com.inmysleep.backend.game.entity.UserSkin;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface UserSkinRepository extends JpaRepository<UserSkin, Long> {
    UserSkin findByUserId(int userId);
}
