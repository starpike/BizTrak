<template>
    <span :class="getStateClass()" class="is-small has-text-weight-normal">
        {{ getStateName() }}
    </span>
</template>

<script>
import { defineComponent } from 'vue';
import { QuoteState } from '@/constants/quoteState';

export default defineComponent({
    name: "StateTag",
    props: {
        state: {
            type: Number,
            required: true,
        },
    },
    setup(props) {

        const getStateClass = () => {
            if (props.state === QuoteState.ACCEPTED) {
                return 'tag is-success';
            } else if (props.state === QuoteState.REJECTED) {
                return 'tag is-danger';
            } else if (props.state === QuoteState.SENT) {
                return 'tag is-info';
            } else {
                return 'tag is-light has-text-black';
            }
        }

        const getStateName = () => {
            if (props.state === QuoteState.ACCEPTED) {
                return 'Accepted';
            } else if (props.state === QuoteState.REJECTED) {
                return 'Rejected';
            } else if (props.state === QuoteState.SENT) {
                return 'Sent';
            } else {
                return 'Draft';
            }
        }

        return {
            getStateClass,
            getStateName
        };
    },
});
</script>
<style scoped></style>