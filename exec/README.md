# 🎮 In My Sleep 포팅 매뉴얼

## Unity

### Version

- **게임 엔진**: Unity `2022.3.42f1`
- **실시간 통신**: PUN `2.47`, Photon Chat `2.17`

### 실행 방법

- 유니티 Editor에서 빌드
    - 기본 설정되어 있는 씬을 그대로 빌드

## 인프라

- **웹 서버**: Nginx
- **가상화** : Docker
- **CI/CD** : Jenkins

## Back-end

### Version

- **런타임 환경**: `Java 17.0.12`
- **프레임워크**: SpringBoot
- **DB 모델링**
    - **ORM 라이브러리**: JPA (Hibernate)
    - **Dialect**: MySQL 8.0.37
- **캐싱**: Redis 7.4.0

### 실행 방법
프로젝트에 .env 파일 설정 후 Spring 프로젝트 실행
```env
# .env 파일
# MySQL 설정
DB_URL=jdbc:mysql:/DBURL:3306/e107?serverTimezone=Asia/Seoul&characterEncoding=UTF-8
DB_USERNAME=
DB_PASSWORD=

# 이메일 정보 SMTP
EMAIL_USERNAME=
EMAIL_PASSWORD=

# Redis 설정
REDIS_HOST=
REDIS_PORT=6379
REDIS_PASSWORD=
```

## DB
`/dump` 디렉터리 참조
- **게임 데이터 DB**: MySQL
- **이메일 인증 DB**: Redis

## Front-end

### Version

- **런타임 환경:** Node.js `20.17.0`
- **프레임워크**: Vue.js `3.4.29`

### 실행 방법

```terminal
npm install
npm run build
```

## Blockchain

### Version

- Express.js: '4.20.0'
- ethers: 6.13.2
- web3: 4.12.1
- MetaMask
- Polygon
- Truffle 5.11.5
- IPFS 0.30.0

### 실행 방법
```
npm run dev
```

## Photon Server Setting

1. https://www.photonengine.com/
2. 로그인 후 Create New Application
3. Multiplayer Game을 선택하고 프로젝트 생성 AppId 복사
4. 유니티 -> 포톤 유니티 네트워킹 -> PUN Wizard -> Setup Project -> 복사한 AppId 입력 후 Setup Project

## 시연 시나리오

1. 로그인 및 회원가입
![image.png](./img/02_GameStart.png)


2. 게임시작 화면
![image.png](./img/02_GameStart.png)


3. 로비 화면
![image-1.png](./img/02_GameStart.png)


4. 친구 요청 및 수락
![image-2.png](./img/02_GameStart.png)


5. 게임 시작
![image-3.png](./img/02_GameStart.png)


6. 스테이지 진행
![image-4.png](./img/02_GameStart.png)


7. 엔딩
![image-5.png](./img/02_GameStart.png)
