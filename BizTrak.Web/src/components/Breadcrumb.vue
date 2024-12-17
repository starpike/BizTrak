<template>
  <nav class="breadcrumb is-small has-succeeds-separator" aria-label="breadcrumbs">
    <ul>
      <li v-for="(breadcrumb, index) in breadcrumbs" :key="index" :class="{ 'is-active': breadcrumb.active }">
        <router-link v-if="!breadcrumb.active" :to="breadcrumb.link">{{ breadcrumb.text }}</router-link>
        <span class="ml-3" v-else><strong>{{ breadcrumb.text }}</strong></span>
      </li>
    </ul>
  </nav>
</template>

<script>
export default {
  props: {
    breadcrumbs: {
      type: Array,
      required: true,
    },
  },
};
</script>

<style scoped>
.breadcrumb {
  padding-top: 0.5rem;
  padding-left: 0.75rem;
  padding-bottom: 0.5rem;
}



a {
  color: black !important;
  font-weight: bold;
}
</style>