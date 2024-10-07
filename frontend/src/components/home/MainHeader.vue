<script setup></script>

<template>
  <div class="header-con">
    <img src="@/assets/home/homeback.gif" class="bg-img" />
    <div class="overlay"></div>
  </div>
</template>

<style scoped>
.header-con {
  height: 1000px;
  width: 100%;
  background-color: #d4f1f9;
  position: relative;
  overflow: hidden; /* 이미지가 부모 요소를 벗어날 경우 숨김 처리 */
}

.bg-img {
  width: 100%;
  height: 100%;
  object-fit: cover; /* 이미지 비율을 유지하면서 부모 요소를 꽉 채우기 */
  position: absolute; /* 부모 요소 내에서 고정 위치 */
  top: 0;
  left: 0;
  z-index: 0; /* 오버레이보다 뒤에 위치하도록 설정 */
}

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(128, 128, 128, 0.5); /* 회색 반투명 */
  z-index: 0; /* 이미지 위에 위치하도록 설정 */
}
</style>
