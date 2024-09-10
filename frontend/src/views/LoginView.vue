<script setup>
import Nav from "@/components/Nav.vue";
import { login } from "@/api/user";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useUserStore } from "@/stores/user";

const router = useRouter();
const uStore = useUserStore();
const email = ref("");
const password = ref("");

function loginFun() {
  login({
    email: email.value,
    password: password.value,
  })
    .then((res) => {
      // console.log(res.data);
      uStore.userLogin(res.data.data);
      router.replace({ name: "home" });
    })
    .catch((err) => {
      console.log(err);
      Swal.fire({
        icon: "error",
        title: "로그인에 실패했습니다.",
        text: "이메일과 비밀번호를 확인해주세요!",
      });
    });
}
</script>

<template>
  <div>
    <Nav />
    <div class="background box-md">
      <div class="login-con box-col">
        <span class="big-text bit-t">로그인</span>
        <div class="input-con box-col">
          <div class="email box-col">
            <label for="email" class="bit-t">이메일</label>
            <input
              id="email"
              class="bit-t"
              type="text"
              placeholder="이메일을 입력해주세요."
              v-model="email"
            />
          </div>
          <div class="password box-col">
            <label for="password" class="bit-t">비밀번호</label>
            <input
              id="password"
              class="bit-t"
              type="password"
              placeholder="비밀번호를 입력해주세요."
              v-model="password"
            />
            <a href="#" class="bit-t password-link"
              >비밀번호를 잊어버리셨나요?</a
            >
          </div>
          <button class="login-btn bit-t" @click="loginFun">로그인</button>
          <button
            class="signup-btn bit-t"
            @click="router.push({ name: 'signup' })"
          >
            회원가입
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.background {
  height: 85vh;
  background-color: aqua;
}
.login-con {
  width: 650px;
  height: 480px;
  align-items: center;
  /* background-color: blue; */
}

.input-con {
  width: 100%;
  height: 90%;
  margin-top: 10px;
  padding: 30px 70px;

  background-color: white;
  border-radius: 20px;
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}

.big-text {
  font-size: 35px;
}

label {
  font-size: 20px;
}
.input-con input {
  padding-left: 20px;
  font-size: 20px;
  margin: 10px 0;
  height: 40px;
}
.password-link {
  text-decoration: none;
  margin-top: 10px;
}
.password-link:hover {
  color: rgb(74, 74, 247);
}

button {
  height: 50px;
  margin-top: 20px;
  border-radius: 10px;
  border-width: 5px;
  font-size: 20px;
}
.login-btn {
  border-color: #211d54;
  background-color: #1f1a59;
  color: white;
}
.signup-btn {
  color: #1f1a59;
  border-color: #1f1a59;
  background-color: white;
}
</style>
