import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import tailwindcss from "@tailwindcss/vite";
import svgr from "vite-plugin-svgr";
import { env } from 'process';

export default defineConfig({
    plugins: [
        plugin(),
        tailwindcss(),
        svgr({
            svgrOptions: {
                icon: true,
                exportType: "named",
                namedExport: "ReactComponent",
            },
        }),
    ],
    server: {
        port: parseInt(env.DEV_SERVER_PORT || '3000'),
    }
});