<script setup>
import { ref } from "vue";
import CollectionNft from "@/components/collection/CollectionNft.vue";

const choice = ref("bear");
const bear = {
  nft: [1, 2, 3],
};
const rabbit = {
  nft: [4, 5, 6],
};

function choiceCharacter(character) {
  choice.value = character;
}
</script>

<template>
  <div class="character box-col">
    <div class="choice flex-align">
      <div class="bear box-md" @click="choiceCharacter('bear')">
        <img
          src="/src/assets/collection/bear.svg"
          alt="곰"
          :class="choice === 'bear' ? 'active' : ''"
        />
      </div>
      <div class="rabbit box-md" @click="choiceCharacter('rabbit')">
        <img
          src="/src/assets/collection/rabbit.svg"
          alt="토끼"
          :class="choice === 'rabbit' ? 'active' : ''"
        />
      </div>
    </div>
    <CollectionNft :nft-data="choice === 'bear' ? bear : rabbit" />
  </div>
</template>

<style scoped>
.character {
  width: 70%;
  height: 100%;

  justify-content: space-around;
  align-items: center;
  /* background-color: blue; */
}
.choice {
  width: 100%;
  height: 10%;
  justify-content: space-around;
}
.bear img,
.rabbit img {
  width: 80%;
  opacity: 0.5;
  border: 3px solid rgb(22, 22, 115);
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.3s ease-in-out;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.24);
}
.bear img:hover,
.rabbit img:hover {
  scale: 1.1;
  opacity: 1;
}

.active {
  opacity: 1 !important;
}
</style>
