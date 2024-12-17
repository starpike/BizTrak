<template>
    <div class="panel is-radiusless inline-container unscheduled-jobs-panel">
        <p class="panel-heading is-size-6 is-header no-wrap">Unscheduled Jobs</p>
        <div class="panel-block panel-content">
            <div id="external-events" class="external-events">
                <div v-if="jobs && jobs.length" v-for="job in jobs" :key="job.id"
                    class="fc-event" :data-title="job.title" :data-id="job.id"
                    :data-customerName="job.customerName">
                    <div class="fc-event-title">
                        <div>{{ job.title }}</div>
                        <div class="add-calendar-button">
                            <button class="icon-button" title="add to today" aria-label="add to today button"
                                @click.stop="addToCalendar(job)">
                                <FontAwesomeIcon icon="circle-plus" />
                            </button>
                        </div>
                    </div>
                    <span class="fc-event-customer">{{ job.customerName }}</span>
                </div>
                <div v-else>
                    <p class="is-size-7">No unscheduled jobs</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue';
import FontAwesomeIcon from '@/utils/fontawesome';
import { Draggable } from '@fullcalendar/interaction';

const emit = defineEmits(["addToCalendar"]);

const props = defineProps({
    jobs: {
        type: Array,
        required: true,
    },
});

const addToCalendar = (job) => {
    emit("addToCalendar", job);
};

onMounted(() => {

    const draggable = new Draggable(document.getElementById('external-events'), {
        itemSelector: '.fc-event',
        eventData(eventEl) {
            return {
                title: `${eventEl.getAttribute('data-title')} (${eventEl.getAttribute('data-customer')})`,
                id: eventEl.getAttribute('data-id')
            };
        }
    });
});

onUnmounted(() => {
});

</script>

<style scoped>
.unscheduled-jobs-panel {
    margin-right: 15px;
}

.fc-event {
    margin: 5px 0;
    padding: 8px;
    background: var(--dark-blue-l3);
    color: #fff;
    cursor: pointer;
    border-radius: 5px;
    transition: all 0.3s ease;
    line-height: normal;
}

.fc-event:hover {
    transform: scale(1.03);
    background: var(--dark-blue-l5);
}

.external-events {
    display: flex;
    flex-direction: column;
    width: 100%;
}

.fc-event-title {
    font-size: 0.7em;
    font-weight: bold;
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
}

.fc-event-customer {
    font-size: 0.7rem;
    color: #fff;
}

.icon-button {
    background: none;
    border: none;
    cursor: pointer;
    transition: all 0.3s ease;
    margin-left: 5px;
}

.icon-button:hover {
    transform: scale(1.2);
}

@media (max-width: 768px) {
    .unscheduled-jobs-panel {
        order: 1;
        margin-right: 0px;
        width: 100%;
        margin-bottom: 20px;
    }

    .fc-event:not(:last-child) {
        margin-right: 5px;
    }

    .external-events {
        flex-direction: row;
        flex-wrap: wrap;
    }
}
</style>