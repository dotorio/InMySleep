<script setup>
import { useUserStore } from "@/stores/user";
import { useSkinStore } from "@/stores/skin";
import { ref } from "vue";
const { VITE_VUE_IPFS_URL } = import.meta.env;

defineProps({
  nftData: Object,
});

const nftIndex = ref(4);
const uStore = useUserStore();
const sStore = useSkinStore();

function nftHover(index) {
  nftIndex.value = index;
}

function imgUrl(nft) {
  if (!nft.imageUrl) {
    return "";
  }
  if (nft.imageUrl === "none") {
    return new URL(`/src/assets/collection/nft/lock-nft.svg`, import.meta.url).href;
  }
  const url = nft.imageUrl;
  return new URL(`${url}`, import.meta.url).href;
}

function changeSkin(selectedSkin) {
  if (!selectedSkin.attributes) {
    Swal.fire({
      icon: "error",
      title: "NFT를 가지고 있지 않습니다",
      text: "스토리를 클리어하면서 NFT를 수집해보세요!",
    });
    return;
  }
  if (sStore.userSkin.choice === "bear") {
    sStore.userSkin.selectedBearMetadata = selectedSkin.id;
  } else if (sStore.userSkin.choice === "rabbit") {
    sStore.userSkin.selectedRabbitMetadata = selectedSkin.id;
  }
}
</script>

<template>
  <div class="nft-con box-md">
    <div class="nft-list">
      <img :src="imgUrl(nft)" alt="lock" v-for="(nft, index) in nftData.nft" :key="index"
        :style="{ left: index * 70 + (index > nftIndex ? 100 : 0) + 'px' }" @mouseenter="nftHover(index)"
        @click="changeSkin(nft)" />
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
