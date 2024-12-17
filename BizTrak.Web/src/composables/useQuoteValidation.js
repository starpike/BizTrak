import { required, numeric, integer, minValue, minLength, helpers } from '@vuelidate/validators'

export function useQuoteValidation(isSubmitting) {

    const requiredIfSubmitting = (message) =>
        helpers.withMessage(message, (value) => {
            const trimmedValue = typeof value === 'string' ? value.trim() : value;
            return !isSubmitting.value || !!trimmedValue;
        });

    const isPositiveInteger = (message) =>
        helpers.withMessage(message, (value) => {
            if (value === null || value === undefined || value === '') {
                return true; // let 'required' handle it
            }
            return !isSubmitting.value || (value > 0 && Number.isInteger(value))
        });

    const minLengthRequired = (message, length) =>
        helpers.withMessage(message, (value) => {
            if (value === null || value === undefined || value === '') {
                return true; //let 'required' handle it
            }
            return !isSubmitting.value || value.length >= length
        })

    const currencyValidator = helpers.withMessage('The value must be a valid currency (e.g. 100.00)', (value) => {
        if (!isSubmitting.value) return true;
        if (value === null || value === undefined || value === '') {
            return true; // let 'required' handle it
        }
        const currencyRegex = /^\d+(\.\d{1,2})?$/; // Matches numbers with up to two decimal places
        return currencyRegex.test(value);
    });

    return {

        title: {
            required: helpers.withMessage('The title cannot be empty', required),
            minLength: minLength(3)
        },
        customer_details: {
            required: helpers.withMessage('Please select a customer', required),
        },
        tasks: {
            $each: helpers.forEach({
                description: {
                    required: requiredIfSubmitting('Please enter a description'),
                    minLength: minLengthRequired('Min length of 5', 5)
                },
                estimated_duration: {
                    required: requiredIfSubmitting('Please enter a duration'),
                },
                cost: {
                    required: requiredIfSubmitting('Please enter a cost'),
                    currency: currencyValidator
                },
            })
        },
        materials: {
            $each: helpers.forEach({
                material_name: {
                    required: requiredIfSubmitting('Please enter a material'),
                    minLength: minLengthRequired('Min length of 5', 5)
                },
                quantity: { 
                    required: requiredIfSubmitting('Please enter a quantity'),
                    integer: isPositiveInteger('Must be a positive integer > 0')
                 },
                unit_price: { 
                    required: requiredIfSubmitting('Please enter a price'),
                    currency: currencyValidator
                 },
            })
        }
    }
}