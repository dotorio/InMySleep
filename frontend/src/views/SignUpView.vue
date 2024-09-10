<script setup>
import Nav from "@/components/Nav.vue";
import { emailCheck, usernameCheck, signUp } from "@/api/user";
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const email = ref("");
const username = ref("");
const password = ref("");
const passwordConfirm = ref("");

const emailCheckVal = ref(false);
const usernameCheckVal = ref(false);
const passwordConfirmVal = ref(false);

function signUpFun() {
  if (!emailCheckVal.value || !usernameCheckVal.value) {
    Swal.fire({
      icon: "error",
      title: "중복확인을 진행해주세요!",
    });
    return;
  }

  if (!passwordConfirmVal.value) {
    Swal.fire({
      icon: "error",
      title: "비밀번호가 다릅니다",
    });
    return;
  }

  signUp({
    email: email.value,
    username: username.value,
    password: password.value,
  })
    .then((res) => {
      console.log(res.data);
      router.replace({ name: "login" });
    })
    .catch((err) => {
      console.log(err);
    });
}

function emailCheckFun() {
  emailCheck(email.value)
    .then((res) => {
      emailCheckVal.value = true;
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "이미 사용중인 이메일입니다.",
      });
    });
}

function usernameCheckFun() {
  usernameCheck(username.value)
    .then((res) => {
      usernameCheckVal.value = true;
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "이미 사용중인 닉네임입니다.",
      });
    });
}

function passwordCheck() {
  if (password.value === "") {
    return "";
  } else if (password.value === passwordConfirm.value) {
    passwordConfirmVal.value = true;
    return "비밀번호와 일치합니다!";
  } else {
    passwordConfirmVal.value = false;
    return "비밀번호와 다릅니다.";
  }
}

function emailInput() {
  emailCheckVal.value = false;
}

function usernameInput() {
  usernameCheckVal.value = false;
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
              <button
                class="check-btn bit-t"
                :class="{ check: emailCheckVal }"
                @click="emailCheckFun"
              >
                중복확인
              </button>
            </div>
            <input
              id="email"
              class="bit-t"
              type="text"
              placeholder="이메일을 입력해주세요."
              v-model="email"
              @input="emailInput"
            />
          </div>
          <div class="username box-col">
            <div class="flex-align" style="justify-content: space-between">
              <label for="username" class="bit-t">닉네임</label>
              <button
                class="check-btn bit-t"
                :class="{ check: usernameCheckVal }"
                @click="usernameCheckFun"
              >
                중복확인
              </button>
            </div>
            <input
              id="username"
              class="bit-t"
              type="text"
              placeholder="닉네임을 입력해주세요."
              v-model="username"
              @input="usernameInput"
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
            <div class="flex-align" style="justify-content: space-between">
              <label for="password-confirm" class="bit-t">비밀번호 확인</label>
              <span
                class="bit-t"
                :class="{
                  agreement: passwordConfirmVal,
                  disagreement: !passwordConfirmVal,
                }"
                >{{ passwordCheck() }}</span
              >
            </div>
            <input
              id="password-confirm"
              class="bit-t"
              type="password"
              v-model="passwordConfirm"
            />
          </div>
          <button class="btn bit-t" @click="signUpFun">회원가입</button>
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

.check {
  background-color: grey;
  border-color: grey;
}

.agreement {
  color: green;
}
.disagreement {
  color: grey;
}
</style>
