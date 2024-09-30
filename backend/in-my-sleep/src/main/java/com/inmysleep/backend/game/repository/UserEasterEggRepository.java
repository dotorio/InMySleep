package com.inmysleep.backend.game.repository;

import com.inmysleep.backend.game.entity.UserEasterEgg;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;

public interface UserEasterEggRepository extends CrudRepository<UserEasterEgg, Long> {
    UserEasterEgg findByUserId(int userId);

    // 중복 검사
    Optional<UserEasterEgg> findByUserIdAndMetadataId(int userId, int id);
}
