<template>
    <div class="panel is-radiusless inline-container">
        <p class="panel-heading is-header is-size-6">My Customers</p>
        <div class="panel-block">
            <p class="control has-icons-left">
                <input class="input" type="text" placeholder="Search" />
                <span class="icon is-left">
                    <i class="fas fa-search" aria-hidden="true"></i>
                </span>
            </p>
        </div>
        <div class="panel-block">
            <table class="table responsive-table is-fullwidth is-striped is-hoverable">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Address</th>
                        <th>Email</th>
                        <th>Phone</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="customer in customers" :key="customer.id">
                        <td data-label="Name:">{{ customer.name }}</td>
                        <td data-label="Address:">{{ customer.address }}</td>
                        <td data-label="Email:">{{ customer.email }}</td>
                        <td data-label="Tel:">{{ customer.phone }}</td>
                        <td class="no-wrap has-text-right">
                            <router-link :to="{ name: 'customer-edit', params: { id: customer.id } }"
                                class="button is-small mr-1">Edit</router-link>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                            <router-link :to="{ name: 'customer-create' }" class="button mr-1">Add Customer</router-link>
                        </td>
                        <td colspan="5">
                            <Pagination :page="currentPage" :setPage="setPage" :totalCount="totalCount"
                                :pageSize="pageSize" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</template>


<script>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { useStore } from 'vuex';
import apiClient from '@/services/apiClient';
import Pagination from '@/components/Pagination.vue'

export default {
    components: {
        Pagination,
    },
    setup(_, { emit }) {
        const store = useStore();
        const customers = ref([]);
        const currentPage = ref(1);
        const pageSize = ref(10);
        const totalCount = ref(0);
        const route = useRoute();
        const breadcrumbs = [
            { text: 'Home', link: '/' },
            { text: 'Customers', link: route.path, active: true }
        ]

        const fetchCustomers = async () => {
            store.commit('setIsLoading', true)

            try {
                const response = await apiClient.get('/customers/', {
                    params: {
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    },
                });
                customers.value = response.data.customers;
                totalCount.value = response.data.total;
            } catch (error) {
                console.error('Failed to fetch customers:', error);
            }

            store.commit('setIsLoading', false)

        };

        const setPage = (newPage) => {
            currentPage.value = newPage;
            fetchCustomers(); 
        };

        onMounted(() => {
            fetchCustomers(); // Fetch initial data
            document.title = 'Customers | BizTrak'
            emit("update-breadcrumbs", breadcrumbs);
        });

        return {
            customers,
            currentPage,
            pageSize,
            totalCount,
            setPage,
            breadcrumbs
        };
    }
}




</script>