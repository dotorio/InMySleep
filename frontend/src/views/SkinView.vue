<script setup>
import Nav from "@/components/Nav.vue";
import Footer from "@/components/Footer.vue";
import skinData from "@/assets/data/skin.json";
import { ref, onBeforeMount } from "vue";
import { myNFTs } from "@/api/nft";
import { putEquipSkin, getEquippedSkin, getSkinList } from "@/api/skin";
import { useSkinStore } from "@/stores/skin";
import { useUserStore } from "@/stores/user";

const sStore = useSkinStore();
const uStore = useUserStore();

const currentSkin = ref(3);
const spacing = ref(0);


const defaultUrl = "none";
const defaultName = "잠금"
const bear = ref({
  nft: Array.from({ length: 15 }, () => ({
    imageUrl: defaultUrl,
    name: defaultName,
  })),
});
const rabbit = ref({
  nft: Array.from({ length: 15 }, () => ({
    imageUrl: defaultUrl,
    name: defaultName,
  })),
});
const nftData = ref([
  {
    id: 0,
    description: "",
    attributes: {},
    imageUrl: "",
    metadataUri: "",
  }
]);

onBeforeMount(async () => {
  sStore.userSkin["choice"] = "bear";
  try {
    const response = await getEquippedSkin(uStore.user.data.userId);
    if (response.data[0].attributes.character === "bear") {
      sStore.userSkin.selectedBearMetadata = sStore.userSkin.bearMetadata = response.data[0].id;
      sStore.userSkin.selectedRabbitMetadata = sStore.userSkin.rabbitMetadata = response.data[1].id;
    } else {
      sStore.userSkin.selectedRabbitMetadata = sStore.userSkin.rabbitMetadata = response.data[0].id;
      sStore.userSkin.selectedBearMetadata = sStore.userSkin.bearMetadata = response.data[1].id;
    }
  } catch (error) {
    console.error(error);
  }

  try {
    const response = await getSkinList(uStore.user.data.userId);
    nftData.value = response.data;
    nftData.value = prepareNftData(response.data);
    filterNftData(nftData);
    sStore.userBearSkin = bear.value.nft;
    sStore.userRabbitSkin = rabbit.value.nft;
    sStore.userSkin.selectedBearMetadata = sStore.userBearSkin[currentSkin.value].id;
  } catch (error) {
    console.error(error);
  }

  if (!uStore.user.data.metamaskToken) {
    return;
  }

  try {
    const response = await myNFTs(uStore.user.data.address, uStore.user.data.metamaskToken);
    nStore.userNft = response.data;
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "NFT를 불러오는데 실패했습니다. 지갑 연동 상태를 확인해주세요.",
    });
    uStore.user.data.metamaskToken = "";
    uStore.user.data.address = "";
    console.error(error);
  }
})


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
  if (currentSkin.value === 15) {
    currentSkin.value = 14;

  } else {
    spacing.value -= 150;
  }
  sStore.userSkin.selectedBearMetadata = sStore.userBearSkin[currentSkin.value].id;
  sStore.userSkin.selectedRabbitMetadata = sStore.userRabbitSkin[currentSkin.value].id;
}

function prevBtn() {
  currentSkin.value -= 1;
  console.log(currentSkin.value);
  if (currentSkin.value === -1) {
    currentSkin.value = 0;
  } else {
    spacing.value += 150;
  }
  sStore.userSkin.selectedBearMetadata = sStore.userBearSkin[currentSkin.value].id;
  sStore.userSkin.selectedRabbitMetadata = sStore.userRabbitSkin[currentSkin.value].id;
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

function prepareNftData(responseData) {
  return responseData.map((nft) => {
    return {
      id: nft.id,
      description: nft.description,
      attributes: nft.attributes,
      imageUrl: nft.image_url,
      metadataUri: nft.metadata_uri,
      name: nft.name,
    };
  });
}

function filterNftData(nftData) {
  let bearIndex = 0
  let rabbitIndex = 0
  nftData.value.forEach((nft) => {
    if (nft.attributes.character.toLowerCase() === "bear") {
      bear.value.nft[bearIndex] = nft;
      bearIndex++;
    } else {
      rabbit.value.nft[rabbitIndex] = nft;
      rabbitIndex++;
    }
  });
}

function choiceCharacter(character) {
  sStore.userSkin.choice = character;
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

async function equipSkin() {
  let choice = "";
  let selectedMetadata = "";

  console.log("inner equipSkin")

  if (sStore.userSkin.choice === "bear") {
    choice = "bear";
    selectedMetadata = sStore.userSkin.selectedBearMetadata;
  } else if (sStore.userSkin.choice === "rabbit") {
    choice = "rabbit";
    selectedMetadata = sStore.userSkin.selectedRabbitMetadata;
  }
  try {
    const response = await putEquipSkin(uStore.user.data.userId, choice, selectedMetadata);
    if (response.status === 200) {
      if (choice === "bear") {
        sStore.userSkin.bearMetadata = sStore.userSkin.selectedBearMetadata;
      } else if (choice === "rabbit") {
        sStore.userSkin.rabbitMetadata = sStore.userSkin.selectedRabbitMetadata;
      }
      Swal.fire({
        icon: "success",
        title: "성공",
        text: "스킨이 변경되었습니다",
      });
    }
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "실패",
      text: "스킨 변경에 실패했습니다",
    })
    console.error(error);
  }
}

