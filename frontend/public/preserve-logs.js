// Preserve console logs across page reloads
if (typeof window !== 'undefined') {
  const originalLog = console.log;
  const originalError = console.error;
  const logs = [];
  
  console.log = function(...args) {
    logs.push({type: 'log', args, time: new Date().toISOString()});
    originalLog.apply(console, args);
  };
  
  console.error = function(...args) {
    logs.push({type: 'error', args, time: new Date().toISOString()});
    originalError.apply(console, args);
  };
  
  // Expose logs globally
  window.__PRESERVED_LOGS__ = logs;
  
  // Function to view all logs
  window.viewLogs = function() {
    console.log('=== ALL PRESERVED LOGS ===');
    logs.forEach((log, i) => {
      console.log(`[${i}] ${log.time} [${log.type}]`, ...log.args);
    });
  };
  
  console.log('Log preservation enabled. Use window.viewLogs() to see all logs.');
}
