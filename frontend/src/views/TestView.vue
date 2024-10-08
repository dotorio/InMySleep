<script setup>
import Nav from "@/components/Nav.vue";
import Footer from "@/components/Footer.vue";
import skinData from "@/assets/data/skin.json";
import { ref } from "vue";

const currentSkin = ref(3);
const spacing = ref(0);

function skinScale(idx) {
  // console.log(idx);

  if (idx == currentSkin.value) {
    return "skin-center";
  } else if (idx == currentSkin.value + 1) {
    return "skin-right";
  } else if (idx == currentSkin.value - 1) {
    return "skin-left";
  } else {
    return "";
  }
}

function nextBtn() {
  currentSkin.value += 1;

  if (currentSkin.value === 16) {
    currentSkin.value = 15;
  } else {
    spacing.value -= 150;
  }

  console.log(spacing.value);
}

function prevBtn() {
  currentSkin.value -= 1;

  if (currentSkin.value === 0) {
    currentSkin.value = 1;
  } else {
    spacing.value += 150;
  }
}

function positionCalc(idx) {
  let val = 40 + (idx - 1) * 150;
  if (idx == currentSkin.value) {
    val += 20;
  } else if (idx == currentSkin.value + 1) {
    val += 40;
  } else if (idx == currentSkin.value + 2) {
    val += 40;
  } else if (idx == currentSkin.value - 1) {
  } else if (idx > currentSkin.value + 2) {
    val += 150;
  }
  return spacing.value + val + "px";
}
</script>

<template>
  <div>
    <Nav />
    <div class="main-con box-md">
      <div class="skin-con box-col">
        <img
          :src="skinData.bear[currentSkin.toString()].img"
          alt="곰1"
          class="main-skin"
        />
        <div class="skin-name box-md bitbit">
          {{ skinData.bear[currentSkin.toString()].name }}
        </div>
      </div>
      <div class="skin-list flex-align">
        <button class="skin-btn btn bitbit" @click="prevBtn"><</button>
        <div class="skins flex-align">
          <img
            :src="skin.img"
            :alt="skin.name"
            v-for="(skin, num) in skinData.bear"
            :key="num"
            class="skin"
            :class="skinScale(num)"
            :style="{ left: positionCalc(num) }"
          />
        </div>
        <button class="skin-btn btn bitbit" @click="nextBtn">></button>
      </div>
      <div class="btn-con">
        <button class="nft-btn btn bitbit">NFT 발행</button>
        <button class="select-btn btn bitbit">선택하기</button>
      </div>
    </div>
    <Footer />
  </div>
</template>

<style scoped>
.main-con {
  height: 850px;
  background-color: black;
  padding-top: 50px;
  flex-direction: column;
  justify-content: space-around;
}

.select-btn,
.nft-btn {
  position: absolute;
  bottom: -130px;
  transition: all 0.2s ease-in;

  padding: 10px 20px;
}
.btn-con {
  width: 300px;
  position: absolute;
  bottom: 10px;
  background-color: white;
}
.select-btn:hover,
.nft-btn:hover {
  box-shadow: 0px 0px 0px 5px #aba4f7;
}

.skin {
  transition: all 1s ease-in-out;
  width: 12%;
  position: absolute;
}

.skin-con {
  width: 600px;
  height: 450px;

  background-color: rgb(0, 0, 0);
  position: relative;
}

.main-skin {
  width: 100%;
  height: 100%;
  object-fit: cover; /* 이미지 비율을 유지하면서 부모 요소를 꽉 채우기 */
}

.skin-name {
  color: white;
  margin-top: 20px;

  font-size: 25px;
}

.skin-list {
  width: 60%;
  margin-bottom: 50px;
}

.skins {
  width: 100%;
  height: 320px;
  /* background-color: brown; */
  justify-content: space-between;
  position: relative;
  overflow: hidden;
}

.skin-center {
  scale: 1.4;
}

.skin-left,
.skin-right {
  scale: 1.2;
}
.btn {
  background-color: #1f1a59;
  color: white;
  font-size: 25px;
  text-align: center;
  border-radius: 10px;
  border-width: 5px;
  border-color: #1c165c;
}
.skin-btn {
  width: 50px;
  height: 50px;
}

.select-btn,
.nft-btn {
  border-width: 1px;
}

.select-btn {
  right: 0px;
}
</style>
