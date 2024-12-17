<template>
    <div class="panel is-radiusless inline-container">
        <p class="panel-heading is-size-6 is-header">My Quotes</p>
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
                        <th>Quote Ref</th>
                        <th>Title</th>
                        <th>Customer</th>
                        <th>Total</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="quote in quotes" :key="quote.id">
                        <td data-label="Quote Ref:">{{ quote.quote_ref }}</td>
                        <td data-label="Title:">{{ quote.title }}</td>
                        <td data-label="Customer:">{{ quote.customer_details.name }}</td>
                        <td data-label="Total">£{{ quote.total_amount }}</td>
                        <td data-label="Status:">
                            <StateTag :state="quote.state"></StateTag>
                        </td>
                        <td class="no-wrap list-controls">
                            <router-link :to="{ name: 'quote-detail', params: { id: quote.id } }"
                                class="button is-small mr-1">View</router-link>
                            <router-link v-if="quote.state === QuoteState.DRAFT" :to="{ name: 'quote-edit', params: { id: quote.id } }"
                                class="button is-small mr-1">Edit</router-link>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                            <router-link :to="{ name: 'quote-create' }" class="button mr-1">Create Quote</router-link>
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
import { createQuote } from '@/utils/quoteFactory';
import Pagination from '@/components/Pagination.vue'
import StateTag from '@/components/StateTag.vue';
import { QuoteState } from '@/constants/quoteState';

export default {
    components: {
        Pagination,
        StateTag
    },
    setup(_, { emit }) {
        const store = useStore();
        const quotes = ref([]);
        const currentPage = ref(1);
        const pageSize = ref(10);
        const totalCount = ref(0);
        const route = useRoute();
        const breadcrumbs = [
            { text: 'Home', link: '/' },
            { text: 'Quotes', link: route.path, active: true }
        ]

        const fetchQuotes = async () => {
            store.commit('setIsLoading', true)

            try {
                const response = await apiClient.get('/quotes/', {
                    params: {
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    },
                });
                quotes.value = response.data.quotes.map(quote => createQuote(quote));;
                totalCount.value = response.data.total;
            } catch (error) {
                console.error('Failed to fetch quotes:', error);
            }

            store.commit('setIsLoading', false)

        };

        const setPage = (newPage) => {
            currentPage.value = newPage;
            fetchQuotes(); // Fetch quotes for the new page
        };

        onMounted(() => {
            fetchQuotes(); // Fetch initial data
            document.title = 'Quotes | BizTrak'
            emit("update-breadcrumbs", breadcrumbs);
        });

        return {
            quotes,
            currentPage,
            pageSize,
            totalCount,
            setPage,
            breadcrumbs,
            QuoteState
        };
    }
}

</script>