# Runtime Stage - JAR 파일을 실행하는 런타임 환경
FROM eclipse-temurin:17-jdk
WORKDIR /app

# 빌드된 JAR 파일을 Jenkins에서 전달하여 이미지에 복사
COPY app.jar /app/app.jar

# Spring Boot 애플리케이션을 실행
ENTRYPOINT ["java", "-jar", "/app/app.jar"]

# 기본 포트 설정 (Spring Boot 기본 포트)
EXPOSE 8080
