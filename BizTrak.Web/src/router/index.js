import { createRouter, createWebHistory } from 'vue-router'
import store from '@/store';
import HomeView from '../views/HomeView.vue'
import QuoteDetailView from '@/views/QuoteDetailView.vue'
import QuoteListView from '@/views/QuoteListView.vue'
import QuoteEditView from '@/views/QuoteEditView.vue'
import InvoiceListView from '@/views/InvoiceListView.vue'
import CustomerListView from '@/views/CustomerListView.vue'
import CustomerEditView from '@/views/CustomerEditView.vue'
import ScheduleView from '@/views/ScheduleView.vue'
import LoginView from '@/views/LoginView.vue';
import LogoutView from '@/views/LogoutView.vue';
import authenticationService from '@/services/authenticationService';

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/quotes/',
    name: 'quote-list',
    component: QuoteListView,
    meta: { requiresAuth: true }
  },
  {
    path: '/quotes/create',
    name: 'quote-create',
    component: QuoteEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/quotes/:id',
    name: 'quote-detail',
    component: QuoteDetailView,
    meta: { requiresAuth: true }
  },
  {
    path: '/quotes/:id/edit',
    name: 'quote-edit',
    component: QuoteEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/invoices/',
    name: 'invoice-list',
    component: InvoiceListView ,
    meta: { requiresAuth: true }
  },
  {
    path: '/customers/',
    name: 'customer-list',
    component: CustomerListView,
    meta: { requiresAuth: true }
  },
  {
    path: '/customers/create',
    name: 'customer-create',
    component: CustomerEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/customers/:id/edit',
    name: 'customer-edit',
    component: CustomerEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/schedule/',
    name: 'schedule',
    component: ScheduleView,
    meta: { requiresAuth: true }
  },
  {
    path: '/login/',
    name: 'login',
    component: LoginView,
  },
  {
    path: '/logout/',
    name: 'logout',
    component: LogoutView,
  },
  {
    path: '/register/',
    name: 'register',
    component: () => import('@/views/RegisterView.vue'),
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  const token = store.state.token;

  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!token) {
      next('/login');
    } else if (authenticationService.isTokenExpired(token)) {
      store.commit('clearToken');
      alert('"Your session has expired. Please log in again."')
      next('/login');
    } else {
      next(); // Allow access if authenticated
    }
  } else {
    next(); // Allow access for non-protected routes
  }
});

export default router
