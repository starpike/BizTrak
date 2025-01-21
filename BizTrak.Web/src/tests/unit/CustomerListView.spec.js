import { mount } from '@vue/test-utils';
import CustomerList from '@/views/CustomerListView.vue';
import Pagination from '@/components/Pagination.vue';
import apiClient from '@/services/apiClient';
import { useRoute } from 'vue-router';
import { useStore } from 'vuex';
import flushPromises from 'flush-promises';

// Mock API client
jest.mock('@/services/apiClient');

// Mock Vuex Store
jest.mock('vuex', () => ({
  useStore: jest.fn(() => ({
    commit: jest.fn(),
  })),
}));

// Mock Vue Router
jest.mock('vue-router', () => ({
  createRouter: jest.fn(() => ({
    push: jest.fn(),
  })),
  createWebHistory: jest.fn(),
  useRoute: jest.fn(() => ({
    path: '/customers',
  })),
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
const mockCustomers = [
  { id: 1, name: 'John Doe', address: '123 Main St', email: 'john@example.com', phone: '1234567890' },
  { id: 2, name: 'Jane Smith', address: '456 Elm St', email: 'jane@example.com', phone: '0987654321' },
];

const mockApiResponse = {
  data: {
    customers: mockCustomers,
    total: 2,
  },
};

// Mock API Response
apiClient.get.mockResolvedValue(mockApiResponse);

describe('CustomerListView', () => {
  test('renders the customer list component and fetches data', async () => {
    const wrapper = mount(CustomerList, {
      global: {
        stubs: {
          'router-link': true, // Stub the router-link
          Pagination, // Use the real Pagination component
        },
      },
    });

    // Wait for the component to fetch data
    await flushPromises();

    // Assert panel heading and search input
    expect(wrapper.find('.panel-heading').text()).toBe('My Customers');
    expect(wrapper.find('input[type="text"]').attributes('placeholder')).toBe('Search');

    // Assert customers are rendered in the table
    const rows = wrapper.findAll('tbody tr');
    expect(rows).toHaveLength(mockCustomers.length);
    expect(rows[0].text()).toContain('John Doe');
    expect(rows[1].text()).toContain('Jane Smith');
  });

  test('calls the API and updates the customer list', async () => {
    const wrapper = mount(CustomerList, {
      global: {
        mocks: {
          $store: useStore(),
          $route: useRoute(),
        },
        stubs: {
          'router-link': true,
          Pagination: true,
        },
      },
    });

    // Wait for API call
    await flushPromises();

    // Assert that the API was called with correct parameters
    expect(apiClient.get).toHaveBeenCalledWith('/customers/', {
      params: {
        page: 1,
        pageSize: 10,
      },
    });

    // Assert that data was fetched and assigned
    expect(wrapper.vm.customers).toEqual(mockCustomers);
    expect(wrapper.vm.totalCount).toBe(2);
  });

  test('handles pagination correctly', async () => {
    const wrapper = mount(CustomerList, {
      global: {
        mocks: {
          $store: useStore(),
          $route: useRoute(),
        },
        stubs: {
          'router-link': true,
          Pagination: true,
        },
      },
    });

    await flushPromises();

    // Simulate changing the page
    wrapper.vm.setPage(2);
    await wrapper.vm.$nextTick();

    // Assert that the page change triggered an API call
    expect(apiClient.get).toHaveBeenCalledWith('/customers/', {
      params: {
        page: 2,
        pageSize: 10,
      },
    });
  });
});
