import { ref } from "vue";
import { defineStore } from "pinia";

export const useSkinStore = defineStore(
  "skin",
  () => {
    const userSkin = ref({
      choice: "bear",
      bearMetadata: "0",
      selectedBearMetadata: "0",
      rabbitMetadata: "0",
      selectedRabbitMetadata: "0",
    });
    const userBearSkin = ref([]);
    const userRabbitSkin = ref([]);

    return { userSkin, userBearSkin, userRabbitSkin };
  },
  { persist: true }
);
