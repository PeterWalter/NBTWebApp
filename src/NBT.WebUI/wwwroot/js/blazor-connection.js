// Enhanced Blazor connection management
(function () {
    window.blazorReconnectionHandler = {
        reconnectAttempt: 0,
        maxReconnectAttempts: 20,
        reconnectDelay: 2000,
        
        startReconnection: function () {
            console.log('Starting reconnection handler...');
            
            Blazor.start({
                reconnectionOptions: {
                    maxRetries: this.maxReconnectAttempts,
                    retryIntervalMilliseconds: (retryCount) => {
                        // Exponential backoff: 2s, 4s, 8s, 16s, max 30s
                        return Math.min(2000 * Math.pow(2, retryCount), 30000);
                    }
                },
                circuit: {
                    // Configure circuit options for stability
                    reconnectionHandler: {
                        onConnectionDown: () => console.warn('Connection lost'),
                        onConnectionUp: () => console.log('Connection restored')
                    }
                }
            }).then(() => {
                console.log('Blazor started successfully');
            }).catch(err => {
                console.error('Blazor start failed:', err);
            });
        }
    };
    
    // Auto-start when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.blazorReconnectionHandler.startReconnection();
        });
    } else {
        window.blazorReconnectionHandler.startReconnection();
    }
})();
