import axios from "axios";
import router from "@/router";
import store from '@/store';

const apiClient = axios.create({
    baseURL: "https://localhost:7234/api",
    headers: {
        "Content-Type": "application/json",
    },
});

apiClient.interceptors.request.use(config => {
    const token = store.state.token;
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
});

apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            store.clearToken("token");
            router.push("/login");
        }
        return Promise.reject(error);
    }
);

export default apiClient;
