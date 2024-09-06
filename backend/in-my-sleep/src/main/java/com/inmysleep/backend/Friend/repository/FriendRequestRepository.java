package com.inmysleep.backend.Friend.repository;

import com.inmysleep.backend.Friend.entity.FriendRequest;
import com.inmysleep.backend.user.dto.UserInfoDto;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.Optional;

public interface FriendRequestRepository extends JpaRepository<FriendRequest, Integer> {
    Optional<List<FriendRequest>> findByRequestUserIdAndReceiveUserIdAndIsActiveFalse(int requestUserId, int receiveUserId);
    Optional<FriendRequest> findByRequestUserIdAndReceiveUserIdAndIsActiveTrue(int requestUserId, int receiveUserId);

    Optional<List<FriendRequest>> findByRequestUserIdAndReceiveUserIdAndIsActive(int requestUserId, int receiveUserId, Boolean isActive);

    boolean existsByRequestUserIdAndReceiveUserIdAndIsActive(int requestUserId, int receiveUserId, boolean b);

    @Query("SELECT new com.inmysleep.backend.user.dto.UserInfoDto(u.userId, u.username, u.email, u.lastStage) " +
            "FROM FriendRequest fr JOIN User u ON fr.requestUserId = u.userId " +
            "WHERE fr.receiveUserId = :receiveUserId AND fr.isActive = true")
    List<UserInfoDto> findActiveFriendRequestsByReceiveUserId(int receiveUserId);

    @Query("SELECT new com.inmysleep.backend.user.dto.UserInfoDto(u.userId, u.username, u.email, u.lastStage) " +
            "FROM FriendRequest fr JOIN User u ON fr.receiveUserId = u.userId " +
            "WHERE fr.requestUserId = :requestUserId AND fr.isActive = true")
    List<UserInfoDto> findActiveFriendRequestsByRequestUserId(int requestUserId);

    @Modifying
    @Transactional
    @Query("UPDATE FriendRequest fr SET fr.isActive = false WHERE fr.requestUserId = :requestUserId AND fr.receiveUserId = :receiveUserId")
    void deactivateFriendRequest(@Param("requestUserId") int requestUserId, @Param("receiveUserId") int receiveUserId);
}
