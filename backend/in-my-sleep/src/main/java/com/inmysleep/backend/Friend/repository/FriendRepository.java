package com.inmysleep.backend.Friend.repository;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.entity.Friend;
import com.inmysleep.backend.user.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.Optional;

public interface FriendRepository extends JpaRepository<Friend, Long> {
    List<Friend> findAllByUserId(int userId);

    @Query("SELECT new com.inmysleep.backend.Friend.dto.FriendDto(f.friendUser.userId, f.friendUser.username, f.friendUser.email) " +
            "FROM Friend f WHERE f.userId = :userId AND f.isActive = true")
    List<FriendDto> findAllFriendsByUserId(@Param("userId") int userId);
}
