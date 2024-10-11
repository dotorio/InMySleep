<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

defineProps({
  info: Object,
});

function imgUrl(info) {
  const id = info.id;

  switch (id) {
    case 1:
      return new URL("@/assets/home/link/img1.png", import.meta.url).href;
    case 2:
      return new URL("@/assets/home/link/img2.jpg", import.meta.url).href;
    case 3:
      return new URL("@/assets/home/link/img3.png", import.meta.url).href;
    default:
      return new URL("@/assets/home/link/img1.png", import.meta.url).href;
  }
}

function goLink(linkName) {
  router.push({ name: linkName });
}
</script>

<template>
  <div class="link-con" @click="goLink(info.linkName)">
    <div class="header flex-align">
      <div class="title bit-t">
        {{ info.header }}
      </div>
    </div>
    <div class="link-img">
      <img :src="imgUrl(info)" alt="이미지" class="img" />
    </div>
    <div class="content bit-t">
      {{ info.content }}
    </div>
  </div>
</template>

<style scoped>
.link-con {
  width: 400px;
  height: 350px;
  border-radius: 15px;

  position: relative;
  overflow: hidden;
  cursor: pointer;
  background-color: rgb(195, 195, 195);

  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
  transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
}

.link-con:hover {
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.25), 0 2px 4px rgba(0, 0, 0, 0.22);
}

.header {
  height: 60px;

  background: rgb(26, 54, 133);
}

.title {
  font-size: 25px;
  margin-left: 20px;

  color: white;
}
.img {
  width: 100%;
}

.content {
  width: 100%;
  height: 90px;

  font-size: 25px;
  padding: 15px;
  background-color: rgba(255, 255, 255, 0.731);
  position: absolute;
  bottom: 0px;
}
</style>
