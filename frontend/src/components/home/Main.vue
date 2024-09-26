<script setup>
import MainHeader from "@/components/home/MainHeader.vue";
import MainLink from "@/components/home/MainLink.vue";
import { ref } from "vue";

const linkInfo = ref([
  {
    id: 1,
    header: "공지",
    content: "[공지] 이건 공지입니다.",
    linkName: "noti",
  },
  {
    id: 2,
    header: "NFT 전시관",
    content: "[전시관] 이건 전시관입니다.",
    linkName: "collection",
  },
  {
    id: 3,
    header: "가이드",
    content: "[가이드] 이건 가이드입니다.",
    linkName: "guied",
  },
]);
</script>

<template>
  <div class="main-con">
    <MainHeader />
    <div class="download-button-container">
      <button class="bit-t btn-download">게임 다운로드</button>
    </div>
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

.btn-download {
  height: 100%;
  padding: 10px 20px;
  margin-top: 20px;
  border-radius: 10px;
  border-width: 5px;
  font-size: 30px;
  border-color: #1B4F72;
  background-color: #2980b9;
  color: white;
  cursor: pointer;
}

.download-button-container {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -70%);
}
</style>
