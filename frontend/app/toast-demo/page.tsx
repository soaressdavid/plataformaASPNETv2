'use client';

import { useToast } from '@/lib/hooks/useToast';

export default function ToastDemoPage() {
  const toast = useToast();

  return (
    <div className="min-h-screen bg-gray-50 py-12 px-4">
      <div className="max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold text-gray-900 mb-8">
          Toast Notifications Demo
        </h1>
        
        <div className="bg-white rounded-lg shadow p-6 space-y-4">
          <p className="text-gray-600 mb-6">
            Click the buttons below to see different types of toast notifications:
          </p>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <button
              onClick={() => toast.success('Operation completed successfully!')}
              className="px-4 py-3 bg-green-500 text-white rounded-lg hover:bg-green-600 transition"
            >
              Show Success Toast
            </button>
            
            <button
              onClick={() => toast.error('Something went wrong. Please try again.')}
              className="px-4 py-3 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
            >
              Show Error Toast
            </button>
            
            <button
              onClick={() => toast.info('This is an informational message.')}
              className="px-4 py-3 bg-gray-500 text-white rounded-lg hover:bg-gray-600 transition"
            >
              Show Info Toast
            </button>
            
            <button
              onClick={() => {
                const loadingToast = toast.loading('Processing...');
                setTimeout(() => {
                  toast.dismiss(loadingToast);
                  toast.success('Done!');
                }, 2000);
              }}
              className="px-4 py-3 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition"
            >
              Show Loading Toast
            </button>
          </div>
          
          <div className="mt-8 pt-6 border-t border-gray-200">
            <h2 className="text-xl font-semibold text-gray-900 mb-4">
              Real-world Examples
            </h2>
            
            <div className="space-y-3">
              <button
                onClick={() => toast.success('Challenge submitted successfully! You earned 25 XP.')}
                className="w-full px-4 py-2 bg-green-100 text-green-800 rounded hover:bg-green-200 transition text-left"
              >
                Challenge Submission Success
              </button>
              
              <button
                onClick={() => toast.error('Compilation error on line 15: Missing semicolon')}
                className="w-full px-4 py-2 bg-red-100 text-red-800 rounded hover:bg-red-200 transition text-left"
              >
                Compilation Error
              </button>
              
              <button
                onClick={() => toast.error('Network error. Please check your connection.')}
                className="w-full px-4 py-2 bg-red-100 text-red-800 rounded hover:bg-red-200 transition text-left"
              >
                Network Error
              </button>
              
              <button
                onClick={() => toast.success('Welcome back, John!')}
                className="w-full px-4 py-2 bg-green-100 text-green-800 rounded hover:bg-green-200 transition text-left"
              >
                Login Success
              </button>
              
              <button
                onClick={() => toast.error('Execution timeout. Code exceeded 30 second limit.')}
                className="w-full px-4 py-2 bg-red-100 text-red-800 rounded hover:bg-red-200 transition text-left"
              >
                Timeout Error
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
