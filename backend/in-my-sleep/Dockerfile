# Runtime Stage - JAR 파일을 실행하는 런타임 환경
FROM eclipse-temurin:17-jdk
WORKDIR /app

# Spring Boot 애플리케이션을 실행
ENTRYPOINT ["java", "-jar", "./app.jar"]

# 기본 포트 설정 (Spring Boot 기본 포트)
EXPOSE 8080
