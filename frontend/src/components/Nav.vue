<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

function goSignUp() {
  router.push({ name: "signup" });
}
</script>

<template>
  <div class="nav-con flex-align">
    <div class="left-side">
      <img src="/src/assets/home/logo.gif" alt="logo" class="logo" />
    </div>

    <div class="right-side">
      <span class="bit-t account">로그인</span>
      <span class="bit-t account" @click="goSignUp">회원가입</span>
    </div>
  </div>
</template>

<style scoped>
.nav-con {
  height: 15vh;
  justify-content: space-between;
  background-color: #1f1a59;
}
.logo {
  width: 100px;
  margin-left: 10px;
}
.account {
  font-size: 25px;
  color: white;
  margin-right: 20px;

  cursor: pointer;
}
.account:hover {
  color: #6f63f5;
}
</style>
