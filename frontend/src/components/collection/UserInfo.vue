<script setup>
import { useUserStore } from "@/stores/user";
import { putEquipSkin } from "@/api/skin";
const { VITE_VUE_IPFS_URL } = import.meta.env;

const uStore = useUserStore();

function getNftSrc(character, color) {
  // return new URL(`/src/assets/collection/nft/${nft}.svg`, import.meta.url).href;
  let hash = "";
  if (uStore.userBearSkin.length === 0 || uStore.userRabbitSkin === 0) {
    return "";
  }
  console.log(character, color);
  if (character === "bear") {
    hash = uStore.userBearSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
    console.log(hash);
  } else if (character === "rabbit") {
    hash = uStore.userRabbitSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
    console.log(hash)
  }
  return new URL(`${VITE_VUE_IPFS_URL}${hash}`, import.meta.url).href;
}

async function equipSkin() {
  let choice = "";
  let selectedColor = "";
  if (uStore.userInfo.choice === "bear") {
    choice = "bear";
    selectedColor = uStore.userInfo.selectedBearColor;
  } else if (uStore.userInfo.choice === "rabbit") {
    choice = "rabbit";
    selectedColor = uStore.userInfo.selectedRabbitColor;
  }
  try {
    const response = await putEquipSkin(uStore.user.userId, choice, selectedColor);
    console.log(response);
    if (response.status === 200) {
      if (choice === "bear") {
        uStore.userInfo.bearColor = uStore.userInfo.selectedBearColor;
      } else if (choice === "rabbit") {
        uStore.userInfo.rabbitColor = uStore.userInfo.selectedRabbitColor;
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
</script>

<template>
  <div class="user-con box-col">
    <div class="nickname bit-t">{{ uStore.userInfo.nickname }}</div>
    <img v-if="uStore.userInfo.choice === 'bear'" :src="getNftSrc('bear', uStore.userInfo.selectedBearColor)" alt="곰" />
    <img v-else-if="uStore.userInfo.choice === 'rabbit'" :src="getNftSrc('rabbit', uStore.userInfo.selectedRabbitColor)"
      alt="토끼" />
      <div v-if="(uStore.userInfo.choice === 'bear' && uStore.userInfo.selectedBearColor !== uStore.userInfo.bearColor) || (uStore.userInfo.choice === 'rabbit' && uStore.userInfo.selectedRabbitColor !== uStore.userInfo.rabbitColor)" class="equip box-md" @click="equipSkin()">
        <img src="/src/assets/collection/equipped.svg" alt="equipped" />
      </div>
  </div>
</template>

<style scoped>
.user-con {
  width: 30%;
  height: 100%;

  border-radius: 20px;
  background-color: white;
}

.nickname {
  font-size: 30px;
  margin: 20px 30px;
}

img {
  height: 70%;
}

.equip>img {
  width: 80%;
  height: 100%;
  border: 3px solid rgb(22, 22, 115);
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.3s ease-in-out;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.24);
}

.equip>img:hover {
  scale: 1.1;
  opacity: 1;
}
</style>
