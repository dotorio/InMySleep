import { ref } from "vue";
import { defineStore } from "pinia";

export const useNftStore = defineStore(
  "nft",
  () => {
    const userNft = ref("");

    return {
      userNft,
    };
  },
  { persist: true }
);
