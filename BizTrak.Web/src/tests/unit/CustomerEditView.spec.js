import { mount } from '@vue/test-utils';
import CustomerEdit from '@/views/CustomerEditView.vue';
import apiClient from '@/services/apiClient';
import { useRoute, useRouter } from 'vue-router';
import { useStore } from 'vuex';
import flushPromises from 'flush-promises';
import useVuelidate from '@vuelidate/core';

// Mock API client
jest.mock('@/services/apiClient', () => ({
    get: jest.fn(),
    post: jest.fn(),
    put: jest.fn(),
}));

// Mock Vuex Store
jest.mock('vuex', () => ({
    useStore: jest.fn(() => ({
        commit: jest.fn(),
        state: {
            isLoading: false
        }
    })),
}));

const mockPush = jest.fn();

// Mock Vue Router
jest.mock('vue-router', () => ({
    createRouter: jest.fn(() => ({
        push: jest.fn(),
    })),
    createWebHistory: jest.fn(),
    useRoute: jest.fn(() => ({
        path: '/customers/',
        params: {
            id: 1
        }
    })),
    useRouter: jest.fn(() => ({
        push: mockPush,
    }))
}));

jest.mock('@/store', () => ({
    state: {
        token: '',
        isLoading: false,
    },
    commit: jest.fn(),
}));

jest.mock('@/router', () => ({
    default: {
        beforeEach: jest.fn(),
    },
}));

// Mock Data
const mockCustomer = { id: 1, name: 'John Doe', address: '123 Main St', email: 'john@example.com', phone: '1234567890' };

const mockApiResponse = {
    data: mockCustomer
};

describe('CustomerEditView', () => {

    test('renders the customer edit component and fetches data', async () => {

        apiClient.get.mockResolvedValue(mockApiResponse);

        const wrapper = mount(CustomerEdit, {
            global: {
                mocks: {
                    $store: useStore(),
                    $router: useRouter()                },
            },
        });

        // Wait for the component to fetch data
        await flushPromises();

        expect(wrapper.find('#name-input').element.value).toBe('John Doe');
        expect(wrapper.find('#address-input').element.value).toBe('123 Main St');
        expect(wrapper.find('#email-input').element.value).toBe('john@example.com');
        expect(wrapper.find('#phone-input').element.value).toBe('1234567890');
    });

    it('submits the form to create a new customer', async () => {

        useRoute.mockReturnValue({
            path: '/customers/',
            params: {}
          });
        
        apiClient.post.mockResolvedValueOnce(mockCustomer);

        const wrapper = mount(CustomerEdit, {
            global: {
                mocks: {
                    $store: useStore(),
                    $router: useRouter(),
                },
            },
        });

        wrapper.vm.customer = mockCustomer;

        // Trigger form submission
        await wrapper.vm.submitForm();

        // Assert that the post method was called with the correct payload
        expect(apiClient.post).toHaveBeenCalledWith('/customers/', mockCustomer);

        // Assert that the router was redirected to the customers list
        expect(mockPush).toHaveBeenCalledWith('/customers/');
    });
});



