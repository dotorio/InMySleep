package com.inmysleep.backend.Friend.repository;

import com.inmysleep.backend.Friend.entity.FriendRequest;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface FriendRequestRepository extends JpaRepository<FriendRequest, Integer> {
    Optional<FriendRequest> findByRequestUserIdAndReceiveUserIdAndIsActive(int requestUserId, int receiveUserId, boolean isActive);
}
