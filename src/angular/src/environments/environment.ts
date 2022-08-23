import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';
export const imagePath='https://image.tmdb.org/t/p/original/'
export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'MovieMatch',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44359',
    redirectUri: baseUrl,
    clientId: 'MovieMatch_App',
    responseType: 'code',
    scope: 'offline_access MovieMatch',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44359',
      rootNamespace: 'MovieMatch',
    },
  },
} as Environment;
