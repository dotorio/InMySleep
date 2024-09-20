<script setup>
import { useUserStore } from "@/stores/user";
import { ref } from "vue";

const { VITE_VUE_IPFS_URL } = import.meta.env;

defineProps({
  nftData: Object,
});

const nftIndex = ref(4);
const uStore = useUserStore();

function nftHover(index) {
  nftIndex.value = index;
  console.log(nftIndex.value);
}

// function imgUrl(nft) {
//   return new URL(`/src/assets/collection/nft/${nft}.svg`, import.meta.url).href;
// }
function imgUrl(nft) {
  const hash = nft.imageUrl.split("ipfs://")[1];
  return new URL(`${VITE_VUE_IPFS_URL}${hash}`, import.meta.url).href;
}
</script>

<template>
  <div class="nft-con box-md">
    <div class="nft-list">
      <img :src="imgUrl(nft)" alt="lock" v-for="(nft, index) in nftData.nft" :key="index"
        :style="{ left: index * 70 + (index > nftIndex ? 100 : 0) + 'px' }" @mouseenter="nftHover(index)"
        @click="uStore.changeNft(nft)" />
    </div>
    <div></div>
  </div>
</template>

<style scoped>
.nft-con {
  width: 90%;
  height: 80%;
  margin-top: 20px;
  border-radius: 20px;
  background-color: white;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.24);
}

.nft-list {
  position: relative;
  /* background-color: blue; */
  width: 90%;
  height: 80%;
  overflow: hidden;
  /* background-color: blue; */
}

img {
  height: 100%;
  position: absolute;
  top: 0px;
  cursor: pointer;
  transition: all 0.5s ease-in-out;
  z-index: 1;
}
</style>