function equipSkinCheck() {
  if (sStore.userSkin.choice === "bear") {
    if (sStore.userBearSkin[currentSkin.value].name === "잠금") {
      return false;
    }
  } else if (sStore.userSkin.choice === "rabbit") {
    if (sStore.userRabbitSkin[currentSkin.value].id === "잠금") {
      return false;
    }
  }
  if (sStore.userSkin.choice === "bear") {
    return sStore.userSkin.selectedBearMetadata !== sStore.userSkin.bearMetadata;
  } else if (sStore.userSkin.choice === "rabbit") {
    return sStore.userSkin.selectedRabbitMetadata !== sStore.userSkin.rabbitMetadata;
  }
}
</script>

<template>
  <div>
    <Nav />
    <div class="main-con box-md">
      <button @click="choiceCharacter('bear')">곰</button>
      <button @click="choiceCharacter('rabbit')">토끼</button>
      <div class="skin-con box-col">
        <div v-if="sStore.userSkin.choice === 'bear'" class="skin-con">
          <img :src="imgUrl(sStore.userBearSkin[currentSkin])" alt="곰1" class="main-skin" />
        </div>
        <div v-else-if="sStore.userSkin.choice === 'rabbit'" class="skin-con">
          <img :src="imgUrl(sStore.userRabbitSkin[currentSkin])" alt="토끼1" class="main-skin" />
        </div>
        <div v-if="sStore.userSkin.choice === 'bear'" class="skin-name box-md bitbit">
          {{ sStore.userBearSkin[currentSkin].name }}
        </div>
        <div v-else-if="sStore.userSkin.choice === 'rabbit'" class="skin-name box-md bitbit">
          {{ sStore.userRabbitSkin[currentSkin].name }}
        </div>
      </div>
      <div class="skin-list flex-align">
        <button class="skin-btn btn bitbit" @click="prevBtn">&lt;
        </button>
        <div class="skins flex-align">
          <div v-if="sStore.userSkin.choice === 'bear'" class="skin-list">
            <img :src="imgUrl(skin)" :alt="skin.name" v-for="(skin, num) in sStore.userBearSkin" :key="num" class="skin"
              :class="skinScale(num)" :style="{ left: positionCalc(num) }" />
          </div>
          <div v-else-if="sStore.userSkin.choice === 'rabbit'" class="skin-list">
            <img :src="imgUrl(skin)" :alt="skin.name" v-for="(skin, num) in sStore.userRabbitSkin" :key="num"
              class="skin" :class="skinScale(num)" :style="{ left: positionCalc(num) }" />
          </div>
        </div>
        <button class="skin-btn btn bitbit" @click="nextBtn">></button>
      </div>
      <div class="btn-con">
        <button class="nft-btn btn bitbit">NFT 발행</button>
        <button v-if="equipSkinCheck()" class="select-btn btn bitbit" @click="equipSkin()">선택하기</button>
        <button v-else class="select-btn btn bitbit disable">선택하기</button>
      </div>
    </div>
    <Footer />
  </div>
</template>

<style scoped>
.main-con {
  height: 950px;
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
  bottom: -90px;
  background-color: white;
}

.select-btn:hover,
.nft-btn:hover {
  box-shadow: 0px 0px 0px 5px #aba4f7;
}

.skin {
  transition: all 0.5s ease-in-out;
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
  object-fit: cover;
  /* 이미지 비율을 유지하면서 부모 요소를 꽉 채우기 */
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

.disable {
  transform: scale(1) !important;
  cursor: not-allowed !important;
  filter: brightness(0.7);
  opacity: 0.7 !important;
}
</style>
