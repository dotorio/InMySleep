<script setup>
import MainHeader from "@/components/home/MainHeader.vue";
import MainLink from "@/components/home/MainLink.vue";
import { ref, onMounted, onUnmounted } from "vue";

const linkInfo = ref([
  {
    id: 1,
    header: "공지",
    content: "[공지] 이건 공지입니다.",
    linkName: "noti",
  },
  {
    id: 2,
    header: "NFT 전시관",
    content: "[전시관] 이건 전시관입니다.",
    linkName: "skin",
  },
  {
    id: 3,
    header: "가이드",
    content: "[가이드] 이건 가이드입니다.",
    linkName: "guide",
  },
]);

const downloadProgress = ref(0); // 진행률 상태
const isDownloading = ref(false); // 다운로드 중 상태
const dotCount = ref(1); // 점 개수

// 점 개수를 업데이트하는 함수
const updateDots = () => {
  dotCount.value = dotCount.value === 3 ? 1 : dotCount.value + 1;
};

// 다운로드 함수
const downloadFile = async () => {
  if (isDownloading.value) return;

  isDownloading.value = true; // 다운로드 시작
  const url = "https://j11e107.p.ssafy.io/download/InMySleep.exe";
  const response = await fetch(url);

  if (!response.ok) {
    console.error("Download failed");
    isDownloading.value = false;
    return;
  }

  const contentLength = response.headers.get("content-length");
  if (!contentLength) {
    console.error("Cannot determine file size");
    isDownloading.value = false;
    return;
  }

  const total = parseInt(contentLength, 10);
  let loaded = 0;

  const reader = response.body.getReader();
  const stream = new ReadableStream({
    async start(controller) {
      while (true) {
        const { done, value } = await reader.read();
        if (done) {
          controller.close();
          break;
        }
        loaded += value.length;
        downloadProgress.value = (loaded / total) * 100;
        controller.enqueue(value);
      }
    },
  });

  const blob = await new Response(stream).blob();

  const link = document.createElement("a");
  link.href = window.URL.createObjectURL(blob);
  link.download = "InMySleep.exe";
  link.click();

  window.URL.revokeObjectURL(link.href);
  isDownloading.value = false; // 다운로드 완료
};

// 컴포넌트가 마운트될 때 점 애니메이션 시작
let intervalId;
onMounted(() => {
  intervalId = setInterval(updateDots, 500); // 500ms마다 점 개수 업데이트
});

// 컴포넌트가 언마운트될 때 인터벌 제거
onUnmounted(() => {
  clearInterval(intervalId);
});
</script>

<template>
  <div class="main-con">
    <MainHeader />
    <div class="download-button-container">
      <button class="bit-t btn-download" @click="downloadFile" :disabled="isDownloading">
        <!-- 점 개수에 따라 다운로드 중 텍스트를 표시 -->
        {{
          isDownloading ? `다운로드 중${".".repeat(dotCount)}` : "게임 다운로드"
        }}
        <div v-if="downloadProgress > 0 && isDownloading">
          <p style="font-size: medium">{{ downloadProgress.toFixed(2) }}%</p>
          <progress :value="downloadProgress" max="100"></progress>
        </div>
      </button>
    </div>
    <div class="links flex-align">
      <MainLink v-for="(link, index) in linkInfo" :key="index" :info="link" />
    </div>
  </div>
</template>

<style scoped>
.main-con {
  position: relative;
}

.links {
  width: 100%;
  position: absolute;
  bottom: 50px;
  justify-content: space-around;
}

.btn-download {
  height: 100%;
  width: 300px;
  /* 고정된 버튼의 가로 크기 */
  padding: 10px 20px;
  margin-top: 20px;
  border-radius: 10px;
  border-width: 5px;
  font-size: 30px;
  border-color: #1b4f72;
  background-color: #2980b9;
  color: white;
  cursor: pointer;
  text-align: center;
}

.btn-download:disabled {
  background-color: #a0a0a0;
  cursor: not-allowed;
}

.download-button-container {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -70%);
}

progress {
  width: 100%;
  height: 40px;
  margin-top: 10px;
}
</style>
