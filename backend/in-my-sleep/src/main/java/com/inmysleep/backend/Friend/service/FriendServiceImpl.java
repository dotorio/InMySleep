package com.inmysleep.backend.Friend.service;

import com.inmysleep.backend.Friend.dto.FriendDto;
import com.inmysleep.backend.Friend.dto.FriendRequestDto;
import com.inmysleep.backend.Friend.entity.Friend;
import com.inmysleep.backend.Friend.entity.FriendRequest;
import com.inmysleep.backend.Friend.repository.FriendRepository;
import com.inmysleep.backend.Friend.repository.FriendRequestRepository;
import com.inmysleep.backend.api.exception.NotFoundElementException;
import com.inmysleep.backend.user.dto.UserInfoDto;
import com.inmysleep.backend.user.entity.User;
import com.inmysleep.backend.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

@Service
public class FriendServiceImpl implements FriendService {

    private final UserRepository userRepository;
    private final FriendRepository friendRepository;
    private final FriendRequestRepository friendRequestRepository;

    public FriendServiceImpl(FriendRepository friendRepository, UserRepository userRepository, FriendRequestRepository friendRequestRepository) {
        this.friendRepository = friendRepository;
        this.userRepository = userRepository;
        this.friendRequestRepository = friendRequestRepository;
    }

    @Override
    public List<FriendDto> getAllFriends(int userId) {
        Optional<User> user = userRepository.findById(userId);
        if (user.isEmpty()) {
            throw new NotFoundElementException("User not found");
        }

        return friendRepository.findAllFriendsByUserId(userId);
    }

    @Override
    public void requestFriend(FriendRequestDto requestDto) {
        // 유효성 검사: 나에게 요청
        if (requestDto.getRequestUserId() == requestDto.getReceiveUserId()) {
            throw new IllegalArgumentException("유효하지 않은 요청입니다.");
        }

        // 유효성 검사: 중복된 요청이 있는지 확인
        if (friendRequestRepository.existsByRequestUserIdAndReceiveUserIdAndIsActive(requestDto.getRequestUserId(), requestDto.getReceiveUserId(), true)) {
            throw new IllegalArgumentException("이미 친구 요청을 보냈습니다.");
        }

        // 유효성 검사: 존재하는 유저인지 확인 및 친구 여부 확인
        User receiveUser = userRepository.findById(requestDto.getReceiveUserId())
                .orElseThrow(() -> new NotFoundElementException("User not found for receive user."));

        if (friendRepository.existsByUserIdAndFriendUserAndIsActive(requestDto.getRequestUserId(), receiveUser, true)) {
            throw new IllegalArgumentException("친구 목록에 있는 유저입니다.");
        }

        // 새로운 FriendRequest 엔티티 생성 및 값 설정
        FriendRequest friendRequest = new FriendRequest();
        friendRequest.setRequestUserId(requestDto.getRequestUserId());
        friendRequest.setReceiveUserId(requestDto.getReceiveUserId());
        friendRequest.setIsActive(true); // 기본값으로 활성화된 요청 설정
        friendRequest.setCreatedAt(LocalDateTime.now());
        friendRequest.setUpdatedAt(LocalDateTime.now());

        // 데이터베이스에 저장
        friendRequestRepository.save(friendRequest);
    }

    @Override
    public List<UserInfoDto> getReceivedRequests(int userId) {
        List<UserInfoDto> receivedRequests = friendRequestRepository.findActiveFriendRequestsByReceiveUserId(userId);
        return receivedRequests;
    }

    @Override
    public List<UserInfoDto> getSentRequests(int userId) {
        List<UserInfoDto> sentRequests = friendRequestRepository.findActiveFriendRequestsByRequestUserId(userId);
        return sentRequests;
    }

    @Override
    @Transactional
    public void acceptFriend(FriendRequestDto requestDto) {
        // 요청한 유저와 받은 유저를 DB에서 조회
        User requestUser = userRepository.findById(requestDto.getRequestUserId())
                .orElseThrow(() -> new IllegalArgumentException("Invalid user ID"));
        User receiveUser = userRepository.findById(requestDto.getReceiveUserId())
                .orElseThrow(() -> new IllegalArgumentException("Invalid user ID"));

        // 과거에 친구였던 적이 있는지 확인
        List<Friend> existingFriends = friendRepository.findByUserIdAndFriendUserBothWays(
                requestDto.getRequestUserId(), requestDto.getReceiveUserId());

        if (existingFriends.isEmpty()) {
            Friend friend1 = createFriend(requestUser, receiveUser);
            Friend friend2 = createFriend(receiveUser, requestUser);

            friendRepository.saveAll(List.of(friend1, friend2));
        } else {
            existingFriends.forEach(friend -> friend.setActive(true));
        }
    }

    private Friend createFriend(User user, User friendUser) {
        Friend friend = new Friend();
        friend.setUserId(user.getUserId());
        friend.setFriendUser(friendUser);
        friend.setActive(true);
        return friend;
    }

    @Override
    public void closeRequest(FriendRequestDto requestDto) {
        friendRequestRepository.deactivateFriendRequest(requestDto.getRequestUserId(), requestDto.getReceiveUserId());
        friendRequestRepository.deactivateFriendRequest(requestDto.getReceiveUserId(), requestDto.getRequestUserId());
    }

    @Override
    @Transactional
    public void deleteFriend(FriendRequestDto requestDto) {
        User requestUser = userRepository.findById(requestDto.getRequestUserId())
                .orElseThrow(() -> new IllegalArgumentException("Invalid user ID"));
        User receiveUser = userRepository.findById(requestDto.getReceiveUserId())
                .orElseThrow(() -> new IllegalArgumentException("Invalid user ID"));

        Friend friend = friendRepository.findByUserIdAndFriendUserAndIsActive(requestDto.getRequestUserId(), receiveUser, true);
        Friend friend2 = friendRepository.findByUserIdAndFriendUserAndIsActive(requestDto.getReceiveUserId(), requestUser, true);

        friend.setActive(false);
        friend2.setActive(false);
    }
}
