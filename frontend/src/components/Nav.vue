<script setup>
import { onMounted, ref } from "vue";
import { useUserStore } from "@/stores/user";
import { useRouter } from "vue-router";
import { logout } from "@/api/user";
import { walletAuth } from "@/api/wallet";

const router = useRouter();
const uStore = useUserStore();

function logoutFun() {
  logout()
    .then((res) => {
      uStore.userLogout();
      router.push({ name: "home" });
    })
    .catch((err) => {
      console.log(err);
    });
}

async function connectWallet() {

  if (uStore.user.data.address && uStore.user.data.address !== "" && uStore.user.data.metamaskToken && uStore.user.data.metamaskToken !== "") {
    console.log("Already connected");
    return;
  }

  if (typeof window.ethereum !== "undefined") {
    console.log("MetaMask is installed!");
    window.ethereum.on("chainChanged", () => {
      window.location.reload();
    });
  } else {
    Swal.fire({
      icon: "error",
      title: "MetaMask을 설치해주세요",
    });
    return
  }

  try {
    const accounts = await window.ethereum.request({
      method: "eth_requestAccounts",
    });
    const message = "Connect with MetaMask";
    const signature = await signMessage(accounts[0], message);

    const response = await walletAuth(
      accounts[0],
      signature,
      message,
      uStore.user.data.username,
    );

    if (response.data.token) {
      uStore.user.data['metamaskToken'] = response.data.token;
      uStore.user.data['address'] = accounts[0];
      window.location.reload();
    }

    console.log("Connected", accounts);
    // const provider = new ethers.BrowserProvider(window.ethereum);
    // await provider.send("eth_requestAccounts", []);
    // const signer = await provider.getSigner();
    // uStore.user['address'] = await signer.getAddress();
    // console.log("Connected", uStore.user.address);

  } catch (error) {
    console.error(error);
  }
}

async function signMessage(account, message) {
  try {
    const signature = await window.ethereum.request({
      method: "personal_sign",
      params: [message, account],
    });
    console.log("Signature", signature);
    return signature;
  } catch (error) {
    console.error(error);
  }
}
</script>

<template>
  <div class="nav-con flex-align">
    <div class="left-side">
      <img src="/src/assets/home/logo.gif" alt="logo" class="logo" @click="router.push({ name: 'home' })" />
    </div>

    <div class="right-side">
      <div v-if="!uStore.user">
        <span class="bit-t account" @click="router.push({ name: 'login' })">로그인</span>
        <span class="bit-t account" @click="router.push({ name: 'signup' })">회원가입</span>
      </div>
      <div v-else>
        <span v-if="uStore.user.data.metamaskToken" class="bit-t account connect-wallet">지갑 연동 완료</span>
        <span v-else class="bit-t account" @click="connectWallet">지갑 연동</span>
        <span class="bit-t account" @click="logoutFun">로그아웃</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.nav-con {
  height: 8vh;
  justify-content: space-between;
  background-color: #1f1a59;
}

.logo {
  /* width: 100px; */
  height: 8vh;
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

.connect-wallet {
  color: #6f63f5;
  cursor: not-allowed;
}
</style>
