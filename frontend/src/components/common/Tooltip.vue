<script setup>
defineProps({
    message: String
})

</script>

<template>
    <div class="tooltip-message">
        {{ message }}
    </div>
</template>

<style scoped>
/* .tootip {
    position: relative;
    display: inline-block;
} */

.tooltip-message {
    visibility: hidden;
    width: 200px;
    background-color: #333;
    color: #fff;
    text-align: center;
    padding: 10px;
    border-radius: 5px;

    position: absolute;
    /* bottom: 150%; */
    left: 25%;
    transform: translateX(-50%);

    opacity: 0;
    transition: opacity 0.3s;

    z-index: 1;
}

.tooltip-message::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -5px;
    border-width: 5px;
    border-style: solid;
    border-color: #333 transparent transparent transparent;
}

.tooltip:hover .tooltip-message {
    visibility: visible;
    opacity: 1;
}
</style>