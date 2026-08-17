import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    base: '/Task_Management/',
    plugins: [plugin()],
    server: {
        port: 53639,
    }
})
