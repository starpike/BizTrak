<template>
    <!-- Display a loading message while data is being fetched -->
    <div v-if="isDataLoaded" class="inline-container">
        <section class="hero is-small is-header">
            <div class="hero-body">
                <div class="columns is-mobile">
                    <div class="column is-size-6 has-text-weight-bold">Quote Details</div>
                    <div class="column has-text-right">
                        <span class="is-size-6">Status:
                            <StateTag :state="quote?.state"></StateTag>
                        </span>
                    </div>
                </div>
            </div>
        </section>
        <div class="box custom-box is-radiusless">
            <div class="field is-horizontal px-3 pt-3">
                <p><strong>Ref:</strong><span class="pl-1">{{ quote.quote_ref ? quote.quote_ref : 'N/A' }}</span>
                </p>
            </div>
            <BorderWrapper></BorderWrapper>
            <div class="field px-3">
                <label class="label">Title:</label>
                <div class="control">
                    <input type="text" id="title-input" class="input" :class="{ 'is-danger': v.title.$error }"
                        v-model="quote.title">
                    <p v-if="v.title.$errors.length" class="help is-danger">
                        <span v-for="error in v.title.$errors" :key="error.$uid">
                            {{ error.$message }}
                        </span>
                    </p>
                </div>
            </div>
            <BorderWrapper></BorderWrapper>
            <div class="field px-3">
                <label class="label">Customer:</label>
                <div class="control">
                    <div class="select is-narrow" :class="{ 'is-danger': v.customer_details.$error }">
                        <select v-model="quote.customer_details" id="customer-options">
                            <option disabled :value="null">select a customer...</option>
                            <option v-for="customer in customers" :key="customer.id"
                                :value="{ id: customer.id, name: customer.name }">
                                {{ customer.name }}
                            </option>
                        </select>
                    </div>
                    <p v-if="v.customer_details.$errors.length" class="help is-danger">
                        <span v-for="error in v.customer_details.$errors" :key="error.$uid">
                            {{ error.$message }}
                        </span>
                    </p>
                </div>
            </div>
            <BorderWrapper></BorderWrapper>
            <div class="field is-horizontal px-3">
                <p><strong>Total:</strong><span class="pl-1">£{{ totalCost }}</span>
                </p>
            </div>
            <BorderWrapper></BorderWrapper>
            <EditTasks :tasks="quote.tasks" :v="v.tasks" @add-task="addTask" @undo-task="undoAddTask" />
            <BorderWrapper></BorderWrapper>
            <EditMaterials :materials="quote.materials" :v="v.materials" @add-material="addMaterial"
                @undo-material="undoAddMaterial" />
            <BorderWrapper></BorderWrapper>
            <div class="is-flex is-justify-content-flex-end is-align-items-flex-start px-4 pb-5">
                <div class="buttons">
                    <button class="button" @click="submitForm">Save</button>
                    <router-link :to="cancelLink" class="button">Cancel</router-link>
                </div>
            </div>
        </div>
    </div>
    <div v-else>
        <p>Loading...</p>
    </div>
</template>

<script>
import { ref, reactive, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useStore } from 'vuex';
import apiClient from '@/services/apiClient';
import useVuelidate from '@vuelidate/core';
import EditTasks from '@/components/EditTasks.vue';
import EditMaterials from '@/components/EditMaterials.vue';
import BorderWrapper from '@/components/BorderWrapper.vue';
import StateTag from '@/components/StateTag.vue';
import { useQuoteValidation } from '@/composables/useQuoteValidation';
import { useCalculateTotal } from '@/composables/useCalculateTotal';
import { createQuote, createPostData, updateQuote } from '@/utils/quoteFactory';

export default {
    name: 'QuoteCreateUpdate',
    components: {
        EditTasks,
        EditMaterials,
        BorderWrapper,
        StateTag
    },
    setup() {
        const store = useStore();
        const customers = ref([]);
        const quote = reactive(createQuote());
        const isEditMode = ref(false);
        const route = useRoute();
        const router = useRouter();
        const isSubmitting = ref(false);
        const rules = computed(() => useQuoteValidation(isSubmitting));
        const v = useVuelidate(rules, quote);
        const isDataLoaded = computed(() => customers.value.length > 0 && quote && !store.state.isLoading);
        const cancelLink = computed(() => (isEditMode.value ? `/quotes/${quote.id}/` : '/quotes/'));
        const totalCost = computed(() => useCalculateTotal(quote.tasks, quote.materials));

        const fetchCustomers = async () => {
            try {
                const response = await apiClient.get(`/customers/`);
                customers.value = response.data.customers;

            } catch (error) {
                console.error('Error fetching the customers:', error);
            }
        };

        const fetchQuote = async (id) => {
            try {
                const response = await apiClient.get(`/quotes/${id}/?include_tasks=true&include_materials=true`);
                updateQuote(quote, response.data);
            } catch (error) {
                console.error('Error fetching the quote:', error);
            }
        };

        const submitForm = async () => {
            isSubmitting.value = true;
            v.value.$touch();
            if (v.value.$invalid) return console.error('Please correct validation errors before submitting.');

            try {
                const response = isEditMode.value
                    ? await apiClient.put(`/quotes/${quote.id}/`, createPostData(quote))
                    : await apiClient.post('/quotes/', createPostData(quote));
                router.push(`/quotes/${response.data?.id ?? quote.id}`);
            } catch (error) {
                console.error("Error saving the quote:", error);
            } finally {
                isSubmitting.value = false;
            }
        };

        const addTask = () => quote.tasks.push({ description: "", estimated_duration: 1, cost: 0.00, isToBeDeleted: false, orderIndex: quote.tasks.length });
        const undoAddTask = (index) => quote.tasks.splice(index, 1);
        const addMaterial = () => quote.materials.push({ material_name: "", quantity: 1, unit_price: 0.00, isToBeDeleted: false });
        const undoAddMaterial = (index) => quote.materials.splice(index, 1);

        onMounted(() => {
            store.commit('setIsLoading', true)
            document.title = 'Quote Edit | BizTrak';
            const quoteId = route.params.id;

            if (quoteId) {
                isEditMode.value = true;
                fetchQuote(quoteId);
            }
            fetchCustomers();

            store.commit('setIsLoading', false)
        });

        return {
            customers,
            quote,
            isEditMode,
            totalCost,
            cancelLink,
            addTask,
            undoAddTask,
            addMaterial,
            undoAddMaterial,
            submitForm,
            isDataLoaded,
            v
        };
    },
};
</script>
