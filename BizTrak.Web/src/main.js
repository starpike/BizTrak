import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './assets/global.css';
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

const app = createApp(App);
const options = {
};

app.use(Toast, options);
// Register store and router
app.use(store).use(router).mount('#app');