<template>
    <div :class="['side-panel', { 'is-active': isPanelActive }]" @click.stop>
        <div class="tab-handle" @click="closePanel">Job Details</div>
        <div class="">
            <div class="">
                <div>
                    <label class="label">Title:</label>
                    {{ selectedJob?.jobTitle }}
                </div>
                <div>
                    <label class="label">Customer:</label>
                    {{ selectedJob?.customerName }}
                </div>
                <div>
                    <label class="label">Start:</label>
                    {{ formatDate(selectedJob?.start) }}
                </div>
                <div>
                    <label class="label">End:</label>
                    {{ formatDate(selectedJob?.end) }}
                </div>
                <div>
                    <label class="label">Quote Ref:</label>
                    <router-link v-if="selectedJob?.quoteId"
                        :to="{ name: 'quote-detail', params: { id: selectedJob?.quoteId } }" class="" @click.stop>{{
                        selectedJob?.quoteRef }}</router-link>
                </div>
            </div>
        </div>
    </div>
    <div v-if="isPanelActive" class="overlay" @click="closePanel"></div>
</template>
<script setup>

import { ref, defineExpose, computed } from 'vue';
import { formatDate } from '@/utils/dateUtils';

const selectedJob = ref(null);
const isPanelActive = computed(() => selectedJob.value !== null);

const openPanel = (job) => {

    if (job) {
        selectedJob.value = job;
    }
};

const closePanel = () => {
    selectedJob.value = null
}

defineExpose({
  openPanel,
});

</script>
<style scoped>
.side-panel {
    position: fixed;
    padding: 15px;
    right: -400px;
    top: 100px;
    width: 250px;
    height: 100%;
    background-color: #fff;
    box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
    transition: right 0.3s ease;
    z-index: 20;
    border-top: 1px solid var(--dark-blue-l2);
    border-left: 1px solid var(--dark-blue-l2);
    border-bottom: 1px solid var(--dark-blue-l2);
}

.side-panel.is-active {
    right: 0;
    top: 100px;
}

.overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 9;
}

.tab-handle {
    position: absolute;
    top: -40px;
    left: -1px;
    width: 140px;
    height: 40px;
    background-color: var(--dark-blue-l2);
    color: #fff;
    text-align: center;
    align-content: center;
    font-weight: bold;
    border-radius: 5px 5px 0 0;
    box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
    cursor: pointer;
}

@media (max-width: 768px) {
.side-panel {
        width: 200px;
    }
}

@media (min-width: 1024px) {
    .side-panel {
        width: 300px;
    }
}
</style>