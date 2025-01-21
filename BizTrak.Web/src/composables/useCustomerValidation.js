import { required, email, helpers, maxLength } from '@vuelidate/validators'

export default function useCustomerValidation() {

    const phone = helpers.withMessage('invalid phone number', value => {
        const phoneRegex = /^[+]?[0-9]{10,15}$/;
        return phoneRegex.test(value) || !value || value.trim().length === 0;
    });

    return {
        name: {
            required: helpers.withMessage('name cannot be empty', required),
            maxLength: maxLength(100)
        },
        address: {
            required: helpers.withMessage('address cannot be empty', required),
            maxLength: maxLength(250)
        },
        email: {
            email
        },
        phone: {
            phone
        }
    }
}