<script setup>
import Nav from "@/components/Nav.vue";
import Footer from "@/components/Footer.vue";
import { ref } from "vue";

const categoryNum = ref(1);
const page = ref(1);

function selectCategory(categoryIdx) {
  categoryNum.value = categoryIdx;
  page.value = 1;
}

function nextPage() {
  page.value += 1;

  if (categoryNum.value == 1) {
    if (page.value > 2) {
      page.value = 2;
    }
  } else if (categoryNum.value == 2) {
    if (page.value > 4) {
      page.value = 4;
    }
  } else if (categoryNum.value == 3) {
    if (page.value > 5) {
      page.value = 5;
    }
  } else if (categoryNum.value == 41) {
    if (page.value > 4) {
      page.value = 4;
    }
  } else if (categoryNum.value == 42) {
    if (page.value > 4) {
      page.value = 4;
    }
  }
}

function prevPage() {
  page.value -= 1;

  if (page.value <= 0) {
    page.value = 1;
  }
}

function guideBookFun(categoryIdx, page) {
  switch (categoryIdx) {
    case 1:
      switch (page) {
        case 1:
          return new URL("@/assets/guide/1/1.PNG", import.meta.url).href;
        case 2:
          return new URL("@/assets/guide/1/2.PNG", import.meta.url).href;
      }
    case 2:
      switch (page) {
        case 1:
          return new URL("@/assets/guide/2/1.PNG", import.meta.url).href;
        case 2:
          return new URL("@/assets/guide/2/2.PNG", import.meta.url).href;
        case 3:
          return new URL("@/assets/guide/2/3.PNG", import.meta.url).href;
        case 4:
          return new URL("@/assets/guide/2/4.PNG", import.meta.url).href;
      }
    case 3:
      switch (page) {
        case 1:
          return new URL("@/assets/guide/3/1.PNG", import.meta.url).href;
        case 2:
          return new URL("@/assets/guide/3/2.PNG", import.meta.url).href;
        case 3:
          return new URL("@/assets/guide/3/3.PNG", import.meta.url).href;
        case 4:
          return new URL("@/assets/guide/3/4.PNG", import.meta.url).href;
        case 5:
          return new URL("@/assets/guide/3/5.PNG", import.meta.url).href;
      }
    case 41:
      switch (page) {
        case 1:
          return new URL("@/assets/guide/41/1.PNG", import.meta.url).href;
        case 2:
          return new URL("@/assets/guide/41/2.PNG", import.meta.url).href;
        case 3:
          return new URL("@/assets/guide/41/3.PNG", import.meta.url).href;
        case 4:
          return new URL("@/assets/guide/41/4.PNG", import.meta.url).href;
      }
    case 42:
      switch (page) {
        case 1:
          return new URL("@/assets/guide/42/1.PNG", import.meta.url).href;
        case 2:
          return new URL("@/assets/guide/42/2.PNG", import.meta.url).href;
        case 3:
          return new URL("@/assets/guide/42/3.PNG", import.meta.url).href;
        case 4:
          return new URL("@/assets/guide/42/4.PNG", import.meta.url).href;
      }
    default:
      return new URL("@/assets/guide/1/1.PNG", import.meta.url).href;
  }
}
</script>

<template>
  <div>
    <Nav />
    <div class="background box-md">
      <div class="guide-con">
        <span class="big-text bit-t">가이드 북</span>
        <div class="guide-category flex-align">
          <button class="guide-btn bit-t" @click="selectCategory(1)">
            스테이지1
          </button>
          <button class="guide-btn bit-t" @click="selectCategory(2)">
            스테이지2
          </button>
          <button class="guide-btn bit-t" @click="selectCategory(3)">
            스테이지3
          </button>
          <button class="guide-btn bit-t" @click="selectCategory(41)">
            스테이지4-1
          </button>
          <button class="guide-btn bit-t" @click="selectCategory(42)">
            스테이지4-2
          </button>
        </div>
        <div class="guide-main box-md">
          <img
            :src="guideBookFun(categoryNum, page)"
            alt="가이드북"
            class="bg-img"
          />
        </div>
        <div class="guide-page box-md">
          <button class="page-btn bit-t" @click="prevPage">이전</button>
          <button class="page-btn bit-t" @click="nextPage">다음</button>
        </div>
      </div>
    </div>
    <Footer />
  </div>
</template>

<style scoped>
.background {
  height: 800px;
  background-color: #d4f1f9;
}

.big-text {
  font-size: 35px;
}

.guide-category {
  margin-top: 20px;
}

.guide-btn,
.page-btn {
  margin: 10px;
  font-size: 15px;

  color: white;
  background-color: #1f1a59;

  border-radius: 10px;
  border-width: 3px;
  border-color: #2d2866;
}

.guide-btn {
  width: 120px;
  height: 60px;
}

.page-btn {
  width: 80px;
  height: 40px;
}

.guide-main {
  width: 100%;
  height: 400px;
  margin-top: 10px;
  background-color: white;

  position: relative;
}

.bg-img {
  width: 90%;
  height: 100%;
  object-fit: cover; /* 이미지 비율을 유지하면서 부모 요소를 꽉 채우기 */

  z-index: 0; /* 오버레이보다 뒤에 위치하도록 설정 */
}
</style>
