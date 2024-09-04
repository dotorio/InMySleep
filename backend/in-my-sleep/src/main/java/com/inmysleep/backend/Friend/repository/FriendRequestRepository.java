package com.inmysleep.backend.Friend.repository;

import com.inmysleep.backend.Friend.entity.FriendRequest;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface FriendRequestRepository extends JpaRepository<FriendRequest, Integer> {
    Optional<List<FriendRequest>> findByRequestUserIdAndReceiveUserIdAndIsActiveFalse(int requestUserId, int receiveUserId);
    Optional<FriendRequest> findByRequestUserIdAndReceiveUserIdAndIsActiveTrue(int requestUserId, int receiveUserId);

    Optional<List<FriendRequest>> findByRequestUserIdAndReceiveUserIdAndIsActive(int requestUserId, int receiveUserId, Boolean isActive);

    boolean existsByRequestUserIdAndReceiveUserIdAndIsActive(int requestUserId, int receiveUserId, boolean b);
}
