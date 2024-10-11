package com.inmysleep.backend.game.repository;

import com.inmysleep.backend.game.entity.Room;
import org.springframework.data.jpa.repository.JpaRepository;

public interface RoomRepository extends JpaRepository<Room, Integer> {

}
