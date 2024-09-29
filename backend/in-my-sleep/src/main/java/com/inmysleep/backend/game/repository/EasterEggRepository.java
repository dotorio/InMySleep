package com.inmysleep.backend.game.repository;

import com.inmysleep.backend.game.entity.EasterEgg;
import org.springframework.data.jpa.repository.JpaRepository;

public interface EasterEggRepository extends JpaRepository<EasterEgg, Long> {
}
