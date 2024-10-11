package com.inmysleep.backend.Friend.entity;

import com.inmysleep.backend.user.entity.User;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@ToString
@Table(name = "friend")
public class Friend {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int friendId;

    private int userId;

    @ManyToOne
    @JoinColumn(name = "friend_user_id", referencedColumnName = "userId")
    private User friendUser;

    private boolean isActive;

    private LocalDateTime createdAt;

    private LocalDateTime updatedAt;

    // 엔티티가 처음 저장되기 전에 호출되는 메서드
    @PrePersist
    protected void onCreate() {
        this.createdAt = LocalDateTime.now();
    }

    // 엔티티가 업데이트되기 전에 호출되는 메서드
    @PreUpdate
    protected void onUpdate() {
        this.updatedAt = LocalDateTime.now();
    }
}
