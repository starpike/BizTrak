<template>
    <div class="panel is-radiusless inline-container" v-if="quote && !$store.state.isLoading">
        <div class="panel-heading is-header">
            <div class="is-flex is-justify-content-space-between">
                <span class="is-size-6">Quote Details</span>
                <span class="is-size-6">Status:
                    <StateTag :state="quote?.state"></StateTag>
                </span>
            </div>
        </div>
        <div class="panel-block">
            <p><strong>Ref:</strong> {{ quote.quote_ref }}</p>
        </div>
        <div class="panel-block">
            <p><strong>Title:</strong> {{ quote.title }}</p>
        </div>
        <div class="panel-block">
            <p><strong>Customer:</strong> {{ quote.customer_details?.name }}</p>
        </div>
        <div class="panel-block">
            <p><strong>Quote Total:</strong> £{{ quote.total_amount }}</p>
        </div>
        <div class="panel-block">
            <div class="columns is-fullwidth control">
                <div class="column">
                    <p><strong>Tasks:</strong></p>
                    <div class="columns">
                        <div class="column">
                            <table class="table is-fullwidth is-striped is-hoverable">
                                <thead>
                                    <tr>
                                        <th>Description</th>
                                        <th>Est Duration</th>
                                        <th></th>
                                        <th class="has-text-right">Cost</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="task in quote.tasks" :key="task.id">
                                        <td>{{ task.description }}</td>
                                        <td class="has-text-centered">{{ task.estimated_duration }} hrs</td>
                                        <td></td>
                                        <td class="has-text-right">£{{ task.cost }}</td>
                                    </tr>
                                    <tr v-if="!quote.tasks || quote.tasks.length === 0">
                                        <td colspan="4">No tasks added</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-block">
            <div class="columns is-fullwidth control">
                <div class="column">
                    <p><strong>Materials:</strong></p>
                    <div class="columns">
                        <div class="column">
                            <table class="table is-fullwidth is-striped is-hoverable">
                                <thead>
                                    <tr>
                                        <th>Item Name</th>
                                        <th class="has-text-centered">Qty</th>
                                        <th class="has-text-right">Unit Price</th>
                                        <th class="has-text-right">Cost</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="material in quote.materials" :key="material.id">
                                        <td>{{ material.material_name }}</td>
                                        <td class="has-text-centered">{{ material.quantity }}</td>
                                        <td class="has-text-right">£{{ material.unit_price }}</td>
                                        <td class="has-text-right">£{{ material.total_price }}</td>
                                    </tr>
                                    <tr v-if="!quote.materials || quote.materials.length === 0">
                                        <td colspan="4">No materials added</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-block is-flex is-justify-content-flex-end">
            <div class="buttons">
                <router-link :to="{ name: 'quote-list' }" class="button">
                    < Back</router-link>
                        <router-link v-if="quote.state === QuoteState.DRAFT"
                            :to="{ name: 'quote-edit', params: { id: quote.id } }" class="button">Edit</router-link>
                        <button class="button" @click="downloadPdf">PDF</button>
                        <button class="button" v-if="quote.state === QuoteState.DRAFT" @click="sendQuote">Send</button>
                        <button class="button" v-if="quote.state === QuoteState.SENT"
                            @click="acceptQuote">Accept</button>
                        <button class="button" v-if="quote.state === QuoteState.SENT"
                            @click="rejectQuote">Reject</button>
                        <button class="button" v-if="quote.state === QuoteState.ACCEPTED" @click="bookJob">Book</button>
            </div>
        </div>
    </div>
    <!-- Display a loading message while data is being fetched -->
    <div v-else>
        <p>Loading...</p>
    </div>


</template>


<script>
import apiClient from '@/services/apiClient';
import { QuoteState } from '@/constants/quoteState';
import { createQuote } from '@/utils/quoteFactory';
import { useNotification } from '@/composables/useNotification';
import StateTag from '@/components/StateTag.vue';

export default {
    name: 'QuoteDetail',
    components: {
        StateTag
    },
    data() {
        return {
            quote: createQuote(),
            breadcrumbs: [
                { text: 'Home', link: '/' },
                { text: 'Quotes', link: '/quotes' },
                { text: 'Quote Details', link: this.$route.path, active: true }
            ],
            notification: useNotification(),
            QuoteState
        }
    },
    mounted() {
        this.getQuote(this.$route.params.id);
        document.title = 'Quote Detail | BizTrak';
        this.$emit("update-breadcrumbs", this.breadcrumbs);
    },
    methods: {
        async getQuote(id) {
            this.$store.commit('setIsLoading', true)
            try {
                const response = await apiClient.get(`/quotes/${id}/?include_tasks=true&include_materials=true`);
                this.quote = createQuote(response.data);
            } catch (error) {
                console.error('Error fetching the quote:', error);
            }
            this.$store.commit('setIsLoading', false)
        },
        async sendQuote() {
            const originalState = this.quote.state;
            this.quote.state = QuoteState.SENT;

            try {
                await apiClient.post(`/quotes/${this.quote.id}/send`);
                this.notification.showSuccess();
            } catch (error) {
                console.error("Error sending the quote:", error);
                this.quote.state = originalState;
                this.notification.showError();
            }
        },
        async acceptQuote() {
            const originalState = this.quote.state;
            this.quote.state = QuoteState.ACCEPTED;

            try {
                await apiClient.post(`/quotes/${this.quote.id}/accept`);
                this.notification.showSuccess();
            } catch (error) {
                console.error("Error accepting the quote:", error);
                this.quote.state = originalState;
                this.notification.showError();
            }
        },
        async rejectQuote() {
            const originalState = this.quote.state;
            this.quote.state = QuoteState.REJECTED;

            try {
                await apiClient.post(`/quotes/${this.quote.id}/reject`);
                this.notification.showSuccess();
            } catch (error) {
                console.error("Error rejecting the quote:", error);
                this.quote.state = originalState;
                this.notification.showError();
            }
        },
        downloadPdf() {
            const quoteId = this.quote.id;
            const url = `${apiClient.defaults.baseURL}/quotes/${quoteId}/pdf/`;

            apiClient.get(url, {
                responseType: 'blob', // Important for handling binary data (PDF)
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`, // Add token here
                },
            }).then((response) => {
                const fileURL = window.URL.createObjectURL(new Blob([response.data], { type: 'application/pdf' }));
                const fileName = `INV-${quoteId}.pdf`; // Specify the filename

                // Create a temporary <a> tag to trigger the download
                const a = document.createElement('a');
                a.href = fileURL;
                a.download = fileName; // Set the filename for download
                document.body.appendChild(a);
                a.click(); // Trigger the download
                document.body.removeChild(a); // Clean up the DOM
                window.URL.revokeObjectURL(fileURL); // Clean up the Blob URL
            })
                .catch((error) => {
                    console.error('Error fetching the PDF:', error);
                    alert('Failed to generate PDF.');
                });
        },
    }
};
</script>