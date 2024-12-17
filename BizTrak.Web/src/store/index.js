import { createStore } from 'vuex'
import apiClient from '@/services/apiClient';

export default createStore({
  state: {
    token: localStorage.getItem('token') || '',
    isLoading: false
  },
  getters: {
    isAuthenticated: state => !!state.token,
  },
  mutations: {
    setIsLoading(state, status) {
      state.isLoading = status
    },
    setToken(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
    },
    clearToken(state) {
      state.token = '';
      localStorage.removeItem('token');
    },
  },
  actions: {
    async login({ commit }, user) {
      try {
        const response = await apiClient.post('/authentication/login', user);
        const token = response.data.token;
        commit('setToken', token);
      } catch (error) {
        console.error('Error during login:', error);
        throw error;
      }
    },
    logout({ commit }) {
      commit('clearToken');
    },
  },
  modules: {
  }
})
