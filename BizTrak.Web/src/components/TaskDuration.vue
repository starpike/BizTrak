<template>
    <div class="time-adjuster">
        <button class="button" @click="decreaseTime" :disabled="timeInHours <= 0">-</button>
        <input type="text" class="input" :value="formattedTime" readonly :class="{ 'is-danger': v.length }" />
        <button class="button" @click="increaseTime">+</button>
    </div>
        <span class="help is-danger" v-for="error in v" :key="error">
            {{ error.$message }}
        </span>
</template>

<script>
export default {
    data() {
        return {
            timeInHours: this.initialTime,
        };
    },
    props: {
        initialTime: {
            type: Number,
            default: 0, // Default initial value if not provided
        },
        v: {
            type: Object,
            required: true
        }
    },
    computed: {
        formattedTime() {
            return `${this.timeInHours.toFixed(2)} hrs`;
        },
    },
    methods: {
        increaseTime() {
            this.timeInHours += 0.25; // Increase by 1/4 hour
            this.emitTimeChange();
        },
        decreaseTime() {
            if (this.timeInHours > 0) {
                this.timeInHours -= 0.25; // Decrease by 1/4 hour, but not below 0
                this.emitTimeChange();
            }
        },
        emitTimeChange() {
            this.$emit('update:duration', this.timeInHours);
        }
    },
};
</script>

<style scoped>
.time-adjuster {
    display: flex;
    align-items: center;
}

button {
    width: 30px;
}

input {
    text-align: center;
    width: 100px;
    margin: 0 3px;
}
</style>