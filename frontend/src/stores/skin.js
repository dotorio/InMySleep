import { ref } from "vue";
import { defineStore } from "pinia";

export const useSkinStore = defineStore(
  "skin",
  () => {
    const userSkin = ref({
      nickname: "김종덕 만렙",
      choice: "bear",
      bearColor: "0",
      selectedBearColor: "0",
      rabbitColor: "0",
      selectedRabbitColor: "0",
    });
    const userBearSkin = ref([]);
    const userRabbitSkin = ref([]);

    return { userSkin, userBearSkin, userRabbitSkin };
  },
  { persist: true }
);
