<template>
  <div class="ptb-8 right-flex">
    <button class="btn" @click="setPage(page - 1)" :disabled="page === 1">
      Previous
    </button>
    <span class="p-2">{{ page }} / {{ totalPages }}</span>
    <button class="btn" @click="setPage(page + 1)" :disabled="page === totalPages">
      Next
    </button>
  </div>
</template>

<script>
import { defineComponent, computed } from 'vue';

export default defineComponent({
  props: {
    page: {
      type: Number,
      required: true
    },
    setPage: {
      type: Function,
      required: true
    },
    totalCount: {
      type: Number,
      required: true
    },
    pageSize: {
      type: Number,
      required: true
    }
  },
  setup(props) {
    const totalPages = computed(() => Math.ceil(props.totalCount / props.pageSize));

    return {
      totalPages
    };
  }
});
</script>

<style scoped>
.right-flex {
  display: flex;
  justify-content: flex-end;
  align-items: center;
}

.btn {
  cursor: pointer;
}

.btn:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}
</style>