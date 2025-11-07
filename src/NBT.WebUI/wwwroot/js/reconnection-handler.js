// Enhanced Blazor reconnection handler
// Addresses persistent reconnection issues in Blazor Server applications
// Based on Microsoft's issue #28592 and community solutions

window.enhancedReconnection = {
    isReconnecting: false,
    reconnectAttempts: 0,
    
    init: function() {
        console.log('Enhanced reconnection handler initialized');
    },
    
    showReconnectUI: function(attempt, maxAttempts) {
        console.log(`Reconnection attempt ${attempt} of ${maxAttempts}`);
        // You can add custom UI here if needed
    },
    
    hideReconnectUI: function() {
        console.log('Reconnection successful');
    },
    
    reloadPage: function() {
        console.log('Max retries reached. Reloading page...');
        setTimeout(() => window.location.reload(), 1000);
    }
};
