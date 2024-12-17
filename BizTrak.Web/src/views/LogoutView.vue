<template>
    <div class="login-form">
        <div class="panel is-radiusless inline-container">
            <p class="panel-heading is-size-6 is-header">You have logged out successfully</p>
            <div class="panel-block">
                <router-link to="/login/" class="button">Log in</router-link>
            </div>
        </div>
    </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useStore } from 'vuex';

const store = useStore();

onMounted(() => {
    store.commit('clearToken');
    document.title = 'Logout | BizTrak';
});

</script>