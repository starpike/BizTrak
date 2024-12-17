<template>
  <div class="home">
    <section class="hero is-medium is-dark mb-6">
      <div class="hero-body has-text-centered">
        <p class="title mb-6">
          Welcome to BizTrak
        </p>
        <p class="subtitle">
          Track your business performance
        </p>
      </div>
    </section>
  </div>
</template>

<script>
import axios from 'axios'
import Breadcrumb from '@/components/Breadcrumb.vue';

export default {
  name: 'Home',
  data() {
    return {
      breadcrumbs: [
      { text: 'Home', link: '/' },
      ]
    }
  },
  mounted() {
    document.title = 'Home | BizTrak'
    this.$emit('update-breadcrumbs', this.breadcrumbs)
  },
  methods: {
   
  }
}
</script>