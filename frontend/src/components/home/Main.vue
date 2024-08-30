<script setup>
import MainHeader from "@/components/home/MainHeader.vue";
import MainLink from "@/components/home/MainLink.vue";
import { ref } from "vue";

const linkInfo = ref([
  { id: 1, header: "공지", content: "[공지] 이건 공지입니다." },
  { id: 2, header: "NFT 전시관", content: "[전시관] 이건 전시관입니다." },
  { id: 3, header: "가이드", content: "[가이드] 이건 가이드입니다." },
]);
</script>

<template>
  <div class="main-con">
    <MainHeader />
    <div class="links flex-align">
      <MainLink v-for="(link, index) in linkInfo" :key="index" :info="link" />
    </div>
  </div>
</template>

<style scoped>
.main-con {
  position: relative;
}
.links {
  width: 100%;
  position: absolute;
  bottom: 50px;

  justify-content: space-around;
}
</style>
