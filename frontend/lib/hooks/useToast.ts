import toast from 'react-hot-toast';

/**
 * Custom hook for toast notifications
 * Provides a simple interface for showing success, error, info, and loading toasts
 */
export const useToast = () => {
  const success = (message: string) => {
    toast.success(message, {
      duration: 4000,
      position: 'top-right',
    });
  };

  const error = (message: string) => {
    toast.error(message, {
      duration: 5000,
      position: 'top-right',
    });
  };

  const info = (message: string) => {
    toast(message, {
      duration: 4000,
      position: 'top-right',
      icon: 'ℹ️',
    });
  };

  const loading = (message: string): string => {
    return toast.loading(message, {
      position: 'top-right',
    });
  };

  const dismiss = (toastId?: string) => {
    if (toastId) {
      toast.dismiss(toastId);
    } else {
      toast.dismiss();
    }
  };

  return {
    success,
    error,
    info,
    loading,
    dismiss,
  };
};
