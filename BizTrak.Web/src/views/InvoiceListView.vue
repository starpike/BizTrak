<template>
    <div class="panels-wrapper">
        <UnscheduledJobsPanel :jobs="unscheduledJobs"></UnscheduledJobsPanel>
        <div class="panel is-radiusless inline-container my-jobs-panel">
            <p class="panel-heading is-size-6 is-header">My Jobs</p>
            <div class="panel-block panel-content">
                <MyJobsCalendar :scheduled-jobs="scheduledJobs" @calendar-event-received="jobReceived"
                    @calendar-event-changed="jobChanged" @event-click="jobSelected"
                    @calendar-dates-set="calendarDatesSet">
                </MyJobsCalendar>
            </div>
        </div>
        <JobDetailSidePanel ref="jobDetailPanelRef"></JobDetailSidePanel>
    </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue';
import FontAwesomeIcon from '@/utils/fontawesome';
import { useStore } from 'vuex';
import { debounce } from 'lodash';
import UnscheduledJobsPanel from '@/components/UnscheduledJobsPanel.vue';
import JobDetailSidePanel from '@/components/JobDetailSidePanel.vue';
import MyJobsCalendar from '@/components/MyJobsCalendar.vue';
import jobService from '@/services/jobService'
import '../assets/fullCalendar.css'

const calStart = ref();
const calEnd = ref();
const scheduledJobs = ref([]);
const unscheduledJobs = ref([]);

const jobDetailPanelRef = ref(null);
const store = useStore();

const components = {
    FontAwesomeIcon,
    UnscheduledJobsPanel,
    JobDetailSidePanel,
    MyJobsCalendar
}

watch([calStart, calEnd], () => {
    fetchScheduledJobs();
});

const jobReceived = (jobId, jobStart, jobEnd) => {
    const job = unscheduledJobs.value.find(j => j.id == jobId)

    if (job) {
        let start = jobStart;
        let end = jobEnd || new Date(start);

        if (start.getTime() === end.getTime()) {
            end.setHours(end.getHours() + 1);
        }

        updateJob(start, end, job);
    }
}

const jobChanged = (job) => {
    let start = job.start;
    let end = job.end || new Date(start);

    if (start.getTime() === end.getTime()) {
        end.setHours(end.getHours() + 1);
    }

    updateJob(start, end, job);
}

const jobSelected = (jobId) => {
    let job = scheduledJobs.value.find(j => j.id == jobId);
    jobDetailPanelRef.value.openPanel(job)
}

const calendarDatesSet = (startDate, endDate) => {
    calStart.value = startDate.toISOString();
    calEnd.value = endDate.toISOString();
}

const updateJob = debounce(async (startDate, endDate, job) => {
    await jobService.updateJob(startDate, endDate, job)
    tryRemoveUnscheduledJobById(job.id);
    scheduledJobs.value = await jobService.fetchScheduledJobs(calStart.value, calEnd.value);
}, 300)

const tryRemoveUnscheduledJobById = (id) => {
    const index = unscheduledJobs.value.findIndex(job => job.id == id);
    if (index !== -1) {
        unscheduledJobs.value.splice(index, 1);
    }
};

const fetchUnscheduledJobs = debounce(async () => {
    store.commit('setIsLoading', true)
    unscheduledJobs.value = await jobService.fetchUnscheduledJobs();
    store.commit('setIsLoading', false)
}, 300);

const fetchScheduledJobs = debounce(async () => {
    store.commit('setIsLoading', true)
    scheduledJobs.value = await jobService.fetchScheduledJobs(calStart.value, calEnd.value);
    store.commit('setIsLoading', false)
}, 300);

onMounted(() => {
    fetchUnscheduledJobs();
    fetchScheduledJobs();
    document.title = 'Scheduler | BizTrak';
});

onUnmounted(() => {

});

</script>
<style scoped>
.panels-wrapper {
    display: flex;
    align-items: stretch
}

@media (max-width: 768px) {
    .panels-wrapper {
        flex-direction: column;
    }

    .my-jobs-panel {
        order: 2;
    }
}
</style>