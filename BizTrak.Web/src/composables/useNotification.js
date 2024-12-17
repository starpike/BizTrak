import { useToast } from "vue-toastification";

export function useNotification() {
    const toast = useToast();
    
    return {
        showError(message = "An error occurred. Please try again.") {
            toast.error(message);
        },
        showSuccess(message = "Operation successful.") {
            toast.success(message);
        },
    };
}