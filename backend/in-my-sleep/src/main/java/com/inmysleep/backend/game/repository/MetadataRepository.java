package com.inmysleep.backend.game.repository;

import com.inmysleep.backend.game.entity.Metadata;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;

public interface MetadataRepository extends CrudRepository<Metadata, Integer> {
    // 조건 없이 전체 메타데이터 개수를 카운트하는 메서드
    long count();

    // 페이징을 통해 메타데이터를 가져오는 메서드
    Page<Metadata> findAll(Pageable pageable);
}
