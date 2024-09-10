<script setup>
import { useUserStore } from "@/stores/user";
import { useRouter } from "vue-router";
import { logout } from "@/api/user";

const router = useRouter();
const uStore = useUserStore();

function logoutFun() {
  logout()
    .then((res) => {
      console.log(res.data);
      uStore.userLogout();
    })
    .catch((err) => {
      console.log(err);
    });
}
</script>

<template>
  <div class="nav-con flex-align">
    <div class="left-side">
      <img
        src="/src/assets/home/logo.gif"
        alt="logo"
        class="logo"
        @click="router.push({ name: 'home' })"
      />
    </div>

    <div class="right-side">
      <div v-if="!uStore.user">
        <span class="bit-t account" @click="router.push({ name: 'login' })"
          >로그인</span
        >
        <span class="bit-t account" @click="router.push({ name: 'signup' })"
          >회원가입</span
        >
      </div>
      <div v-else>
        <span class="bit-t account" @click="logoutFun">로그아웃</span>
      </div>
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

  cursor: pointer;
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
