<script setup>
import Nav from "@/components/Nav.vue";
import { emailCheck } from "@/api/user";
import { ref } from "vue";

const email = ref("");
const username = ref("");
const password = ref("");
const passwordConfirm = ref("");

const emailCheckVal = ref(false);
const usernameCheckVal = ref(false);

function signUp() {
  console.log(email.value);
  console.log(username.value);
  console.log(password.value);
  console.log(passwordConfirm.value);
}

function emailCheckFun() {
  emailCheck({
    email: email.value,
  })
    .then((res) => {
      console.log(res.data);
    })
    .catch((err) => {
      console.log(err);
    });
}
</script>

<template>
  <div>
    <Nav />

    <div class="background box-md">
      <div class="signup-con box-col">
        <span class="big-text bit-t">회원가입</span>
        <div class="input-con box-col">
          <div class="email box-col">
            <div class="flex-align" style="justify-content: space-between">
              <label for="email" class="bit-t">이메일</label>
              <button class="check-btn bit-t" @click="emailCheckFun">
                중복확인
              </button>
            </div>
            <input
              id="email"
              class="bit-t"
              type="text"
              placeholder="이메일을 입력해주세요."
              v-model="email"
            />
          </div>
          <div class="username box-col">
            <div class="flex-align" style="justify-content: space-between">
              <label for="username" class="bit-t">닉네임</label>
              <button class="check-btn bit-t">중복확인</button>
            </div>
            <input
              id="username"
              class="bit-t"
              type="text"
              placeholder="닉네임을 입력해주세요."
              v-model="username"
            />
          </div>
          <div class="password box-col">
            <label for="password" class="bit-t">비밀번호</label>
            <input
              id="password"
              class="bit-t"
              type="password"
              v-model="password"
            />
            <label for="password-confirm" class="bit-t">비밀번호 확인</label>
            <input
              id="password-confirm"
              class="bit-t"
              type="password"
              v-model="passwordConfirm"
            />
          </div>
          <button class="btn bit-t" @click="signUp">회원가입</button>
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
.signup-con {
  width: 650px;
  height: 550px;
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

.btn {
  height: 50px;
  margin-top: 20px;

  border-radius: 10px;
  border-width: 5px;
  border-color: #211d54;
  background-color: #1f1a59;
  color: white;
  font-size: 25px;
}

.check-btn {
  padding: 3px 5px;
  border-radius: 10px;
  border-width: 3px;
  border-color: #211d54;
  background-color: #1f1a59;
  color: white;
  font-size: 15px;
}

button {
  cursor: pointer;
}
</style>
