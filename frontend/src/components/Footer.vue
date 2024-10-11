<script setup></script>

<template>
  <div class="footer-con flex-align">
    <img src="/src/assets/name.svg" alt="팀명" />
    <div class="info bit-t">
      <div>
        <span>(주)어이!김윤손 |</span>
        <span> 대표이사: 김영수 | </span>
        <span>부산 강서구 녹산산업중로 333 삼성전기부산사업장</span>
      </div>
      <div style="margin-top: 10px">
        <span>사업자 등록번호 1111111 | </span>
        <span>Tel 010-1234-5678</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.footer-con {
  height: 100px;
  background-color: rgb(42, 41, 41);
  justify-content: center;
}
img {
  width: 120px;
}
.info {
  color: white;
  margin-left: 40px;
}
</style>
