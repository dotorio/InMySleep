<script setup>
import { useUserStore } from "@/stores/user";
const { VITE_VUE_IPFS_URL } = import.meta.env;

const uStore = useUserStore();

function getNftSrc(character, color) {
  // return new URL(`/src/assets/collection/nft/${nft}.svg`, import.meta.url).href;
  let hash = "";
  if (uStore.userBearSkin.length === 0 || uStore.userRabbitSkin === 0) {
    return "";
  }
  if (character === "bear") {
    hash = uStore.userBearSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
    console.log(hash);
  } else if (character === "rabbit") {
    hash = uStore.userRabbitSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
    console.log(hash)
  }
  return new URL(`${VITE_VUE_IPFS_URL}${hash}`, import.meta.url).href;
}
</script>

<template>
  <div class="user-con box-col">
    <div class="nickname bit-t">{{ uStore.userInfo.nickname }}</div>
    <img v-if="uStore.userInfo.choice === 'bear'" :src="getNftSrc('bear', uStore.userInfo.bearColor)" alt="곰" />
    <img v-else-if="uStore.userInfo.choice === 'rabbit'" :src="getNftSrc('rabbit', uStore.userInfo.rabbitColor)"
      alt="토끼" />
  </div>
</template>

<style scoped>
.user-con {
  width: 30%;
  height: 100%;

  border-radius: 20px;
  background-color: white;
}

.nickname {
  font-size: 30px;
  margin: 20px 30px;
}

img {
  height: 70%;
}
</style>
