package com.inmysleep.backend.Friend.repository;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.entity.Friend;
import com.inmysleep.backend.user.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface FriendRepository extends JpaRepository<Friend, Long> {
    List<Friend> findAllByUserId(int userId);

    @Query("SELECT new com.inmysleep.backend.Friend.dto.FriendDto(f.friendUser.userId, f.friendUser.username, f.friendUser.email) " +
            "FROM Friend f WHERE f.userId = :userId AND f.isActive = true")
    List<FriendDto> findAllFriendsByUserId(@Param("userId") int userId);

    Friend findByUserIdAndFriendUserAndIsActive(int userId, User friendUser, Boolean isActive);

    boolean existsByUserIdAndFriendUserAndIsActive(int requestUserId, User receiveUser, boolean b);


    @Query("SELECT f FROM Friend f WHERE (f.userId = :userId AND f.friendUser.userId = :friendUserId) " +
            "OR (f.userId = :friendUserId AND f.friendUser.userId = :userId)")
    List<Friend> findByUserIdAndFriendUserBothWays(@Param("userId") int userId, @Param("friendUserId") int friendUserId);

    // 친구 관계 확인을 위한 쿼리
    @Query("SELECT CASE WHEN COUNT(f) > 0 THEN true ELSE false END FROM Friend f WHERE f.userId = :userId AND f.friendUser.userId = :friendUserId")
    boolean existsFriendRelation(@Param("userId") int userId, @Param("friendUserId") int friendUserId);
}
