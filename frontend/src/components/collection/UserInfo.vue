<script setup>
import { useUserStore } from "@/stores/user";

const uStore = useUserStore();
</script>

<template>
  <div class="user-con box-col">
    <div class="nickname bit-t">{{ uStore.userInfo.nickname }}</div>
    <img
      :src="'/src/assets/collection/nft/' + uStore.userInfo.nft + '.svg'"
      alt="락"
    />
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
