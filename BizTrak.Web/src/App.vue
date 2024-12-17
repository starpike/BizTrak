<template>
  <div id="wrapper">
    <nav class="navbar brand-bar">
      <div class="navbar-brand">
        <router-link to="/" class="navbar-item is-size-4"
          @click="closeMobileMenu"><strong>BizTrak</strong></router-link>

        <a class="navbar-burger has-text-white" role="button" aria-label="menu" aria-expanded="false"
          data-target="navbar-menu" @click="showMobileMenu = !showMobileMenu">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>

      <div class="navbar-menu" id="navbar-menu" v-bind:class="{ 'is-active': showMobileMenu }">
        <div class="navbar-end">
          <router-link to="/quotes/" class="navbar-item" @click="closeMobileMenu">
            <FontAwesomeIcon icon="file-lines" />Quotes
          </router-link>
          <router-link to="/invoices/" class="navbar-item" @click="closeMobileMenu">
            <FontAwesomeIcon icon="dollar-sign" />
            Invoices
          </router-link>
          <router-link to="/customers/" class="navbar-item" @click="closeMobileMenu">
            <FontAwesomeIcon icon="person" />Customers
          </router-link>
          <router-link to="/schedule/" class="navbar-item" @click="closeMobileMenu">
            <FontAwesomeIcon icon="calendar-days" />Schedule
          </router-link>
          <div class="navbar-item">
            <div class="buttons">
              <div v-if="isLoggedIn">
                <router-link to="/logout/" class="button is-light">Log out</router-link>
              </div>
              <div v-else>
                <router-link to="/login/" class="button is-light">Log in</router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
    <div class="mb-5">
      <!-- <Breadcrumb :breadcrumbs="breadcrumbs" /> -->
    </div>
    <section class="section section-center pt-1">
      <router-view @update-breadcrumbs="updateBreadcrumbs" />
    </section>

    <footer class="footer">
    </footer>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useStore } from 'vuex';
import authenticationService from './services/authenticationService';
import Breadcrumb from '@/components/Breadcrumb.vue';
import FontAwesomeIcon from './utils/fontawesome';

const components = {
  Breadcrumb,
  FontAwesomeIcon
}

const store = useStore();
const showMobileMenu = ref(false);
const breadcrumbs = ref([]);
const isLoggedIn = computed(() => { return store.state.token && !authenticationService.isTokenExpired(store.state.token) });

const updateBreadcrumbs = (newBreadcrumbs) => {
  breadcrumbs.value = newBreadcrumbs;
};

const closeMobileMenu = () => {
  showMobileMenu.value = false;
}

</script>

<style lang="scss">
@import '../node_modules/bulma';
</style>
