<script setup>
import Nav from "@/components/Nav.vue";
import Footer from "@/components/Footer.vue";
import {
  emailUserAuth,
  emailChangePasswordCode,
  emailChangePasswordCodeCheck,
  changeEmailPassword,
} from "@/api/user";
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const email = ref("");
const emailConfirm = ref("");
const emailAuthNum = ref("");
const password = ref("");
const passwordConfirm = ref("");

const emailCheckVal = ref(false);
const emailAuthVal = ref(false);  // 이메일 인증이 완료된 경우 true
const passwordConfirmVal = ref(false);


function changeFun() {
  if (!emailCheckVal.value) {
    Swal.fire({
      icon: "error",
      title: "이메일 인증을 진행해주세요!",
    });
    return;
  }

  if (!passwordConfirmVal.value) {
    Swal.fire({
      icon: "error",
      title: "비밀번호가 다릅니다.",
    });
    return;
  }

  if (!emailAuthVal.value) {
    Swal.fire({
      icon: "error",
      title: "이메일 인증을 해주세요!",
    });
    return;
  }

  // 비밀번호 변경
  changeEmailPassword({
    email: email.value,
    password: password.value,
  })
    .then((res) => {
      console.log(res.data);
      Swal.fire({
        icon: "success",
        title: "비밀번호 변경 완료!",
      });
      router.replace({ name: "login" });
    })
    .catch((err) => {
      console.log(err);
      Swal.fire({
        icon: "error",
        title: "알림",
        text: err.response.data.message || "비밀번호 변경 중 에러가 발생했습니다.",
      });
    });
}

function emailAuthFun() {
  const useremail = email.value;
  const authNum = emailAuthNum.value;

  emailChangePasswordCodeCheck(useremail, authNum)
    .then((res) => {
      console.log(res.data);
      emailAuthVal.value = true;
    })
    .catch((err) => {
      console.log(err);
      Swal.fire({
        icon: "error",
        title: "인증에 실패했습니다.",
        text: "이메일 또는 인증 번호를 확인해 주세요."
      });
    });
}

function emailCheckFun() {
  if (email.value === "") {
    Swal.fire({
      icon: "error",
      text: "이메일을 입력해주세요.",
    });
    return;
  }

  if (email.value.indexOf("@") === -1) {
    Swal.fire({
      icon: "error",
      text: "올바른 이메일 형식을 입력해주세요.",
    });
    return;
  }

  emailUserAuth(email.value)
    .then((res) => {
      emailCheckVal.value = true;
      emailConfirm.value = "인증 번호를 확인해주세요.";
      console.log("가입한 유저");

      // 이메일 인증 요청
      emailChangePasswordCode(email.value)
        .then((res) => {
          console.log(res.data);
        })
        .catch((err) => {
          // 이메일 인증에 실패한 경우
          Swal.fire({
            icon: "error",
            text: "이메일 인증에 실패했습니다. 다시 시도해주세요.",
          });
          // 상태 초기화
          emailCheckVal.value = false;
          emailAuthVal.value = false;
          emailConfirm.value = "";
        });
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        text: "등록되지 않은 유저 입니다.",
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
  emailAuthVal.value = false;
  emailConfirm.value = "";
}
</script>

<template>
  <div>
    <Nav />

    <div class="background box-md">
      <div class="signup-con box-col box-md">
        <span class="big-text bit-t">비밀번호 변경</span>
        <div class="input-con box-col">
          <!-- 이메일 입력 필드 -->
          <div class="email box-col">
            <div class="flex-align" style="justify-content: space-between">
              <label for="email" class="bit-t">이메일</label>
              <div class="flex-align">
                <div class="email-confirm bit-t">
                  {{ emailConfirm }}
                </div>
                <button
                  class="check-btn bit-t"
                  :class="{ check: emailCheckVal }"
                  @click="emailCheckFun"
                >
                  인증받기
                </button>
              </div>
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

          <!-- 이메일 인증 완료 시 인증번호 입력 필드 표시 -->
          <div class="username box-col" v-if="emailCheckVal">
            <div class="flex-align" style="justify-content: space-between">
              <label for="username" class="bit-t">인증번호</label>
            </div>
            <div class="flex-align" style="justify-content: space-between">
              <input
                id="emailAuth"
                class="bit-t"
                type="text"
                placeholder="인증번호"
                v-model="emailAuthNum"
                maxlength="6"
              />
              <div>
                <span class="isAuth bit-t" v-if="emailAuthVal">인증완료!</span>
                <button
                  class="check-btn bit-t"
                  :class="{ check: emailAuthVal }"
                  @click="emailAuthFun"
                >
                  인증하기
                </button>
              </div>
            </div>
          </div>

          <!-- 인증번호 인증 완료 시 비밀번호 입력 필드 표시 -->
          <div class="password box-col" v-if="emailAuthVal">
            <label for="password" class="bit-t">새로운 비밀번호</label>
            <input
              id="password"
              class="bit-t"
              type="password"
              v-model="password"
            />
            <div class="flex-align" style="justify-content: space-between">
              <label for="password-confirm" class="bit-t">새로운 비밀번호 확인</label>
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

          <!-- 비밀번호 입력 완료 시 비밀번호 변경 버튼 표시 -->
          <button
            class="btn bit-t"
            v-if="passwordConfirmVal"
            @click="changeFun"
          >
            비밀번호 변경
          </button>
        </div>
      </div>
    </div>
    <Footer />
  </div>
</template>

<style scoped>
.background {
  height: 85vh;
  background-color: #D4F1F9;
}

.signup-con {
  width: 650px;
  align-items: center;
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

#emailAuth {
  text-align: center;
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

.email-confirm,
.isAuth {
  color: rgb(48, 164, 48);
  margin-right: 20px;
}

.agreement {
  color: green;
}

.disagreement {
  color: grey;
}
</style>
