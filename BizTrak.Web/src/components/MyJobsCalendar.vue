<template>
    <div class="calendar-container">
        <FullCalendar ref="calendarRef" :options="calendarOptions" />
        <div class="level level-right">
            <label>
                <input type="checkbox" v-model="showWeekends" /><span class="is-size-7">show weekends</span>
            </label>
        </div>
    </div>
</template>
<script setup>

import { ref, computed } from 'vue';
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin, { Draggable } from '@fullcalendar/interaction';

const emit = defineEmits(["calendar-event-received", "calendar-event-changed", "event-left-click", "calendar-dates-set"]);

const props = defineProps({
    scheduledJobs: {
        type: Array,
        default: () => []
    },
});

const showWeekends = ref(false);
const calendarRef = ref(null);

const handleDatesSet = (info) => {
    emit("calendar-dates-set", info.start, info.end);
}

const calendarOptions = computed(() => ({
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
    initialView: 'timeGridWeek',
    slotMinTime: '07:00:00',
    slotMaxTime: '20:00:00',
    allDaySlot: false,
    headerToolbar: {
        left: '',
        center: 'title',
        right: 'prev,next today dayGridMonth,timeGridWeek,timeGridDay'
    },
    views: {
        dayGridMonth: {
            titleFormat: { month: 'short', year: 'numeric' }
        },
        timeGridDay: {
            titleFormat: { day: '2-digit', month: 'short', year: '2-digit' }
        },
        timeGridWeek: {
            titleFormat: { year: '2-digit', month: '2-digit', day: '2-digit' },
            dayHeaderFormat: { weekday: 'short', day: '2-digit' },
        },
    },
    dayCellDidMount: function (info) {
        // Find the day number element
        const dayNumberEl = info.el.querySelector('.fc-daygrid-day-top');
        const calendarApi = info.view.calendar;

        if (dayNumberEl) {
            dayNumberEl.style.cursor = 'pointer'; // Indicate it's clickable
            dayNumberEl.addEventListener('click', (e) => {
                calendarApi.changeView('timeGridDay', info.date); // Navigate to day view
            });
        }
    },
    editable: true,
    droppable: true,
    eventBackgroundColor: '#005cb3',
    eventBorderColor: '#005cb3',
    eventTextColor: '#fff',
    height: 'auto',
    dayHeaderClassNames: 'day-header',
    dayCellClassNames: 'day-cell',
    weekends: showWeekends.value,
    datesSet: handleDatesSet,
    events: props.scheduledJobs,
    eventDidMount: function (info) {
        info.el.addEventListener('contextmenu', function (event) {
            event.preventDefault();
            emit('event-right-click', info.event.id);
        });
    },
    eventReceive: function (info) {
        let eventStart = info.event.start;
        let eventEnd = info.event.end;

        if (info.view.type == 'dayGridMonth') {
            eventStart.setHours(7, 0, 0, 0);
            eventStart.setHours(8, 0, 0, 0);
        }

        emit("calendar-event-received", info.event.id, eventStart, eventEnd)
    },
    eventChange: function (info) {
        emit("calendar-event-changed", info.event);
    },
    eventClick(info) {
        emit('event-left-click', info.event.id);
    },
    locale: 'en-GB',
    height: 'auto',
}));

</script>
<style></style>