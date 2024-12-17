<template>
    <div class="login-form">
        <form @submit.prevent="handleLogin">
            <div class="panel is-radiusless inline-container">
                <p class="panel-heading is-size-6 is-header">Login</p>
                <div class="panel-block">
                    <p><strong>Email:&nbsp;</strong></p>
                    <div class="control">
                        <input type="email" id="name-input" placeholder="" required class="input" v-model="email">
                    </div>
                </div>
                <div class="panel-block">
                    <p><strong>Password:&nbsp;</strong></p>
                    <input v-model="password" class="input" type="password" placeholder="Password" required />
                </div>
                <div class="panel-block is-justify-content-space-between">
                    <button type="submit" class="button">Login</button>
                    <router-link to="/register/" class="link">Register</router-link>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
import { ref } from 'vue';
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';

export default {
    setup() {
        const store = useStore();
        const router = useRouter();
        const email = ref('');
        const password = ref('');

        const handleLogin = async () => {
            try {
                await store.dispatch('login', { username: email.value, password: password.value });
                router.push('/quotes');
            } catch (error) {
                console.error('Login failed', error);
            }
        };



        return {
            email,
            password,
            handleLogin,
        };
    },
};
</script>