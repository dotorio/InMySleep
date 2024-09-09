# Runtime Stage - JAR 파일을 실행하는 런타임 환경
FROM eclipse-temurin:17-jdk
WORKDIR /app

# Jenkins에서 복사된 JAR 파일을 Docker 이미지로 복사
COPY app.jar /app/app.jar

# JAR 파일에 실행 권한을 부여
RUN chmod +x /app/app.jar

# Spring Boot 애플리케이션을 실행
ENTRYPOINT ["java", "-jar", "/app/app.jar"]

# 기본 포트 설정 (Spring Boot 기본 포트)
EXPOSE 8080
