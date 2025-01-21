<template>
    <div class="panel is-radiusless inline-container" v-if="isDataLoaded">
        <p class="panel-heading is-header is-size-6">Customers Details</p>
        <div class="panel-block">
            <p><strong>Name:</strong></p>
            <div class="control">
                <input type="text" id="name-input" class="input" v-model="customer.name"
                    :class="{ 'is-danger': v.name.$error }">
            </div>
            <p v-if="v.name.$errors.length" class="help is-danger">
                <span v-for="error in v.name.$errors" :key="error.$uid">
                    <p>{{ error.$message }}</p>
                </span>
            </p>
        </div>
        <div class="panel-block">
            <p><strong>Address:</strong></p>
            <div class="control">
                <input type="text" id="address-input" class="input" v-model="customer.address"
                    :class="{ 'is-danger': v.address.$error }">
            </div>
            <p v-if="v.address.$errors.length" class="help is-danger">
                <span v-for="error in v.address.$errors" :key="error.$uid">
                    <p>{{ error.$message }}</p>
                </span>
            </p>
        </div>
        <div class="panel-block">
            <p><strong>Email:</strong></p>
            <div class="control">
                <input type="email" id="email-input" class="input" v-model="customer.email"
                    :class="{ 'is-danger': v.email.$error }">
            </div>
            <p v-if="v.email.$errors.length" class="help is-danger">
                <span v-for="error in v.email.$errors" :key="error.$uid">
                    <p>{{ error.$message }}</p>
                </span>
            </p>
        </div>
        <div class="panel-block">
            <p><strong>Phone:</strong></p>
            <div class="control">
                <input type="tel" id="phone-input" class="input" v-model="customer.phone"
                    :class="{ 'is-danger': v.phone.$error }">
            </div>
            <p v-if="v.phone.$errors.length" class="help is-danger">
                <span v-for="error in v.phone.$errors" :key="error.$uid">
                    <p>{{ error.$message }}</p>
                </span>
            </p>
        </div>

        <div class="panel-block is-flex is-justify-content-flex-end">
            <div class="buttons">
                <router-link :to="{ name: 'customer-list' }" class="button">
                    < Back</router-link>
                        <button class="button" id="submit-button" @click="submitForm">Save</button>
            </div>
        </div>
    </div>
    <!-- Display a loading message while data is being fetched -->
    <div v-else>
        <p>Loading...</p>
    </div>


</template>


<script>
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import useVuelidate from '@vuelidate/core';
import { useStore } from 'vuex';
import apiClient from '@/services/apiClient';
import useCustomerValidation from '@/composables/useCustomerValidation';

export default {
    name: 'CustomerEdit',
    setup() {
        const store = useStore();
        const customer = ref({});
        const route = useRoute();
        const router = useRouter();
        const isDataLoaded = computed(() => customer.value && !store.state.isLoading);
        const isEditMode = ref(false);
        const rules = computed(() => useCustomerValidation());
        const v = useVuelidate(rules, customer);

        onMounted(() => {
            store.commit('setIsLoading', true)
            document.title = 'Customer Edit | BizTrak';

            const customerId = route.params.id;

            if (customerId) {
                isEditMode.value = true;
                fetchCustomer(customerId);
            }

            store.commit('setIsLoading', false)
        });

        const fetchCustomer = async (id) => {
            try {
                const response = await apiClient.get(`/customers/${id}/`);
                customer.value = response.data;
            } catch (error) {
                console.error('Error fetching customer:', error);
            }
        };

        const submitForm = async () => {
            debugger
            try {
                v.value.$touch();
                if (v.value.$invalid) return console.error('Please correct validation errors before submitting.');

                const response = isEditMode.value
                    ? await apiClient.put(`/customers/${customer.value.id}/`, customer.value)
                    : await apiClient.post('/customers/', customer.value);
                router.push(`/customers/`);
            } catch (error) {
                console.error("Error saving the customer:", error);
            }
        }

        return {
            customer,
            isEditMode,
            isDataLoaded,
            submitForm,
            v
        };
    }
}
</script>

<style scoped>
.panel {
    width: 80%;
}

.panel-block:not(:has(.buttons)) {
    flex-direction: column;
    align-items: flex-start;
}
</style>