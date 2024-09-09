# Runtime Stage - JAR 파일을 실행하는 런타임 환경
FROM eclipse-temurin:17-jdk
WORKDIR /app

# 빌드된 JAR 파일을 Jenkins 작업 공간에서 이미지로 복사
# 이때, Jenkins에서 app.jar이 $WORKSPACE 디렉터리 상위에 위치하므로 해당 경로를 참조
COPY ./app.jar /app/app.jar

# Spring Boot 애플리케이션을 실행
ENTRYPOINT ["java", "-jar", "/app/app.jar"]

# 기본 포트 설정 (Spring Boot 기본 포트)
EXPOSE 8080
